using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using week1HelloWorldMVC.Controllers;
using week1HelloWorldMVC.Data;
using week1HelloWorldMVC.Models;

namespace week1HelloWorldMVC.Tests;

public class MoviesControllerTests
{
    [Fact]
    public async Task Index_SearchesMovieTitleAndGenre()
    {
        await using var context = CreateContext();
        await AddMoviesAsync(context);
        var controller = new MoviesController(context);

        var actionResult = await controller.Index("Darwin", rating: null);

        var view = Assert.IsType<ViewResult>(actionResult);
        var movies = Assert.IsAssignableFrom<IEnumerable<Movie>>(view.Model).ToList();
        var movie = Assert.Single(movies);
        Assert.Equal("Darwin Skies", movie.Title);
        Assert.Equal("Darwin", view.ViewData["CurrentFilter"]);
    }

    [Fact]
    public async Task Index_FiltersMoviesByRating()
    {
        await using var context = CreateContext();
        await AddMoviesAsync(context);
        var controller = new MoviesController(context);

        var actionResult = await controller.Index(searchString: null, rating: "G");

        var view = Assert.IsType<ViewResult>(actionResult);
        var movies = Assert.IsAssignableFrom<IEnumerable<Movie>>(view.Model).ToList();
        var movie = Assert.Single(movies);
        Assert.Equal("Northern Lights", movie.Title);
        Assert.Equal("G", movie.Rating);
        Assert.Equal("G", view.ViewData["CurrentRating"]);
    }

    [Fact]
    public async Task Index_CombinesTextAndRatingFilters()
    {
        await using var context = CreateContext();
        await AddMoviesAsync(context);
        var controller = new MoviesController(context);

        var actionResult = await controller.Index("Science", rating: "M");

        var view = Assert.IsType<ViewResult>(actionResult);
        var movies = Assert.IsAssignableFrom<IEnumerable<Movie>>(view.Model).ToList();
        var movie = Assert.Single(movies);
        Assert.Equal("Code Runner", movie.Title);
    }

    private static MovieContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<MovieContext>()
            .UseInMemoryDatabase($"MoviesControllerTests-{Guid.NewGuid()}")
            .Options;

        return new MovieContext(options);
    }

    private static async Task AddMoviesAsync(MovieContext context)
    {
        context.Movies.AddRange(
            new Movie
            {
                Title = "Darwin Skies",
                ReleaseDate = new DateTime(2024, 5, 10),
                Genre = "Drama",
                Price = 12.50m,
                Rating = "PG",
            },
            new Movie
            {
                Title = "Code Runner",
                ReleaseDate = new DateTime(2025, 2, 14),
                Genre = "Science Fiction",
                Price = 15.00m,
                Rating = "M",
            },
            new Movie
            {
                Title = "Northern Lights",
                ReleaseDate = new DateTime(2023, 8, 21),
                Genre = "Documentary",
                Price = 9.95m,
                Rating = "G",
            }
        );

        await context.SaveChangesAsync();
    }
}
