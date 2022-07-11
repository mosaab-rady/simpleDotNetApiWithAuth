using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Api.Entities;
using Api.Repositories;
using Api.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Api.Controllers;


[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
	private readonly IUsersRepository repository;
	private readonly IConfiguration configuration;

	public AuthController(IUsersRepository _repository, IConfiguration configuration)
	{
		repository = _repository;
		this.configuration = configuration;
	}


	[HttpPost("signup")]
	public async Task SignUp(CreateUSer user)
	{
		var HashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);

		var NewUser = new User
		{
			Id = Guid.NewGuid(),
			FirstName = user.FirstName,
			LastName = user.LastName,
			Email = user.Email,
			Password = HashedPassword,
			Role = user.Role
		};

		await repository.CreateUserAsync(NewUser);

		Response.StatusCode = 201;
		await Response.WriteAsJsonAsync(new
		{
			status = "success",
			data = user
		});
	}



	[HttpPost("login")]
	public async Task Login(LoginUser _user)
	{
		var user = await repository.GetUserByEmailAsync(_user.Email);

		if (user == null || !BCrypt.Net.BCrypt.Verify(_user.Password, user.Password))
		{
			throw new AppException("Encorrect Email OR Password", "fail", 400);
		}

		var claims = new[]
				{
						new Claim("Id",user.Id.ToString()),
						new Claim(ClaimTypes.NameIdentifier, user.FirstName),
						new Claim("Email", user.Email),
						new Claim(ClaimTypes.Role, user.Role)
				};


		var token = new JwtSecurityToken
						 (
								 issuer: configuration["JWT:Issuer"],
								 audience: configuration["JWT:Audience"],
								 claims: claims,
								 expires: DateTime.UtcNow.AddDays(60),
								 notBefore: DateTime.UtcNow,
								 signingCredentials: new SigningCredentials(
										 new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"])),
										 SecurityAlgorithms.HmacSha256)
						 );

		var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

		Response.Cookies.Append("jwt", tokenString);

		Response.StatusCode = 200;
		await Response.WriteAsJsonAsync(new
		{
			status = "success",
			data = tokenString
		});

	}


	// private async Task CreateSendCookie(User user, int statusCode)
	// {
	// 	Response.Cookies.Append("jwt", "super_secret_token");


	// 	Response.StatusCode = statusCode;
	// 	await Response.WriteAsJsonAsync(new
	// 	{
	// 		status = "success",
	// 		data = user
	// 	});

	// }



}