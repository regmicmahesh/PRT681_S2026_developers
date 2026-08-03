using Microsoft.EntityFrameworkCore;

public class week2TheaterAdminContext(DbContextOptions<week2TheaterAdminContext> options) : DbContext(options)
{
    public DbSet<week2TheaterAdmin.Models.Category> Category { get; set; } = default!;
    public DbSet<week2TheaterAdmin.Models.Movie> Movie { get; set; } = default!;
}
