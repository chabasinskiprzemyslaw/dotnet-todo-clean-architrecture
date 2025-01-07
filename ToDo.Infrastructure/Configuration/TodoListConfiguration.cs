using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDo.Domain.Todo;
using ToDo.Domain.Users;

namespace ToDo.Infrastructure.Configuration;

internal sealed class TodoListConfiguration : IEntityTypeConfiguration<TodoList>
{
    public void Configure(EntityTypeBuilder<TodoList> builder)
    {
        builder.ToTable("TodoLists");

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

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.OwnerId);

        builder.Property(x => x.RowVersion)
            .IsRowVersion();
    }
}
