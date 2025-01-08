using Microsoft.AspNetCore.Authorization;

namespace ToDo.Infrastructure.Authorization;

public sealed class HasPermissionAttribute : AuthorizeAttribute
{

    public HasPermissionAttribute(string permission) : base(permission)
    {
    }
}
