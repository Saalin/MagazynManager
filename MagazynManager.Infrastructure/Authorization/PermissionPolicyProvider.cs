using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace MagazynManager.Infrastructure.Authorization
{
    public class PermissionPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        public PermissionPolicyProvider(IOptions<AuthorizationOptions> options) : base(options)
        {
        }

        public override Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith(AuthConst.PolicyPrefix))
            {
                (AppArea area, Access access) = AuthHelper.PolicyToPermission(policyName);

                var policy = new AuthorizationPolicyBuilder()
                    .AddRequirements(new PermissionRequirement(area, access))
                    .Build();

                return Task.FromResult(policy);
            }
            else
            {
                return base.GetPolicyAsync(policyName);
            }
        }
    }
}