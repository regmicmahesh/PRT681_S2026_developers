using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheaterAdmin.Api.Data;
using TheaterAdmin.Api.DTOs;
using TheaterAdmin.Api.Models;

namespace TheaterAdmin.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly TheaterAdminContext _context;

    public CategoriesController(TheaterAdminContext context)
    {
        _context = context;
    }

    // GET: api/categories
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>>
        GetCategories()
    {
        var categories = await _context.Categories
            .AsNoTracking()
            .OrderBy(category => category.Name)
            .Select(category => new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Code = category.Code
            })
            .ToListAsync();

        return Ok(categories);
    }

    // GET: api/categories/5
    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDto>>
        GetCategory(int id)
    {
        var category = await _context.Categories
            .AsNoTracking()
            .Where(category => category.Id == id)
            .Select(category => new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Code = category.Code
            })
            .FirstOrDefaultAsync();

        if (category == null)
        {
            return NotFound();
        }

        return Ok(category);
    }

    // POST: api/categories
    [HttpPost]
    public async Task<ActionResult<CategoryDto>>
        CreateCategory(CategoryRequest request)
    {
        var category = new Category
        {
            Name = request.Name.Trim(),
            Code = request.Code.Trim().ToUpper()
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        var categoryDto = new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Code = category.Code
        };

        return CreatedAtAction(
            nameof(GetCategory),
            new { id = category.Id },
            categoryDto);
    }

    // PUT: api/categories/5
    [HttpPut("{id}")]
    public async Task<IActionResult>
        UpdateCategory(int id, CategoryRequest request)
    {
        var category = await _context.Categories.FindAsync(id);

        if (category == null)
        {
            return NotFound();
        }

        category.Name = request.Name.Trim();
        category.Code = request.Code.Trim().ToUpper();

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/categories/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var category = await _context.Categories.FindAsync(id);

        if (category == null)
        {
            return NotFound();
        }

        var containsMovies = await _context.Movies
            .AnyAsync(movie => movie.CategoryId == id);

        if (containsMovies)
        {
            return Conflict(new
            {
                message =
                    "This category cannot be deleted because it contains movies."
            });
        }

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}