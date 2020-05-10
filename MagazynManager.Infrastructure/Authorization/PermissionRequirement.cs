using Microsoft.AspNetCore.Authorization;

namespace MagazynManager.Infrastructure.Authorization
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public AppArea Area { get; }
        public Access Access { get; }

        public PermissionRequirement(AppArea area, Access access)
        {
            Area = area;
            Access = access;
        }
    }
}