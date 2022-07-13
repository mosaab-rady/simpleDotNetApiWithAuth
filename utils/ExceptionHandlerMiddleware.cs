namespace Api.Utils;


/// <summary>
///		global error handler
/// 	this middleware is not used but instead the (app.useExceptionHandler("/error")) middleware is used
/// </summary>

public class ExceptionHandlerMiddleware
{
	private readonly RequestDelegate _next;

	public ExceptionHandlerMiddleware(RequestDelegate next)
	{
		_next = next;
	}


	public async Task Invoke(HttpContext context)
	{
		try
		{
			await _next.Invoke(context);

		}
		catch (Exception err)
		{
			var response = context.Response;

			switch (err)
			{
				case AppException e:
					response.StatusCode = e.statusCode;
					await response.WriteAsJsonAsync(new
					{
						status = e.status,
						message = e.Message,
						trace = e.StackTrace
					});
					break;
				default:
					response.StatusCode = 500;
					await response.WriteAsJsonAsync(new
					{
						status = "error",
						message = "something went wrong!!!.",
						error = err.ToString()
					});
					break;
			}


		}
	}

}