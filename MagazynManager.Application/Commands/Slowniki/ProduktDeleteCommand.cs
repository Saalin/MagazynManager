using MediatR;
using System;

namespace MagazynManager.Application.Commands.Slowniki
{
    public class ProduktDeleteCommand : IRequest<Unit>
    {
        public Guid ProduktId { get; set; }
        public Guid PrzedsiebiorstwoId { get; set; }

        public ProduktDeleteCommand(Guid produktId, Guid przedsiebiorstwoId)
        {
            ProduktId = produktId;
            PrzedsiebiorstwoId = przedsiebiorstwoId;
        }
    }
}