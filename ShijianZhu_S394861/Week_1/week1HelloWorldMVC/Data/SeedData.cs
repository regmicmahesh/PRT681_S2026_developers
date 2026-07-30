using Microsoft.EntityFrameworkCore;
using week1HelloWorldMVC.Models;

namespace week1HelloWorldMVC.Data;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider services)
    {
        await using var scope = services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<MovieContext>();

        await context.Database.MigrateAsync();

        if (await context.Movies.AnyAsync())
        {
            return;
        }

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
