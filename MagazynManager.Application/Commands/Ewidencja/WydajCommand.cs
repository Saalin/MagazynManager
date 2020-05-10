using MagazynManager.Infrastructure.InputModel.Ewidencja;
using MediatR;
using System;

namespace MagazynManager.Application.Commands.Ewidencja
{
    public class WydajCommand : IRequest<Guid>
    {
        public WydanieCreateModel Model { get; }
        public Guid PrzedsiebiorstwoId { get; }

        public WydajCommand(Guid przedsiebiorstwoId, WydanieCreateModel model)
        {
            Model = model;
            PrzedsiebiorstwoId = przedsiebiorstwoId;
        }
    }
}