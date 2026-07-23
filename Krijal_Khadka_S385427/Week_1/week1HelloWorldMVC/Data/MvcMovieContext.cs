using Microsoft.EntityFrameworkCore;
using week1HelloWorldMVC.Models;

namespace week1HelloWorldMVC.Data;

public class MvcMovieContext : DbContext
{
    public MvcMovieContext(DbContextOptions<MvcMovieContext> options)
        : base(options)
    {
    }

    public DbSet<Movie> Movie { get; set; } = default!;
}