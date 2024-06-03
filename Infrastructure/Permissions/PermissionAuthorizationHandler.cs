using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
namespace Infrastructure.Permissions;

public class PermissionAuthorizationHandler(DataContext context):AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        var user = context.User;
        var userId = context.User.Claims.FirstOrDefault(x=>x.Type=="sid")?.Value;
        if (userId== null)
            return Task.CompletedTask;


        foreach (var claim in context.User.Claims)
        {
            if (claim.Type != "Permissions" || claim.Value != requirement.Permission)
                continue;

            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        return Task.CompletedTask;
    }
}
