using MagazynManager.Application.Commands.Authentication;
using MagazynManager.Domain.Entities.Uzytkownicy;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MagazynManager.Application.CommandHandlers.Authentication
{
    [CommandHandler]
    public class SetPermissionsCommandHandler : IRequestHandler<SetPermissionsCommand, Unit>
    {
        private readonly IUserRepository _userRepository;

        public SetPermissionsCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Unit> Handle(SetPermissionsCommand request, CancellationToken cancellationToken)
        {
            await _userRepository.SetPermissions(request.PrzedsiebiorstwoId, request.Model.UserId,
                request.Model.Claims.Select(x => KeyValuePair.Create(x.Name, x.Value)));
            return Unit.Value;
        }
    }
}