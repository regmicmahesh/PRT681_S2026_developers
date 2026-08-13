using JobBoard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobBoard.Persistence.Configurations;

internal sealed class JobApplicationConfiguration : IEntityTypeConfiguration<JobApplication>
{
    public void Configure(EntityTypeBuilder<JobApplication> builder)
    {
        builder.ToTable("JobApplications");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.CandidateName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(a => a.ResumeUrl)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(a => a.Status)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.OwnsOne(a => a.CandidateEmail, email =>
        {
            email.Property(e => e.Value)
                .HasColumnName("CandidateEmail")
                .HasMaxLength(320)
                .IsRequired();
        });
    }
}
