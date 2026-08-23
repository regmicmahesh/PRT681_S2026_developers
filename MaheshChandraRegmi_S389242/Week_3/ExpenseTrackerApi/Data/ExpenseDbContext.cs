using ExpenseTrackerApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerApi.Data
{
    public class ExpenseDbContext : DbContext
    {
        public ExpenseDbContext()
        {
        }

        public ExpenseDbContext(DbContextOptions<ExpenseDbContext> options) : base(options)
        {
        }

        public DbSet<Expense> Expenses => Set<Expense>();
        public DbSet<Category> Categories => Set<Category>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // same idea as Week 1 WebApi — file sqlite unless tests pass options in
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=expenses.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Food" },
                new Category { Id = 2, Name = "Transport" },
                new Category { Id = 3, Name = "Rent" },
                new Category { Id = 4, Name = "Other" }
            );
        }
    }
}
