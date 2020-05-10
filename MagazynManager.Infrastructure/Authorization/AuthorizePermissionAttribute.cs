using Microsoft.AspNetCore.Authorization;

namespace MagazynManager.Infrastructure.Authorization
{
    public class AuthorizePermissionAttribute : AuthorizeAttribute
    {
        public AuthorizePermissionAttribute(AppArea permission, Access access)
        {
            Policy = AuthHelper.PermissionToPolicy(permission, access);
        }
    }
}