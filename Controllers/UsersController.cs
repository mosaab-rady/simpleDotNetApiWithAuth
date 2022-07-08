using Api.Entities;
using Api.Repositories;
using Api.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;


[ApiController]
[Route("users")]
public class UsersController : ControllerBase
{
	private readonly IUsersRepository repository;

	public UsersController(IUsersRepository repository)
	{
		this.repository = repository;
	}

	[HttpGet]
	public async Task GetAllUsers()
	{
		var users = await repository.GetAllUsersAsync();

		Response.StatusCode = 200;
		await Response.WriteAsJsonAsync(new
		{
			status = "success",
			result = users.Count(),
			data = users
		});
	}


	[HttpGet("{id}")]
	public async Task GetUserById(Guid id)
	{
		var user = await repository.GetUserAsync(id);
		if (user is null) throw new AppException("No user found with that ID.", "fail", 404);

		Response.StatusCode = 200;
		await Response.WriteAsJsonAsync(new
		{
			status = "success",
			data = user
		});
	}


	[HttpPost("signup")]
	public async Task SignUp(CreateUSer user)
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

		Response.StatusCode = 201;
		await Response.WriteAsJsonAsync(new
		{
			status = "success",
			data = NewUser
		});
	}

}