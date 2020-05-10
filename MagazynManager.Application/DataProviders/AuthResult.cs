using NodaTime;

namespace MagazynManager.Application.DataProviders
{
    public class AuthResult
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public Instant ExpireAt { get; set; }
    }
}