using MagazynManager.Technical.Helpers;
using System.Linq;
using System.Security.Claims;

namespace MagazynManager.Infrastructure.Authorization
{
    public static class PermissionHelper
    {
        public static bool CheckPermission(this ClaimsPrincipal user, AppArea area, Access access)
        {
            Claim adminClaim = user.Claims.FirstOrDefault(c => c.Type == AuthConst.AdminClaim);

            if (adminClaim?.Value.Equals(true.ToString()) == true)
            {
                return true;
            }

            Claim permissionClaim = user.Claims
                .FirstOrDefault(c => c.Type == AuthHelper.PermissionToClaim(area.GetAttributeOfType<PermissionNameAttribute>().Name));

            if (permissionClaim == null)
            {
                return false;
            }

            string accessString = permissionClaim.Value;
            Access userAccess = AuthHelper.StringToAccess(accessString);

            return (userAccess & access) == access;
        }
    }
}