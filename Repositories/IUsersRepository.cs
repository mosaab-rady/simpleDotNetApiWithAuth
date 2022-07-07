using Api.Entities;

namespace Api.Repositories;

public interface IUsersRepository
{
	Task<IEnumerable<User>> GetAllUsersAsync();

	Task<User?> GetUserAsync(Guid Id);

	Task CreateUserAsync(User user);
	Task UpdateUserAsync(User user);
	Task DeleteUserAsync(Guid id);
}