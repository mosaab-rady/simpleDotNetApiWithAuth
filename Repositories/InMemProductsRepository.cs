using Api.Entities;

namespace Api.Repositories;

public class InMemProductsRepository : IProductsRepository
{

	private readonly List<Product> products = new()
	{
		new Product {Id=Guid.NewGuid(), Name="t-shirt", Price=45},
		new Product {Id=Guid.NewGuid(), Name="long-sleeve", Price=55},
		new Product {Id=Guid.NewGuid(), Name="sweet-shirt", Price=112}
	};


	public async Task<IEnumerable<Product>> GetAllProductsAsync()
	{
		return await Task.FromResult(products);
	}



	public async Task<Product> GetProductAsync(Guid Id)
	{
		var product = products.Where(x => x.Id == Id).SingleOrDefault();
		return await Task.FromResult(product);
	}



	public async Task CreateProductAsync(Product product)
	{
		products.Add(product);
		await Task.CompletedTask;
	}


	public async Task UpdateProductAsync(Product product)
	{
		var index = products.FindIndex(x => x.Id == product.Id);

		products[index] = product;

		await Task.CompletedTask;
	}

	public async Task DeleteProductAsync(Guid Id)
	{
		var index = products.FindIndex(x => x.Id == Id);

		products.RemoveAt(index);

		await Task.CompletedTask;
	}




}
