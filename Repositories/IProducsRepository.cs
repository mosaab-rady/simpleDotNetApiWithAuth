using Api.Entities;

namespace Api.Repositories;

public interface IProductsRepository
{
	Task<IEnumerable<Product>> GetAllProductsAsync();
	Task<Product> GetProductAsync(Guid Id);
	Task CreateProductAsync(Product product);
	Task UpdateProductAsync(Product product);
	Task DeleteProductAsync(Guid Id);
}