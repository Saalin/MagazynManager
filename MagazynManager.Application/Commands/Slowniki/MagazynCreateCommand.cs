using MediatR;
using System;

namespace MagazynManager.Application.Commands.Slowniki
{
    public class MagazynCreateCommand : IRequest<Guid>
    {
        public string Skrot { get; }
        public string Nazwa { get; }
        public Guid PrzedsiebiorstwoId { get; }

        public MagazynCreateCommand(string skrot, string nazwa, Guid przedsiebiorstwoId)
        {
            Skrot = skrot;
            Nazwa = nazwa;
            PrzedsiebiorstwoId = przedsiebiorstwoId;
        }
    }
}