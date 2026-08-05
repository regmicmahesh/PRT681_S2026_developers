using Microsoft.AspNetCore.Mvc;
using SimpleCrudAppAPI.Data;
using SimpleCrudAppAPI.Models;

namespace SimpleCrudAppAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductRepository _repository;

    public ProductController(IProductRepository repository)
    {
        _repository = repository;
    }

    [HttpPost("insert")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProductResponse>> InsertProduct([FromBody] CreateProductRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return BadRequest(new { error = "Product name is required." });

        if (request.Price <= 0)
            return BadRequest(new { error = "Product price must be greater than zero." });

        var product = new Product
        {
            Name = request.Name,
            Price = request.Price,
            Description = request.Description ?? string.Empty
        };

        var productId = await _repository.InsertProductAsync(product);
        var createdProduct = await _repository.GetProductByIdAsync(productId);

        return CreatedAtAction(nameof(GetProductById), new { id = productId }, 
            new ProductResponse 
            { 
                Id = createdProduct!.Id,
                Name = createdProduct.Name,
                Price = createdProduct.Price,
                Description = createdProduct.Description
            });
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductResponse>> GetProductById(int id)
    {
        var product = await _repository.GetProductByIdAsync(id);
        if (product == null)
            return NotFound(new { error = $"Product with id {id} not found." });

        return Ok(new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Description = product.Description
        });
    }

    [HttpGet("all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ProductResponse>>> GetAllProducts()
    {
        var products = await _repository.GetAllProductsAsync();
        var response = products.Select(p => new ProductResponse
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            Description = p.Description
        });

        return Ok(response);
    }
}

public class CreateProductRequest
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? Description { get; set; }
}

public class ProductResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
}
