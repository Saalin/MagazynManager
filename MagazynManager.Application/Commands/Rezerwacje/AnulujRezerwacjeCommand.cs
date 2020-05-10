using MediatR;
using System;

namespace MagazynManager.Application.Commands.Rezerwacje
{
    public class AnulujRezerwacjeCommand : IRequest<Unit>
    {
        public Guid PrzedsiebiorstwoId { get; }
        public Guid RezerwacjaId { get; }
        public Guid UzytkownikAnulujacyId { get; set; }

        public AnulujRezerwacjeCommand(Guid przedsiebiorstwoId, Guid rezerwacjaId, Guid uzytkownikAnulujacyId)
        {
            PrzedsiebiorstwoId = przedsiebiorstwoId;
            RezerwacjaId = rezerwacjaId;
            UzytkownikAnulujacyId = uzytkownikAnulujacyId;
        }
    }
}