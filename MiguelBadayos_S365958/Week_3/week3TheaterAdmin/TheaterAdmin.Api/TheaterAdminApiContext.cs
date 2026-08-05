using Microsoft.EntityFrameworkCore;

public class TheaterAdminApiContext(DbContextOptions<TheaterAdminApiContext> options) : DbContext(options)
{
    public DbSet<TheaterAdmin.Api.Models.Category> Category { get; set; } = default!;
    public DbSet<TheaterAdmin.Api.Models.Movie> Movie { get; set; } = default!;
}
