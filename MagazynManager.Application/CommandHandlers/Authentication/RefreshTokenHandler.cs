using MagazynManager.Application.Commands.Authentication;
using MagazynManager.Application.DataProviders;
using MagazynManager.Application.QueryHandlers;
using MagazynManager.Domain.Entities.Uzytkownicy;
using MagazynManager.Infrastructure.Specifications;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading;
using System.Threading.Tasks;

namespace MagazynManager.Application.CommandHandlers.Authentication
{
    [CommandHandler]
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, AuthResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly TokenManager _tokenManager;
        private readonly IRefreshTokenStore _tokenStore;

        public RefreshTokenHandler(IUserRepository userRepository, IConfiguration configuration, IRefreshTokenStore tokenStore)
        {
            _userRepository = userRepository;
            _tokenManager = new TokenManager(configuration["Token:Key"]);
            _tokenStore = tokenStore;
        }

        public Task<AuthResult> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            if (!_tokenManager.ValidateToken(request.AuthResult.Token, out var userId_przedsiebiorstwoId))
            {
                return Task.FromResult((AuthResult)null);
            }
            if (_tokenStore.ValidateToken(userId_przedsiebiorstwoId.Item1.Value, request.AuthResult.RefreshToken))
            {
                var refreshToken = _tokenStore.GetRefreshToken(userId_przedsiebiorstwoId.Item1.Value);
                var user = _userRepository.GetUser(new IdSpecification<User, Guid>(userId_przedsiebiorstwoId.Item1.Value), userId_przedsiebiorstwoId.Item2.Value);
                var authResult = new AuthResult
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(_tokenManager.CreateJWTToken(user.Claims)),
                    RefreshToken = refreshToken.Token,
                    ExpireAt = refreshToken.ExpireTimestamp
                };
                return Task.FromResult(authResult);
            }
            else
            {
                return Task.FromResult((AuthResult)null);
            }
        }
    }
}