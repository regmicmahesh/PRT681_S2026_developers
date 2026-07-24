using Microsoft.EntityFrameworkCore;
using week1HelloWorldMVC.Data;

namespace week1HelloWorldMVC.Models;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using var context = new MvcMovieContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<MvcMovieContext>>()
        );

        // Do not add duplicates if movies already exist.
        if (context.Movie.Any())
        {
            return;
        }

        context.Movie.AddRange(
            new Movie
            {
                Title = "The Lion King",
                ReleaseDate = new DateTime(1994, 6, 15),
                Genre = "Animation",
                Price = 7.99M
            },
            new Movie
            {
                Title = "Finding Nemo",
                ReleaseDate = new DateTime(2003, 5, 30),
                Genre = "Animation",
                Price = 8.99M
            },
            new Movie
            {
                Title = "The Incredibles",
                ReleaseDate = new DateTime(2004, 11, 5),
                Genre = "Action",
                Price = 9.99M
            },
            new Movie
            {
                Title = "Toy Story",
                ReleaseDate = new DateTime(1995, 11, 22),
                Genre = "Animation",
                Price = 6.99M
            }
        );

        context.SaveChanges();
    }
}