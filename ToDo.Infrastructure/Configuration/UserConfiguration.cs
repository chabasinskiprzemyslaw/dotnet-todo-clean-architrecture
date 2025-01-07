using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDo.Domain.Users;

namespace ToDo.Infrastructure.Configuration;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(t => t.Id);

        builder.Property(x => x.FirstName)
            .HasMaxLength(200)
            .HasConversion(x => x.Value, value => new FirstName(value));

        builder.Property(x => x.LastName)
            .HasMaxLength(200)
            .HasConversion(x => x.Value, value => new LastName(value));

        builder.Property(x => x.Email)
            .HasMaxLength(400)
            .HasConversion(x => x.Value, value => new Domain.Users.Email(value));

        builder.HasIndex(x => x.Email).IsUnique();

        builder.HasIndex(user => user.IdentityId).IsUnique();
    }
}
