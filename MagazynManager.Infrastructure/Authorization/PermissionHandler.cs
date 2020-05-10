using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace MagazynManager.Infrastructure.Authorization
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (context.User.CheckPermission(requirement.Area, requirement.Access))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}