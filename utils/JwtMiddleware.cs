using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Api.Repositories;
using Microsoft.IdentityModel.Tokens;

namespace Api.Utils;



/// <summary>
/// used to capture the token, 
/// It is global so even if the route does not use authorize the request will have a user,
/// It`s not used instead Protect Attribute is used
/// </summary>

public class JwtMiddleware
{
	private readonly RequestDelegate _next;
	private readonly IConfiguration _configuration;
	private readonly IUsersRepository _repository;

	public JwtMiddleware(RequestDelegate next, IConfiguration configuration, IUsersRepository repository)
	{
		_next = next;
		_configuration = configuration;
		_repository = repository;
	}


	public async Task Invoke(HttpContext context)
	{
		var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

		if (token != null)
		{
			attachAccountToContext(context, token);
		}

		await _next(context);

	}

	private async void attachAccountToContext(HttpContext context, string token)
	{
		try
		{

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

			// var user = await _repository.GetUserByEmailAsync(email);

			// if (user is null)
			// {
			// 	throw new AppException("The user belongs to this token no longer exist", "fail", 400);
			// }

			context.Items["User"] = user;

		}
		catch
		{
			// context.Response.StatusCode = err.statusCode;
			// await context.Response.WriteAsJsonAsync(new
			// {
			// 	status = err.status,
			// 	message = err.Message
			// });

		}
		// catch (Exception e)
		// {
		// 	// context.Response.StatusCode = 500;
		// 	// await context.Response.WriteAsJsonAsync(new
		// 	// {
		// 	// 	status = "error",
		// 	// 	message = e.Message
		// 	// });
		// }



	}

}