using MediatR;
using System;

namespace MagazynManager.Application.Commands.Slowniki
{
    public class JednostkaMiaryDeleteCommand : IRequest<Unit>
    {
        public Guid JednostkaMiaryId { get; }
        public Guid PrzedsiebiorstwoId { get; }

        public JednostkaMiaryDeleteCommand(Guid jednostkaId, Guid przedsiebiorstwoId)
        {
            JednostkaMiaryId = jednostkaId;
            PrzedsiebiorstwoId = przedsiebiorstwoId;
        }
    }
}