using MediatR;
using System;

namespace MagazynManager.Application.Commands.Rezerwacje
{
    public class GenerujDokumentWydaniaCommand : IRequest<Guid>
    {
        public Guid PrzedsiebiorstwoId { get; }
        public Guid RezerwacjaId { get; }

        public GenerujDokumentWydaniaCommand(Guid przedsiebiorstwo, Guid rezerwacjaId)
        {
            PrzedsiebiorstwoId = przedsiebiorstwo;
            RezerwacjaId = rezerwacjaId;
        }
    }
}