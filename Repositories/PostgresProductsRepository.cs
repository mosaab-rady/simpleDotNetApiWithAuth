using Api.Context;
using Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories;

public class PostgresProductsRepository : IProductsRepository
{

	private readonly PostgresApiContext _context;

	public PostgresProductsRepository(PostgresApiContext context)
	{
		_context = context;
	}


	public async Task<IEnumerable<Product>> GetAllProductsAsync()
	{
		var products = await _context.Products.ToListAsync();

		return products;

	}

	public async Task<Product> GetProductAsync(Guid Id)
	{
		var product = await _context.Products.FindAsync(Id);

		return product;
	}



	public async Task CreateProductAsync(Product product)
	{
		await _context.Products.AddAsync(product);

		await _context.SaveChangesAsync();

	}

	public async Task UpdateProductAsync(Product product)
	{
		var existProduct = await _context.Products.FindAsync(product.Id);
		existProduct = product;
		await _context.SaveChangesAsync();

	}


	public async Task DeleteProductAsync(Guid Id)
	{
		var existProduct = await _context.Products.FindAsync(Id);

		_context.Products.Remove(existProduct);

		await _context.SaveChangesAsync();



	}



}
