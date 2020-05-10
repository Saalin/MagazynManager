using NodaTime;

namespace MagazynManager.Application.DataProviders
{
    public class RefreshToken
    {
        public string Token { get; set; }
        public Instant ExpireTimestamp { get; set; }
        public bool Revoked { get; set; }
    }
}