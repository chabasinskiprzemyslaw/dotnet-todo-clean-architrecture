using ToDo.Domain.Users;

namespace ToDo.Infrastructure.Authorization;

public sealed class UserRolesResponse
{
    public Guid Id { get; init; }
    public List<Role> Roles { get; init; } = new();

    public UserRolesResponse(Guid id, List<Role> roles)
    {
        Id = id;
        Roles = roles;
    }
}
