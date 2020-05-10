using MagazynManager.Application.Commands.Rezerwacje;
using MagazynManager.Application.QueryHandlers;
using MagazynManager.Domain.Entities.Rezerwacje;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MagazynManager.Application.CommandHandlers.Rezerwacje
{
    [CommandHandler]
    public class RezerwujCommandHandler : IRequestHandler<RezerwujCommand, Guid>
    {
        private readonly IRezerwacjaRepository _rezerwacjaRepository;

        public RezerwujCommandHandler(IRezerwacjaRepository rezerwacjaRepository)
        {
            _rezerwacjaRepository = rezerwacjaRepository;
        }

        public async Task<Guid> Handle(RezerwujCommand request, CancellationToken cancellationToken)
        {
            var rezerwacja = new Rezerwacja()
            {
                PrzedsiebiorstwoId = request.PrzedsiebiorstwoId,
                UzytkownikRezerwujacyId = request.UserId,
                DataWaznosci = request.Model.DataWaznosci,
                DataRezerwacji = request.Model.DataRezerwacji,
                Opis = request.Model.Opis
            };

            foreach (var pozycja in request.Model.Pozycje)
            {
                rezerwacja.DodajPozycjeRezerwacji(new PozycjaRezerwacji(pozycja.ProduktId, pozycja.Ilosc));
            }

            return await _rezerwacjaRepository.Save(rezerwacja);
        }
    }
}