using MagazynManager.Infrastructure.InputModel.Authentication;
using MediatR;
using System;

namespace MagazynManager.Application.Commands.Authentication
{
    public class SetPermissionsCommand : IRequest<Unit>
    {
        public Guid PrzedsiebiorstwoId { get; }
        public SetPermissionsModel Model { get; }

        public SetPermissionsCommand(Guid przedsiebiorstwoId, SetPermissionsModel inputModel)
        {
            Model = inputModel;
            PrzedsiebiorstwoId = przedsiebiorstwoId;
        }
    }
}