using MagazynManager.Application.Commands.Authentication;
using MagazynManager.Application.DataProviders;
using MagazynManager.Domain.Entities.Uzytkownicy;
using MagazynManager.Domain.Specification.Specifications;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Threading;
using System.Threading.Tasks;

namespace MagazynManager.Application.CommandHandlers.Authentication
{
    [CommandHandler]
    public class LoginHandler : IRequestHandler<LoginCommand, AuthResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly TokenManager _tokenManager;
        private readonly IRefreshTokenStore _tokenStore;

        public LoginHandler(IUserRepository userRepository, IConfiguration configuration, IRefreshTokenStore tokenStore)
        {
            _userRepository = userRepository;
            _tokenManager = new TokenManager(configuration["Token:Key"]);
            _tokenStore = tokenStore;
        }

        public Task<AuthResult> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = _userRepository.GetUser(new EmailSpecification(request.Email), request.PrzedsiebiorstwoId);
            if (user != null && user.ValidatePassword(request.Password))
            {
                var refreshToken = _tokenStore.GetRefreshToken(user.Id);
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