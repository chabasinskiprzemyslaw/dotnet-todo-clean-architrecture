using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using ToDo.Application.Abstractions.Caching;
using ToDo.Domain.Abstractions;
using ToDo.Domain.Users;

namespace ToDo.Infrastructure.Authorization;

internal sealed class AuthorizationService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ICacheService _cacheService;

    public AuthorizationService(
        ApplicationDbContext dbContext, 
        ICacheService cacheService)
    {
        _dbContext = dbContext;
        _cacheService = cacheService;
    }

    public async Task<UserRolesResponse> GetRolesForUserAsync(string identityId)
    {
        var cacheKey = $"auth:roles-{identityId}";
        var cachedRoles = await _cacheService.GetAsync<UserRolesResponse>(cacheKey);

        if (cachedRoles is not null)
        {
            return cachedRoles;
        }

        var roles = await _dbContext.Set<User>()
            .Where(x => x.IdentityId == identityId)
            .Select(x => new UserRolesResponse(x.Id, x.Roles.ToList()))
            .FirstAsync();

        await _cacheService.SetAsync(cacheKey, roles);

        return roles;
    }

    public async Task<HashSet<string>> GetPermissionsForUserAsync(string identityId)
    {
        var cacheKey = $"auth:permissions-{identityId}";
        var cachedPermissions = await _cacheService.GetAsync<HashSet<string>>(cacheKey);

        if (cachedPermissions is not null)
        {
            return cachedPermissions;
        }

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

        await _cacheService.SetAsync(cacheKey, permissions);

        return permissions;
    }
}
