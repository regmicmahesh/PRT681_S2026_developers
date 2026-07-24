using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using week1HelloWorldMVC.Data;

namespace week1HelloWorldMVC.Controllers;

public class MoviesController : Controller
{
    private readonly MvcMovieContext _context;

    public MoviesController(MvcMovieContext context)
    {
        _context = context;
    }

    // URL: /Movies
    public async Task<IActionResult> Index()
    {
        var movies = await _context.Movie
            .OrderBy(movie => movie.Title)
            .ToListAsync();

        return View(movies);
    }
}