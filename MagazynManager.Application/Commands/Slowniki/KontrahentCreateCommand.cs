using MagazynManager.Infrastructure.InputModel.Slowniki;
using MediatR;
using System;

namespace MagazynManager.Application.Commands.Slowniki
{
    public class KontrahentCreateCommand : IRequest<Guid>
    {
        public KontrahentCreateModel Model { get; set; }
        public Guid PrzedsiebiorstwoId { get; set; }

        public KontrahentCreateCommand(Guid przedsiebiorstwoId, KontrahentCreateModel model)
        {
            PrzedsiebiorstwoId = przedsiebiorstwoId;
            Model = model;
        }
    }
}