using Api.Entities;

namespace Api.Repositories;

public class InMemUsersRepository : IUsersRepository
{
	private readonly List<User> users = new()
	{
		new User {Id=Guid.NewGuid(),FirstName="Joe",LastName="Ali",Email="joe@examle.com" , Password="test1234",Role="admin"},
		new User {Id=Guid.NewGuid(),FirstName="Mohamed",LastName="Ali",Email="mohamed@example.com" ,Password="test1234",Role="user"},
		new User {Id=Guid.NewGuid(),FirstName="Khaled",LastName="Ali",Email="khaled@example.com" ,Password="test1234",Role="user"},
		new User {Id=Guid.NewGuid(),FirstName="Ali",LastName="Hassan",Email="ali@example.com" ,Password="test1234",Role="user"},
	};


	public async Task<IEnumerable<User>> GetAllUsersAsync()
	{
		return await Task.FromResult(users);
	}

	public async Task<User?> GetUserAsync(Guid Id)
	{
		var user = users.Where(x => x.Id == Id).SingleOrDefault();
		return await Task.FromResult(user);
	}

	public async Task CreateUserAsync(User user)
	{
		users.Add(user);
		await Task.CompletedTask;
	}

	public async Task UpdateUserAsync(User user)
	{
		var index = users.FindIndex(x => x.Id == user.Id);
		users[index] = user;
		await Task.CompletedTask;
	}
	public async Task DeleteUserAsync(Guid id)
	{
		var index = users.FindIndex(x => x.Id == id);
		users.RemoveAt(index);
		await Task.CompletedTask;
	}


}
