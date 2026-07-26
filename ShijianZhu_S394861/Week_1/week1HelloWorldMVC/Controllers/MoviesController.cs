using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using week1HelloWorldMVC.Data;
using week1HelloWorldMVC.Models;

namespace week1HelloWorldMVC.Controllers;

public class MoviesController(MovieContext context) : Controller
{
    public async Task<IActionResult> Index(string? searchString, string? rating)
    {
        ViewData["CurrentFilter"] = searchString;
        ViewData["CurrentRating"] = rating;

        IQueryable<Movie> movies = context.Movies.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(searchString))
        {
            movies = movies.Where(movie =>
                movie.Title.Contains(searchString) ||
                movie.Genre.Contains(searchString));
        }

        if (!string.IsNullOrWhiteSpace(rating))
        {
            movies = movies.Where(movie => movie.Rating == rating);
        }

        return View(await movies.OrderBy(movie => movie.Title).ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var movie = await context.Movies.AsNoTracking()
            .FirstOrDefaultAsync(item => item.Id == id);

        return movie is null ? NotFound() : View(movie);
    }

    public IActionResult Create()
    {
        return View(new Movie { ReleaseDate = DateTime.Today });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Title,ReleaseDate,Genre,Price,Rating")] Movie movie)
    {
        if (!ModelState.IsValid)
        {
            return View(movie);
        }

        context.Add(movie);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var movie = await context.Movies.FindAsync(id);
        return movie is null ? NotFound() : View(movie);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        [Bind("Id,Title,ReleaseDate,Genre,Price,Rating")] Movie movie)
    {
        if (id != movie.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(movie);
        }

        try
        {
            context.Update(movie);
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await MovieExists(movie.Id))
            {
                return NotFound();
            }

            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var movie = await context.Movies.AsNoTracking()
            .FirstOrDefaultAsync(item => item.Id == id);

        return movie is null ? NotFound() : View(movie);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var movie = await context.Movies.FindAsync(id);
        if (movie is not null)
        {
            context.Movies.Remove(movie);
            await context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private Task<bool> MovieExists(int id)
    {
        return context.Movies.AnyAsync(movie => movie.Id == id);
    }
}
