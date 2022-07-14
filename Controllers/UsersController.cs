using Api.Entities;
using Api.Repositories;
using Api.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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


	[HttpGet]
	public async Task<OkObjectResult> GetAllUsers()
	{

		var users = await repository.GetAllUsersAsync();

		return Ok(new
		{
			result = users.Count(),
			data = users,
		});
	}


	[HttpGet("{id}")]
	public async Task<IActionResult> GetUserById(Guid id)
	{
		var user = await repository.GetUserAsync(id);

		if (user is null)
			return NotFound(new { title = "No user found with that ID." });

		return Ok(new
		{
			data = user
		});
	}
}