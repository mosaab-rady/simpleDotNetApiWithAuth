using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Api.Entities;


public class User
{
	[Required]
	public Guid Id { get; set; }
	[Required]
	public string FirstName { get; set; }
	[Required]
	public string LastName { get; set; }

	[EmailAddress]
	[Required]
	public string Email { get; set; }

	// [JsonIgnore]
	[Required]
	public string Password { get; set; }
	[Required]
	public string Role { get; set; }

}


public class CreateUSer
{
	[Required(ErrorMessage = "Please provide your first name.")]
	public string FirstName { get; set; }


	[Required(ErrorMessage = "Please provide your last name.")]
	public string LastName { get; set; }

	[Required(ErrorMessage = "Please provide your Email.")]
	[EmailAddress(ErrorMessage = "Please provide valid Email address.")]
	public string Email { get; set; }


	[Required(ErrorMessage = "Please provide your password.")]
	public string Password { get; set; }


	[Required]
	[Compare("Password", ErrorMessage = "Password and Confirm Password must match.")]
	public string ConfirmPassword { get; set; }

	public string Role { get; set; } = "user";
}



public class LoginUser
{
	[Required]
	public string Email { get; set; }


	[Required]
	public string Password { get; set; }

}

