namespace ToDo.Domain.Users;

public sealed class Permission
{
    public static Permission UserRead => new(1, "user:read");

    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;

    public Permission(int id, string name)
    {
        Id = id;
        Name = name;
    }
}
