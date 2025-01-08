using Microsoft.EntityFrameworkCore;
using ToDo.Domain.Abstractions;
using ToDo.Domain.Users;

namespace ToDo.Infrastructure.Authorization;

internal sealed class AuthorizationService
{
    private readonly ApplicationDbContext _dbContext;

    public AuthorizationService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserRolesResponse> GetRolesForUserAsync(string identityId)
    {
        var roles = await _dbContext.Set<User>()
            .Where(x => x.IdentityId == identityId)
            .Select(x => new UserRolesResponse(x.Id, x.Roles.ToList()))
            .FirstAsync();

        return roles;
    }

    public async Task<HashSet<string>> GetPermissionsForUserAsync(string identityId)
    {
        var user = await _dbContext.Set<User>()
            .Include(u => u.Roles)
            .ThenInclude(r => r.Permissions)
            .FirstOrDefaultAsync(u => u.IdentityId == identityId);

        if (user == null)
        {
            // Handle user not found
            return new HashSet<string>();
        }

        var permissions = user.Roles
            .SelectMany(r => r.Permissions)
            .Select(p => p.Name)
            .ToHashSet();

        return permissions;
    }
}
