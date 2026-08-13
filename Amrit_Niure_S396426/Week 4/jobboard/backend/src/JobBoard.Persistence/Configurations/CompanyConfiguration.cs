using JobBoard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobBoard.Persistence.Configurations;

internal sealed class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("Companies");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.OwnsOne(c => c.ContactEmail, email =>
        {
            email.Property(e => e.Value)
                .HasColumnName("ContactEmail")
                .HasMaxLength(320)
                .IsRequired();
        });

        builder.Ignore(c => c.DomainEvents);
    }
}
