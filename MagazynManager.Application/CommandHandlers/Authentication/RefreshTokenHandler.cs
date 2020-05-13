using MagazynManager.Application.Commands.Authentication;
using MagazynManager.Application.DataProviders;
using MagazynManager.Domain.Entities.Uzytkownicy;
using MagazynManager.Domain.Specification.Specifications;
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
            var validationResult = _tokenManager.ValidateToken(request.AuthResult.Token);
            if (!validationResult.IsValid)
            {
                return Task.FromResult((AuthResult)null);
            }
            if (_tokenStore.ValidateToken(validationResult.Identity.UserId, request.AuthResult.RefreshToken))
            {
                var refreshToken = _tokenStore.GetRefreshToken(validationResult.Identity.UserId);
                var user = _userRepository.GetUser(new IdSpecification<User, Guid>(validationResult.Identity.UserId), validationResult.Identity.PrzedsiebiorstwoId);
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