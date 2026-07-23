using Microsoft.EntityFrameworkCore;

namespace DbApp {

    public class ExpenseDbContext : DbContext
    {

        public DbSet<Expense> Expenses => Set<Expense>();
        public DbSet<Category> Categories => Set<Category>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=ExpenseTrackerDb;User Id=sa;Password=Password123!!!;TrustServerCertificate=True;");
        }

    }

}
