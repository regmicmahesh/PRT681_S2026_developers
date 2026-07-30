using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MvcMusic.Data;
using System;
using System.Linq;

namespace MvcMusic.Models;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new MvcMusicContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<MvcMusicContext>>()))
        {
            // Look for any music records.
            if (context.Music.Any())
            {
                return;   // DB has been seeded
            }
            context.Music.AddRange(
                new Music
                {
                    Title = "Thriller",
                    Artist = "Michael Jackson",
                    ReleaseDate = DateTime.Parse("1982-11-30"),
                    Genre = "Pop",
                    Price = 9.99M
                },
                new Music
                {
                    Title = "Back in Black",
                    Artist = "AC/DC",
                    ReleaseDate = DateTime.Parse("1980-7-25"),
                    Genre = "Rock",
                    Price = 8.99M
                },
                new Music
                {
                    Title = "The Dark Side of the Moon",
                    Artist = "Pink Floyd",
                    ReleaseDate = DateTime.Parse("1973-3-1"),
                    Genre = "Rock",
                    Price = 10.99M
                },
                new Music
                {
                    Title = "The Bodyguard",
                    Artist = "Whitney Houston",
                    ReleaseDate = DateTime.Parse("1992-11-17"),
                    Genre = "Soundtrack",
                    Price = 7.99M
                }
            );
            context.SaveChanges();
        }
    }
}
