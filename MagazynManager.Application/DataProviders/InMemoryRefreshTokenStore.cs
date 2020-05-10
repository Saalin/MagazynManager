using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MagazynManager.Application.DataProviders
{
    public class InMemoryRefreshTokenStore : IRefreshTokenStore
    {
        private readonly Dictionary<Guid, List<RefreshToken>> _concurrentDictionary;
        private readonly IClock _clock;

        private static readonly object obj = new object();

        public InMemoryRefreshTokenStore(IClock clock)
        {
            _concurrentDictionary = new Dictionary<Guid, List<RefreshToken>>();
            _clock = clock;
        }

        public bool ValidateToken(Guid userId, string token)
        {
            var result = _concurrentDictionary.TryGetValue(userId, out var tokenList);
            var found = tokenList.SingleOrDefault(x => x.Token == token);
            if (result && found != null)
            {
                var expireTimestamp = found.ExpireTimestamp;
                if (expireTimestamp > _clock.GetCurrentInstant())
                {
                    return true;
                }
            }
            return false;
        }

        public RefreshToken GetRefreshToken(Guid userId)
        {
            var token = CreateToken();
            lock (obj)
            {
                if (_concurrentDictionary.ContainsKey(userId))
                {
                    var list = _concurrentDictionary[userId];
                    list.Add(token);
                }
                else
                {
                    _concurrentDictionary.Add(userId, new List<RefreshToken> { token });
                }
            }
            return token;
        }

        private RefreshToken CreateToken()
        {
            return new RefreshToken
            {
                Token = Guid.NewGuid().ToString(),
                ExpireTimestamp = Instant.Add(_clock.GetCurrentInstant(), Duration.FromDays(30))
            };
        }
    }
}