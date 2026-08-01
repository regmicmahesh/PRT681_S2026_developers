using Microsoft.EntityFrameworkCore;

namespace week2TheaterAdmin.Models;

public static class SeedData
{
    public static void Initialise(IServiceProvider serviceProvider)
    {
        using (var context = new week2TheaterAdminContext(
            serviceProvider.GetRequiredService<DbContextOptions<week2TheaterAdminContext>>()))
        {
            if (context.Movie.Any())
            {
                return; // DB already seeded
            }

            var action = new Category { CategoryCode = "ACT", CategoryName = "Action" };
            var drama = new Category { CategoryCode = "DRM", CategoryName = "Drama" };
            var horror = new Category { CategoryCode = "HOR", CategoryName = "Horror" };

            context.Category.AddRange(action, drama, horror);

            context.Movie.AddRange(
                new Movie
                {
                    MovieName = "Rogue Horizon",
                    ReleaseDate = new DateOnly(2024, 6, 14),
                    Director = "Alex Carter",
                    ContactEmailAddress = "info@rogue-horizon.com",
                    Language = Language.English,
                    Category = action
                },
                new Movie
                {
                    MovieName = "Blast Radius",
                    ReleaseDate = new DateOnly(2022, 3, 9),
                    Director = "Wei Zhang",
                    ContactEmailAddress = "info@blastradius.com",
                    Language = Language.Chinese,
                    Category = action
                },
                new Movie
                {
                    MovieName = "Quiet Reckoning",
                    ReleaseDate = new DateOnly(2023, 11, 2),
                    Director = "Haruto Sato",
                    ContactEmailAddress = "contact@quietreckoning.com",
                    Language = Language.Japanese,
                    Category = drama
                },
                new Movie
                {
                    MovieName = "Whispers in the Dark",
                    ReleaseDate = new DateOnly(2025, 10, 31),
                    Director = "Emily Hart",
                    ContactEmailAddress = "hello@whispersinthedark.com",
                    Language = Language.English,
                    Category = horror
                }
            );

            context.SaveChanges();
        }
    }
}
