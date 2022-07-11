using Api.Entities;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Utils;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter
{


	public string[] Roles { get; set; }
	public CustomAuthorizeAttribute(params string[] Role)
	{
		this.Roles = Role;
	}
	public void OnAuthorization(AuthorizationFilterContext context)
	{
		var user = (User)context.HttpContext.Items["User"];

		if (user is null)
		{
			throw new AppException("You are not logged in! Please log in to get access", "fail", 401);
		}

		if (Roles.Length != 0 && !Roles.Contains(user.Role))
		{
			throw new AppException("you do not have permission to perform this action.", "fail", 401);
		}
	}
}
