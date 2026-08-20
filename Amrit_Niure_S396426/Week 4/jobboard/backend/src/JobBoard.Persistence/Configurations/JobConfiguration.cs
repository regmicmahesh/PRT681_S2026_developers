using JobBoard.Domain.Entities;
using JobBoard.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobBoard.Persistence.Configurations;

internal sealed class JobConfiguration : IEntityTypeConfiguration<Job>
{
    public void Configure(EntityTypeBuilder<Job> builder)
    {
        builder.ToTable("Jobs");

        builder.HasKey(j => j.Id);

        builder.Property(j => j.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(j => j.Description)
            .HasMaxLength(4000)
            .IsRequired();

        builder.Property(j => j.EmploymentType)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(j => j.Status)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(j => j.CompanyId)
            .IsRequired();

        builder.OwnsOne(j => j.Salary, salary =>
        {
            salary.Property(s => s.Min).HasColumnName("SalaryMin");
            salary.Property(s => s.Max).HasColumnName("SalaryMax");
            salary.Property(s => s.Currency)
                .HasColumnName("SalaryCurrency")
                .HasConversion(currency => currency.Code, code => new Currency(code))
                .HasMaxLength(3);
            salary.Property(s => s.PayPeriod)
                .HasColumnName("SalaryPayPeriod")
                .HasConversion<string>()
                .HasMaxLength(20);
        });

        builder.HasMany(j => j.Applications)
            .WithOne()
            .HasForeignKey(a => a.JobId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Metadata
            .FindNavigation(nameof(Job.Applications))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.Ignore(j => j.DomainEvents);
    }
}
