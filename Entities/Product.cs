using System.ComponentModel.DataAnnotations;

namespace Api.Entities;

public class Product
{
	[Required]
	public Guid Id { get; set; }
	[Required]
	public string? Name { get; set; }
	[Required]
	public decimal Price { get; set; }
}

public record CreateProduct([Required] string Name, [Range(1, 1000)] decimal Price);
public record UpdateProduct(string Name, [Range(1, 1000)] decimal Price);
