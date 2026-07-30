using Microsoft.EntityFrameworkCore;

public class week1HelloWorldMVCContext(DbContextOptions<week1HelloWorldMVCContext> options) : DbContext(options)
{
    public DbSet<week1HelloWorldMVC.Models.Movie> Movie { get; set; } = default!;
}
