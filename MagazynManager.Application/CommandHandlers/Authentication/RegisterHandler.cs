using MagazynManager.Application.Commands.Authentication;
using MagazynManager.Application.DataProviders;
using MagazynManager.Domain.Entities.Uzytkownicy;
using MagazynManager.Infrastructure.InputModel.Authentication;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace MagazynManager.Application.CommandHandlers.Authentication
{
    [CommandHandler]
    public class RegisterHandler : IRequestHandler<RegisterCommand, RegisterResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMediator _mediator;

        public RegisterHandler(IUserRepository userRepository, IMediator mediator)
        {
            _userRepository = userRepository;
            _mediator = mediator;
        }

        public async Task<RegisterResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var user = User.RegisterUser(request.RegisterModel.Email, request.RegisterModel.Age);
            _userRepository.RegisterWithPassword(user, request.RegisterModel.Password);
            var authResult = await _mediator.Send(new LoginCommand(new UserLoginModel
            {
                Email = request.RegisterModel.Email,
                Password = request.RegisterModel.Password
            }));

            if (authResult == null)
            {
                return null;
            }

            return new RegisterResult
            {
                UserId = user.Id,
                AuthResult = authResult
            };
        }
    }
}