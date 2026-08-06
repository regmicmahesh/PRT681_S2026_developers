using CleanApp.Domain.Common;
using CleanApp.Domain.TodoItems;
using CleanApp.Domain.TodoLists;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanApp.Persistence.Configurations;

public sealed class TodoItemConfiguration : IEntityTypeConfiguration<TodoItem>
{
    public void Configure(EntityTypeBuilder<TodoItem> builder)
    {
        builder.ToTable("TodoItems");

        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id)
            .HasConversion(id => id.Value, value => new TodoItemId(value))
            .ValueGeneratedNever();

        builder.Property(i => i.OwnerId)
            .HasConversion(id => id.Value, value => new UserId(value))
            .IsRequired();
        builder.HasIndex(i => i.OwnerId);

        builder.Property(i => i.TodoListId)
            .HasConversion(id => id.Value, value => new TodoListId(value))
            .IsRequired();
        builder.HasIndex(i => i.TodoListId);

        builder.OwnsOne(i => i.Title, title =>
        {
            title.Property(t => t.Value)
                .HasColumnName("Title")
                .HasMaxLength(TodoItemTitle.MaxLength)
                .IsRequired();
        });
        builder.Navigation(i => i.Title).IsRequired();

        // A smart-enum value object with a derived Name isn't a good fit for owned-type
        // constructor binding, so it's stored as a plain int column instead.
        builder.Property(i => i.Priority)
            .HasConversion(p => p.Value, v => PriorityLevel.FromValueUnsafe(v))
            .HasColumnName("Priority")
            .IsRequired();

        builder.Property(i => i.Note).HasMaxLength(2000);
        builder.Property(i => i.ReminderUtc);
        builder.Property(i => i.IsDone).IsRequired();
        builder.Property(i => i.CreatedOnUtc).IsRequired();
        builder.Property(i => i.CompletedOnUtc);
    }
}
