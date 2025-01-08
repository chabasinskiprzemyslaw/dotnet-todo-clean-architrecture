using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDo.Domain.Users;

namespace ToDo.Infrastructure.Configuration;

internal sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("roles");
        builder.HasKey(x => x.Id);

        builder.HasMany(x => x.Permissions)
            .WithMany()
            .UsingEntity<RolePermission>();

        builder.HasData(Role.Registered);
    }
}
