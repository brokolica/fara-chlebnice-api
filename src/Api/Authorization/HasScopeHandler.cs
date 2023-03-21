using Microsoft.AspNetCore.Authorization;

namespace Api.Authorization;

public class HasScopeHandler : AuthorizationHandler<HasScopeRequirement>
{
    private const string PermissionClaimType = "permissions";
    
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        HasScopeRequirement requirement)
    {
        if (!context.User.HasClaim(c => c.Type == PermissionClaimType && c.Issuer == requirement.Issuer))
        {
            return Task.CompletedTask;
        }

        var permissions = context.User.Claims.Where(c => c.Type == PermissionClaimType &&
                                                         c.Issuer ==  requirement.Issuer);

        if (permissions.Any(perm => perm.Value == requirement.Scope))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}