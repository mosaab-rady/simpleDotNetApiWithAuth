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
	public async Task<CreatedAtActionResult> SignUp(CreateUSer user)
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

		string token = createToken(NewUser);

		return CreatedAtAction("signup", new
		{
			status = "success",
			token,
			data = user
		});

		// Response.StatusCode = 201;
		// await Response.WriteAsJsonAsync(new
		// {
		// 	status = "success",
		// 	data = user
		// });
	}



	[HttpPost("login")]
	public async Task<IActionResult> Login(LoginUser _user)
	{
		var user = await repository.GetUserByEmailAsync(_user.Email);

		if (user == null || !BCrypt.Net.BCrypt.Verify(_user.Password, user.Password))
		{
			return BadRequest(new { status = "fail", title = "Encorrect Email OR Password." });
			// throw new AppException("Encorrect Email OR Password", "fail", 400);
		}

		string tokenString = createToken(user);


		return Ok(new
		{
			status = "success",
			data = tokenString
		});

		// Response.StatusCode = 200;
		// await Response.WriteAsJsonAsync(new
		// {
		// 	status = "success",
		// 	data = tokenString
		// });

	}


	private string createToken(User user)
	{
		var claims = new[]
						{
						new Claim("Id",user.Id.ToString()),
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

		// Response.Cookies.Append("jwt", tokenString);

		return tokenString;
	}

}