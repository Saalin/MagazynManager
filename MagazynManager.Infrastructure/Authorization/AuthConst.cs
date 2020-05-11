namespace MagazynManager.Infrastructure.Authorization
{
    public static class AuthConst
    {
        public static readonly string PolicyPrefix = "Permissions";
        public static readonly string ClaimPrefix = "Permission.";
        public static readonly string AdminClaim = "User.IsAdmin";
    }
}