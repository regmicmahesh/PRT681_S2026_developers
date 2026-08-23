using Microsoft.EntityFrameworkCore;
using WetSeasonBackend.Api.Models;

namespace WetSeasonBackend.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<Incident>  Incidents => Set<Incident>();
    public DbSet<Community> Communities => Set<Community>();

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

        var community = modelBuilder.Entity<Community>();
        community.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(120);
        community.Property(c => c.Region)
            .IsRequired()
            .HasMaxLength(60);
        community.Property(c => c.ContactEmail)
            .IsRequired()
            .HasMaxLength(100);
        
        community.HasIndex(c => c.Name)
            .IsUnique();

        incident.HasOne(i => i.Community)
            .WithMany(i => i.Incidents)
            .HasForeignKey(i => i.CommunityId)
            .OnDelete(DeleteBehavior.Restrict);
        

    }
    
    
}