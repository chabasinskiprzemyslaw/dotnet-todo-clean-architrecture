using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace ToDo.Infrastructure.Authorization;

internal sealed class PermissionAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
{
    private readonly AuthorizationOptions _options;

    public PermissionAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) : base(options)
    {
        _options = options.Value;
    }

    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        var policy = await base.GetPolicyAsync(policyName);

        if (policy is not null)
        {
            return policy;
        }

        var permission = policyName;
        policy = new AuthorizationPolicyBuilder()
            .AddRequirements(new PermissionRequirements(permission))
            .Build();

        _options.AddPolicy(policyName, policy);

        return policy;
    }
}
