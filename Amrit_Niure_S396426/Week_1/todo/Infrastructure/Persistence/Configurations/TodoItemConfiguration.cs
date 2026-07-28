using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public sealed class TodoItemConfiguration : IEntityTypeConfiguration<TodoItem>
{
    public void Configure(EntityTypeBuilder<TodoItem> builder)
    {
        builder.ToTable("TodoItems");

        builder.HasKey(t => t.Id);

        // TodoTitle is a value object in the domain model; EF Core only understands
        // primitive columns, so it's converted to/from a plain string at the mapping boundary.
        builder.Property(t => t.Title)
            .HasConversion(title => title.Value, value => new TodoTitle(value))
            .HasMaxLength(TodoTitle.MaxLength)
            .IsRequired();

        builder.Property(t => t.Description)
            .HasMaxLength(2000);

        builder.Property(t => t.Priority)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Ignore(t => t.DomainEvents);
    }
}
