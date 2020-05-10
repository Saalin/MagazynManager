using System;

namespace MagazynManager.Application.DataProviders
{
    public interface IRefreshTokenStore
    {
        bool ValidateToken(Guid userId, string token);

        RefreshToken GetRefreshToken(Guid userId);
    }
}