using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDo.Domain.Users;

namespace ToDo.Infrastructure.Configuration;

public sealed class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.ToTable("role_permissions");

        builder.HasKey(x => new { x.RoleId, x.PermissionId });

        builder.HasData(
            new RolePermission(Role.Registered.Id, Permission.UserRead.Id));
    }
}

