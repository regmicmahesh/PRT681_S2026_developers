using Microsoft.EntityFrameworkCore;
using WetSeasonBackend.Api.Models;

namespace WetSeasonBackend.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<Incident>  Incidents => Set<Incident>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var incident = modelBuilder.Entity<Incident>();
        
        incident.Property(i => i.Status)
            .HasConversion<String>()
            .HasMaxLength(50)
            .IsRequired();
        incident.Property(i => i.Type)
            .HasConversion<String>()
            .HasMaxLength(50);
        incident.Property(i => i.Description)
            .HasConversion<String>()
            .HasMaxLength(500)
            .IsRequired();
        incident.Property(i => i.ReportedBy)
            .IsRequired()
            .HasMaxLength(100);

    }
    
}