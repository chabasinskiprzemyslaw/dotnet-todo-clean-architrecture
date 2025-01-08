namespace ToDo.Domain.Users;

public class RolePermission
{
    public int RoleId { get; set; }
    public int PermissionId { get; set; }

    public RolePermission(int roleId, int permissionId)
    {
        RoleId = roleId;
        PermissionId = permissionId;
    }
}
