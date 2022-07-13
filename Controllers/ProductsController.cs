using Api.Entities;
using Api.Repositories;
using Api.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;


[ApiController]
[Route("products")]
public class ProductsController : ControllerBase
{
	private readonly IProductsRepository repository;

	public ProductsController(IProductsRepository _repository)
	{
		this.repository = _repository;
	}

	// [CustomAuthorize]
	[HttpGet]
	[TypeFilter(typeof(ProtectAttribute))]
	// [RestrictTo("admin")]
	public async Task<OkObjectResult> GetProductsAsync()
	{
		var products = await repository.GetAllProductsAsync();

		return Ok(new
		{
			result = products.Count(),
			data = products
		});

		// Response.ContentType = "application/json";
		// return StatusCode(200, new
		// {
		// 	status = "success",
		// 	result = products.Count(),
		// 	data = products
		// });
		// await Response.WriteAsJsonAsync(new
		// {
		// 	status = "success",
		// 	result = products.Count(),
		// 	data = products
		// });
	}


	[HttpGet("{id}")]
	public async Task<IActionResult> GetProductById(Guid id)
	{
		var product = await repository.GetProductAsync(id);

		if (product is null)
		{
			return NotFound(new { type = "fail", title = "No product found with that ID." });
			// throw new AppException("No Product found with that ID.", "error", 404);
		}


		return Ok(new
		{
			data = product
		});

		// Response.StatusCode = 200;
		// await Response.WriteAsJsonAsync(new
		// {
		// 	status = "success",
		// 	data = product
		// });
	}


	// [CustomAuthorize("admin")]
	[HttpPost]
	[TypeFilter(typeof(ProtectAttribute))]
	[RestrictTo("admin")]
	public async Task<CreatedAtActionResult> CreateProduct(CreateProduct _product)
	{
		var product = new Product()
		{
			Id = Guid.NewGuid(),
			Name = _product.Name,
			Price = _product.Price,
		};
		await repository.CreateProductAsync(product);


		return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, new
		{
			data = product
		});

		// Response.StatusCode = 201;
		// await Response.WriteAsJsonAsync(new
		// {
		// 	status = "success",
		// 	data = product
		// });
	}



	// [CustomAuthorize("admin")]
	[HttpPatch("{id}")]
	[TypeFilter(typeof(ProtectAttribute))]
	[RestrictTo("admin")]
	public async Task<IActionResult> UpdateProductById(Guid id, UpdateProduct _product)
	{
		var product = await repository.GetProductAsync(id);
		if (product is null)
		{
			return NotFound(new { title = "No product found with that ID." });
			// throw new AppException("No product found with that ID.", "fail", 404);
		}

		product.Name = _product.Name;
		product.Price = _product.Price;

		await repository.UpdateProductAsync(product);


		return Ok(new
		{
			data = product
		});

		// Response.StatusCode = 200;
		// await Response.WriteAsJsonAsync(new
		// {
		// 	status = "success",
		// 	data = product
		// });
	}




	// [CustomAuthorize("admin")]
	[HttpDelete("{id}")]
	[TypeFilter(typeof(ProtectAttribute))]
	[RestrictTo("admin")]
	public async Task<IActionResult> DeleteProductById(Guid id)
	{
		var product = await repository.GetProductAsync(id);
		if (product is null)
		{
			return NotFound(new { title = "No product found with that ID." });
			throw new AppException("No product found with that ID.", "fail", 404);

		}

		await repository.DeleteProductAsync(id);

		// return StatusCode(204);

		return NoContent();

		// Response.StatusCode = 204;
		// await Response.WriteAsJsonAsync(new
		// {
		// 	status = "success"
		// });
	}

}