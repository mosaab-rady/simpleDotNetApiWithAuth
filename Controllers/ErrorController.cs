using Api.Utils;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
public class ErrorController : ControllerBase
{
	[Route("/error")]
	public IActionResult HandleErrorDevelopment(
		[FromServices] IHostEnvironment hostEnvironment)
	{
		var exceptionHandlerFeature =
				HttpContext.Features.Get<IExceptionHandlerFeature>()!;

		if (exceptionHandlerFeature?.Error is AppException)
		{
			var err = (AppException)exceptionHandlerFeature.Error;

			return Problem(
				statusCode: err.statusCode,
				 title: err.Message,
					detail: err.StackTrace
					 );
		}
		else
		{
			var err = exceptionHandlerFeature.Error;

			return Problem(
				statusCode: 500,
				 title: err.Message,
					detail: err.StackTrace);
		}

		// return Problem(
		// 		detail: exceptionHandlerFeature.Error.StackTrace,
		// 		title: exceptionHandlerFeature.Error.Message);
	}
}