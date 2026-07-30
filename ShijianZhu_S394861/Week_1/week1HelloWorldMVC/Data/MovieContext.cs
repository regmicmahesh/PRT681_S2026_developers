using Microsoft.EntityFrameworkCore;
using week1HelloWorldMVC.Models;

namespace week1HelloWorldMVC.Data;

public class MovieContext(DbContextOptions<MovieContext> options) : DbContext(options)
{
    public DbSet<Movie> Movies => Set<Movie>();
}
