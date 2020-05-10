using MediatR;
using System;

namespace MagazynManager.Application.Commands.Slowniki
{
    public class MagazynDeleteCommand : IRequest<Unit>
    {
        public Guid MagazynId { get; }
        public Guid PrzedsiebiorstwoId { get; }

        public MagazynDeleteCommand(Guid magazynId, Guid przedsiebiorstwoId)
        {
            MagazynId = magazynId;
            PrzedsiebiorstwoId = przedsiebiorstwoId;
        }
    }
}