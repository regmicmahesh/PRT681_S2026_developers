using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApp.DAL;

public class EfBookStoreContext : IdentityDbContext
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