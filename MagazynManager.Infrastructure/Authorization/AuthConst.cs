namespace MagazynManager.Infrastructure.Authorization
{
    public static class AuthConst
    {
        public const string PolicyPrefix = "Permissions";
        public const string ClaimPrefix = "Permission.";
        public const string AdminClaim = "User.IsAdmin";
    }
}