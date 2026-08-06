using CleanApp.Domain.Common;
using CleanApp.Domain.TodoLists;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanApp.Persistence.Configurations;

public sealed class TodoListConfiguration : IEntityTypeConfiguration<TodoList>
{
    public void Configure(EntityTypeBuilder<TodoList> builder)
    {
        builder.ToTable("TodoLists");

        builder.HasKey(l => l.Id);
        builder.Property(l => l.Id)
            .HasConversion(id => id.Value, value => new TodoListId(value))
            .ValueGeneratedNever();

        builder.Property(l => l.OwnerId)
            .HasConversion(id => id.Value, value => new UserId(value))
            .IsRequired();
        builder.HasIndex(l => l.OwnerId);

        builder.OwnsOne(l => l.Title, title =>
        {
            title.Property(t => t.Value)
                .HasColumnName("Title")
                .HasMaxLength(TodoListTitle.MaxLength)
                .IsRequired();
        });
        builder.Navigation(l => l.Title).IsRequired();

        // A second owned type on the same entity as Title triggers a reproducible EF Core 10
        // bug where saving 2+ new TodoLists in one SaveChanges leaves Colour NULL on some rows.
        // Colour maps to a single scalar just like Priority does on TodoItem, sidestepping it.
        builder.Property(l => l.Colour)
            .HasConversion(c => c.Code, v => Colour.FromCodeUnsafe(v))
            .HasColumnName("Colour")
            .HasMaxLength(9)
            .IsRequired();

        builder.Property(l => l.CreatedOnUtc).IsRequired();
    }
}
