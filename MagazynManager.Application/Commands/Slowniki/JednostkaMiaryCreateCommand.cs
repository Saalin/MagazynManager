using MediatR;
using System;

namespace MagazynManager.Application.Commands.Slowniki
{
    public class JednostkaMiaryCreateCommand : IRequest<Guid>
    {
        public string Name { get; set; }
        public Guid PrzedsiebiorstwoId { get; set; }

        public JednostkaMiaryCreateCommand(Guid przedsiebiorstwoId, string nazwa)
        {
            PrzedsiebiorstwoId = przedsiebiorstwoId;
            Name = nazwa;
        }
    }
}