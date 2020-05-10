using MagazynManager.Application.Queries.Rezerwacje;
using MagazynManager.Domain.Entities.Rezerwacje;
using MagazynManager.Infrastructure.Dto.Rezerwacje;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MagazynManager.Application.QueryHandlers.Rezerwacje
{
    [QueryHandler]
    public class RezerwacjaListQueryHandler : IRequestHandler<RezerwacjaListQuery, List<RezerwacjaDto>>
    {
        private readonly IRezerwacjaRepository _rezerwacjaRepository;

        public RezerwacjaListQueryHandler(IRezerwacjaRepository rezerwacjaRepository)
        {
            _rezerwacjaRepository = rezerwacjaRepository;
        }

        public async Task<List<RezerwacjaDto>> Handle(RezerwacjaListQuery request, CancellationToken cancellationToken)
        {
            var list = await _rezerwacjaRepository.GetList(request.PrzedsiebiorstwoId);

            return list.Select(x => new RezerwacjaDto
            {
                Id = x.Id,
                DataWaznosci = x.DataWaznosci,
                DokumentWydaniaId = x.DokumentWydaniaId,
                DataRezerwacji = x.DataRezerwacji,
                Opis = x.Opis,
                PozycjeRezerwacji = x.PozycjeRezerwacji.Select(p => new PozycjaRezerwacjiDto
                {
                    Id = p.Id,
                    Ilosc = p.Ilosc,
                    ProduktId = p.ProduktId
                }).ToList(),
                PrzedsiebiorstwoId = x.PrzedsiebiorstwoId,
                UzytkownikRezerwujacyId = x.UzytkownikRezerwujacyId
            }).ToList();
        }
    }
}