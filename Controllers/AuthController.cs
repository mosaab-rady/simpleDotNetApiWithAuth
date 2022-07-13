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
			token,
			data = user
		});
	}



	[HttpPost("login")]
	public async Task<IActionResult> Login(LoginUser _user)
	{
		var user = await repository.GetUserByEmailAsync(_user.Email);

		if (user == null || !BCrypt.Net.BCrypt.Verify(_user.Password, user.Password))
		{
			return BadRequest(new { title = "Encorrect Email OR Password." });
			// throw new AppException("Encorrect Email OR Password", "fail", 400);
		}

		string tokenString = createToken(user);


		return Ok(new
		{
			data = tokenString
		});
	}


	[HttpPost("logout")]
	[TypeFilter(typeof(ProtectAttribute))]
	public IActionResult logout()
	{
		// HttpContext.Items["User"] = null;
		Response.Cookies.Delete("jwt");

		return Ok();
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



		var cookieOptions = new CookieOptions()
		{
			HttpOnly = true,
			Expires = DateTimeOffset.Now.AddHours(1)
		};


		Response.Cookies.Append("jwt", tokenString, cookieOptions);

		return tokenString;
	}

}