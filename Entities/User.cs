using System.Text.Json.Serialization;

namespace Api.Entities;


public class User
{
	public Guid Id { get; set; }
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	[JsonIgnore]
	public string? Password { get; set; }
	public string? Role { get; set; }

}



