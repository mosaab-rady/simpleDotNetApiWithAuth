using System.Runtime.Serialization;

namespace Api.Utils;
public class AppException : Exception
{

	public string? status { get; }
	public int statusCode { get; }
	public AppException()
	{
	}

	public AppException(string? message) : base(message)
	{
	}

	public AppException(string? message, Exception? innerException) : base(message, innerException)
	{
	}

	protected AppException(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}

	public AppException(string message, string status, int statusCode) : this(message)
	{
		this.status = status;
		this.statusCode = statusCode;
	}
}