using Api.Entities;
using Api.Repositories;
using Api.Utils;
using Microsoft.AspNetCore.Authorization;
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


	[Authorize]
	[HttpGet]
	public async Task GetAllUsers()
	{

		var users = await repository.GetAllUsersAsync();

		Response.StatusCode = 200;
		await Response.WriteAsJsonAsync(new
		{
			status = "success",
			result = users.Count(),
			data = users,
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
}