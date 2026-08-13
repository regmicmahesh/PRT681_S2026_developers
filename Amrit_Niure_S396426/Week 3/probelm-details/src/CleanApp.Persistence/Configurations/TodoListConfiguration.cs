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

        builder.OwnsOne(l => l.Title, title =>
        {
            title.Property(t => t.Value)
                .HasColumnName("Title")
                .HasMaxLength(TodoListTitle.MaxLength)
                .IsRequired();
        });
        builder.Navigation(l => l.Title).IsRequired();

        builder.OwnsOne(l => l.Colour, colour =>
        {
            colour.Property(c => c.Code)
                .HasColumnName("Colour")
                .HasMaxLength(9)
                .IsRequired();
        });
        builder.Navigation(l => l.Colour).IsRequired();

        builder.Property(l => l.CreatedOnUtc).IsRequired();
    }
}
