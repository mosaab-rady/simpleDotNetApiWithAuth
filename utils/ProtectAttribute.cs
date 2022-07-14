using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Api.Repositories;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;

namespace Api.Utils;
// must be authorization filter because the order of execution is important
// the authorization filter is executed first before valdition and action filters
/// <summary>
/// authorize middleware
/// make sure the user is logged in and exist.
/// </summary>
public class ProtectAttribute : Attribute, IAsyncAuthorizationFilter
{
	private readonly IConfiguration _configuration;
	private readonly IUsersRepository _repository;



	public ProtectAttribute(IConfiguration configuration, IUsersRepository repository)
	{
		_configuration = configuration;
		_repository = repository;
	}



	public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
	{

		// get token from cookies
		var token = context.HttpContext.Request.Cookies["jwt"];

		if (token is null)
		{
			throw new AppException("You are not logged in! Please log in to get access", "fail", 401);
		}


		var tokenHandler = new JwtSecurityTokenHandler();
		var key = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);

		tokenHandler.ValidateToken(token, new TokenValidationParameters()
		{

			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = _configuration["JWT:Issuer"],
			ValidAudience = _configuration["JWT:Audience"],
			IssuerSigningKey = new SymmetricSecurityKey(key)
		}, out SecurityToken validatedToken);


		var jwtToken = (JwtSecurityToken)validatedToken;



		var Id = jwtToken.Claims.First(claim => claim.Type == "Id").Value;

		var user = await _repository.GetUserAsync(Guid.Parse(Id));

		if (user is null)
		{
			throw new AppException("The user belongs to this token no longer exist", "fail", 400);
		}

		context.HttpContext.Items["User"] = user;

	}





	// public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
	// {

	// 	// get token from bearer authorization
	// 	// var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();


	// 	// get token from cookies
	// 	var token = context.HttpContext.Request.Cookies["jwt"];

	// 	if (token is null)
	// 	{
	// 		throw new AppException("You are not logged in! Please log in to get access", "fail", 401);
	// 	}


	// 	var tokenHandler = new JwtSecurityTokenHandler();
	// 	var key = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);

	// 	tokenHandler.ValidateToken(token, new TokenValidationParameters()
	// 	{

	// 		ValidateAudience = true,
	// 		ValidateLifetime = true,
	// 		ValidateIssuerSigningKey = true,
	// 		ValidIssuer = _configuration["JWT:Issuer"],
	// 		ValidAudience = _configuration["JWT:Audience"],
	// 		IssuerSigningKey = new SymmetricSecurityKey(key)
	// 	}, out SecurityToken validatedToken);


	// 	var jwtToken = (JwtSecurityToken)validatedToken;



	// 	var Id = jwtToken.Claims.First(claim => claim.Type == "Id").Value;

	// 	var user = await _repository.GetUserAsync(Guid.Parse(Id));

	// 	if (user is null)
	// 	{
	// 		throw new AppException("The user belongs to this token no longer exist", "fail", 400);
	// 	}

	// 	context.HttpContext.Items["User"] = user;

	// 	await next();
	// }

}
