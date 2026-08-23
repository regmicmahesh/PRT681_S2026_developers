using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DreamSales.Models;

namespace DreamSales.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets for your entities
        public DbSet<Item> Items { get; set; } = default!;
        public DbSet<Cart> Carts { get; set; } = default!;
        public DbSet<CartItem> CartItems { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Ensure Identity models are configured
        }
    }
}
