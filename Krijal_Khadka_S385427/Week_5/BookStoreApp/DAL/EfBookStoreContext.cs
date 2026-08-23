using Microsoft.EntityFrameworkCore;

namespace BookStoreApp.DAL;

public class EfBookStoreContext : DbContext
{
    public EfBookStoreContext(
        DbContextOptions<EfBookStoreContext> options)
        : base(options)
    {
    }

    public DbSet<Book> Books { get; set; }

    public DbSet<Printer> Printers { get; set; }

    public DbSet<Scanner> Scanners { get; set; }
}