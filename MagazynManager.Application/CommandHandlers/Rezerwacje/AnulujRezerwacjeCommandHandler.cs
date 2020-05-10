using MagazynManager.Application.Commands.Rezerwacje;
using MagazynManager.Domain.Entities.Rezerwacje;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MagazynManager.Application.CommandHandlers.Rezerwacje
{
    [CommandHandler]
    public class AnulujRezerwacjeCommandHandler : IRequestHandler<AnulujRezerwacjeCommand, Unit>
    {
        private readonly IRezerwacjaRepository _rezerwacjaRepository;

        public AnulujRezerwacjeCommandHandler(IRezerwacjaRepository rezerwacjaRepository)
        {
            _rezerwacjaRepository = rezerwacjaRepository;
        }

        public async Task<Unit> Handle(AnulujRezerwacjeCommand request, CancellationToken cancellationToken)
        {
            var list = await _rezerwacjaRepository.GetList(request.PrzedsiebiorstwoId);
            var rezerwacjaDoUsuniecia = list.SingleOrDefault(x => x.Id == request.RezerwacjaId);

            if (rezerwacjaDoUsuniecia == null)
            {
                throw new BussinessException("Brak rezerwacji o takim id");
            }

            if (rezerwacjaDoUsuniecia.Zreazlizowana)
            {
                throw new BussinessException("Rezerwacja została zrealizowana");
            }

            if (rezerwacjaDoUsuniecia.UzytkownikRezerwujacyId != request.UzytkownikAnulujacyId)
            {
                throw new BussinessException("Rezerwacja może zostać anulowana tylko przez twórcę");
            }

            await _rezerwacjaRepository.Delete(rezerwacjaDoUsuniecia);

            return Unit.Value;
        }
    }
}