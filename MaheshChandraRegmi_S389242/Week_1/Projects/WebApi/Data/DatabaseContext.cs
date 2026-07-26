using Microsoft.EntityFrameworkCore;
using WebApi.Entities;

namespace WebApi.Data {

    public class WebApiDbContext: DbContext {

        public DbSet<Todo> Todos => Set<Todo>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){
            optionsBuilder.UseSqlite("Data Source=app.db");
        }

    }
}
