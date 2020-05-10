using MediatR;
using System;

namespace MagazynManager.Application.Commands.Slowniki
{
    public class KontrahentDeleteCommand : IRequest<Unit>
    {
        public Guid KontrahentId { get; set; }
        public Guid PrzedsiebiorstwoId { get; set; }

        public KontrahentDeleteCommand(Guid przedsiebiorstwoId, Guid kontrahentId)
        {
            KontrahentId = kontrahentId;
            PrzedsiebiorstwoId = przedsiebiorstwoId;
        }
    }
}