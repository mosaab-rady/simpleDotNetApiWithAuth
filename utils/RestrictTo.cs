using Api.Entities;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Utils;


// must be authorization filter because of the order of execution
///<summary>
/// restrict access to only the users that have permission.
/// </summary> 
public class RestrictToAttribute : Attribute, IAuthorizationFilter
{
	public string[] Roles { get; }

	public RestrictToAttribute(params string[] roles)
	{
		Roles = roles;
	}

	public void OnAuthorization(AuthorizationFilterContext context)
	{

		var user = (User)context.HttpContext.Items["User"];

		if (Roles.Length != 0 && !Roles.Contains(user.Role))
		{
			throw new AppException("you do not have permission to perform this action.", "fail", 401);
		}
	}

	// public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
	// {
	// 	var user = (User)context.HttpContext.Items["User"];

	// 	if (Roles.Length != 0 && !Roles.Contains(user.Role))
	// 	{
	// 		throw new AppException("you do not have permission to perform this action.", "fail", 401);
	// 	}

	// 	await next();
	// }

}

