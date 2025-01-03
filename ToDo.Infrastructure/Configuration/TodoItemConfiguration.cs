using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDo.Domain.Todo;

namespace ToDo.Infrastructure.Configuration;

internal sealed class TodoItemConfiguration : IEntityTypeConfiguration<TodoItem>
{
    public void Configure(EntityTypeBuilder<TodoItem> builder)
    {
        builder.ToTable("TodoItems");
        builder.HasKey(t => t.Id);

        builder.Property(x => x.Title)
            .HasMaxLength(100)
            .HasConversion(x => x.Value, value => Title.Create(value))
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(500)
            .HasConversion(x => x.Value, value => Description.Create(value))
            .IsRequired(false);

        builder.Property(x => x.CreatedOnUtc)
            .IsRequired();

        builder.Property(x => x.DueDate)
            .IsRequired(false);

        builder.Property(x => x.IsCompleted)
            .IsRequired();

        builder.HasOne<TodoList>()
            .WithMany(x => x.TodoItems)
            .HasForeignKey(x => x.TodoListId);

        builder.Property(x => x.Priority)
            .IsRequired();
    }
}
