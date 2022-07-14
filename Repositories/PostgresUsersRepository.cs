using Api.Context;
using Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories;


public class PostgresUsersRepository : IUsersRepository
{

	private readonly PostgresApiContext _context;

	public PostgresUsersRepository(PostgresApiContext context)
	{
		_context = context;
	}

	public async Task<IEnumerable<User>> GetAllUsersAsync()
	{
		var users = await _context.Users.ToListAsync();

		return users;
	}


	public async Task<User> GetUserAsync(Guid Id)
	{
		var user = await _context.Users.FindAsync(Id);

		return user;
	}




	public async Task<User> GetUserByEmailAsync(string Email)
	{
		var user = await _context.Users.SingleAsync(_ => _.Email == Email);

		return user;
	}



	public async Task CreateUserAsync(User user)
	{
		await _context.Users.AddAsync(user);
		await _context.SaveChangesAsync();
	}



	public async Task UpdateUserAsync(User user)
	{
		var existUser = await _context.Users.FindAsync(user.Id);
		existUser = user;
		await _context.SaveChangesAsync();
	}


	public async Task DeleteUserAsync(Guid id)
	{
		var existUser = await _context.Users.FindAsync(id);

		_context.Users.Remove(existUser);

		await _context.SaveChangesAsync();
	}



}