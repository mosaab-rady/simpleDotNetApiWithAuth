using Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Context;

public class PostgresApiContext : DbContext
{
	public PostgresApiContext(DbContextOptions<PostgresApiContext> options) : base(options)
	{
	}

	public DbSet<Product> Products { get; set; }
	public DbSet<User> Users { get; set; }
}