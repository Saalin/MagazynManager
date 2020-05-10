using MagazynManager.Infrastructure.InputModel.Ewidencja;
using MediatR;
using System;

namespace MagazynManager.Application.Commands.Ewidencja
{
    public class PrzesunCommand : IRequest<Guid>
    {
        public Guid PrzedsiebiorstwoId { get; }
        public PrzesuniecieCreateModel Model { get; }

        public PrzesunCommand(Guid przedsiebiorstwoId, PrzesuniecieCreateModel model)
        {
            Model = model;
            PrzedsiebiorstwoId = przedsiebiorstwoId;
        }
    }
}