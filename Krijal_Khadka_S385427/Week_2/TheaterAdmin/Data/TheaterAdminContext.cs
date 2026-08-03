using Microsoft.EntityFrameworkCore;
using TheaterAdmin.Models;

namespace TheaterAdmin.Data
{
    public class TheaterAdminContext : DbContext
    {
        public TheaterAdminContext(DbContextOptions<TheaterAdminContext> options)
            : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; } = default!;
        public DbSet<Category> Categories { get; set; } = default!;
    }
}