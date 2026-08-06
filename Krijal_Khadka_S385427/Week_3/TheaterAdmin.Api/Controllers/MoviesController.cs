using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheaterAdmin.Api.Data;
using TheaterAdmin.Api.DTOs;
using TheaterAdmin.Api.Models;

namespace TheaterAdmin.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MoviesController : ControllerBase
{
    private readonly TheaterAdminContext _context;

    public MoviesController(TheaterAdminContext context)
    {
        _context = context;
    }

    // GET: api/movies
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MovieDto>>>
        GetMovies()
    {
        var movies = await _context.Movies
            .AsNoTracking()
            .OrderBy(movie => movie.Name)
            .Select(movie => new MovieDto
            {
                Id = movie.Id,
                Name = movie.Name,
                Director = movie.Director,
                Email = movie.Email,
                ReleaseDate = movie.ReleaseDate,
                Language = movie.Language,
                CategoryId = movie.CategoryId,
                CategoryName = movie.Category != null
                    ? movie.Category.Name
                    : string.Empty,
                CategoryCode = movie.Category != null
                    ? movie.Category.Code
                    : string.Empty
            })
            .ToListAsync();

        return Ok(movies);
    }

    // GET: api/movies/5
    [HttpGet("{id}")]
    public async Task<ActionResult<MovieDto>> GetMovie(int id)
    {
        var movie = await _context.Movies
            .AsNoTracking()
            .Where(movie => movie.Id == id)
            .Select(movie => new MovieDto
            {
                Id = movie.Id,
                Name = movie.Name,
                Director = movie.Director,
                Email = movie.Email,
                ReleaseDate = movie.ReleaseDate,
                Language = movie.Language,
                CategoryId = movie.CategoryId,
                CategoryName = movie.Category != null
                    ? movie.Category.Name
                    : string.Empty,
                CategoryCode = movie.Category != null
                    ? movie.Category.Code
                    : string.Empty
            })
            .FirstOrDefaultAsync();

        if (movie == null)
        {
            return NotFound();
        }

        return Ok(movie);
    }

    // POST: api/movies
    [HttpPost]
    public async Task<ActionResult<MovieDto>>
        CreateMovie(MovieRequest request)
    {
        var categoryExists = await _context.Categories
            .AnyAsync(category => category.Id == request.CategoryId);

        if (!categoryExists)
        {
            ModelState.AddModelError(
                nameof(request.CategoryId),
                "The selected category does not exist.");

            return ValidationProblem(ModelState);
        }

        var movie = new Movie
        {
            Name = request.Name.Trim(),
            Director = request.Director.Trim(),
            Email = request.Email.Trim(),
            ReleaseDate = request.ReleaseDate,
            Language = request.Language,
            CategoryId = request.CategoryId
        };

        _context.Movies.Add(movie);
        await _context.SaveChangesAsync();

        var movieDto = await FindMovieDto(movie.Id);

        return CreatedAtAction(
            nameof(GetMovie),
            new { id = movie.Id },
            movieDto);
    }

    // PUT: api/movies/5
    [HttpPut("{id}")]
    public async Task<IActionResult>
        UpdateMovie(int id, MovieRequest request)
    {
        var movie = await _context.Movies.FindAsync(id);

        if (movie == null)
        {
            return NotFound();
        }

        var categoryExists = await _context.Categories
            .AnyAsync(category => category.Id == request.CategoryId);

        if (!categoryExists)
        {
            ModelState.AddModelError(
                nameof(request.CategoryId),
                "The selected category does not exist.");

            return ValidationProblem(ModelState);
        }

        movie.Name = request.Name.Trim();
        movie.Director = request.Director.Trim();
        movie.Email = request.Email.Trim();
        movie.ReleaseDate = request.ReleaseDate;
        movie.Language = request.Language;
        movie.CategoryId = request.CategoryId;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/movies/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMovie(int id)
    {
        var movie = await _context.Movies.FindAsync(id);

        if (movie == null)
        {
            return NotFound();
        }

        _context.Movies.Remove(movie);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private Task<MovieDto?> FindMovieDto(int id)
    {
        return _context.Movies
            .AsNoTracking()
            .Where(movie => movie.Id == id)
            .Select(movie => new MovieDto
            {
                Id = movie.Id,
                Name = movie.Name,
                Director = movie.Director,
                Email = movie.Email,
                ReleaseDate = movie.ReleaseDate,
                Language = movie.Language,
                CategoryId = movie.CategoryId,
                CategoryName = movie.Category != null
                    ? movie.Category.Name
                    : string.Empty,
                CategoryCode = movie.Category != null
                    ? movie.Category.Code
                    : string.Empty
            })
            .FirstOrDefaultAsync();
    }
}