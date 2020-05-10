using MagazynManager.Application.DataProviders;
using MediatR;

namespace MagazynManager.Application.Commands.Authentication
{
    public class RefreshTokenCommand : IRequest<AuthResult>
    {
        public AuthResult AuthResult { get; }

        public RefreshTokenCommand(AuthResult authResult)
        {
            AuthResult = authResult;
        }
    }
}