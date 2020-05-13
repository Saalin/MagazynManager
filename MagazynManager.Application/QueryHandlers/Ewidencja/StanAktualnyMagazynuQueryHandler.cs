using MagazynManager.Application.Queries.Ewidencja;
using MagazynManager.Domain.DomainServices;
using MagazynManager.Domain.Entities;
using MagazynManager.Domain.Entities.Produkty;
using MagazynManager.Infrastructure.Dto.Ewidencja;
using MagazynManager.Infrastructure.Specifications;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MagazynManager.Application.QueryHandlers.Ewidencja
{
    [QueryHandler]
    public class StanAktualnyMagazynuQueryHandler : IRequestHandler<StanAktualnyMagazynuQuery, List<StanAktualnyDto>>
    {
        private readonly StanAktualnyService wydanieService;
        private readonly ISlownikRepository<Produkt> _produktRepository;

        public StanAktualnyMagazynuQueryHandler(StanAktualnyService dokumentRepository, ISlownikRepository<Produkt> produktRepository)
        {
            wydanieService = dokumentRepository;
            _produktRepository = produktRepository;
        }

        public async Task<List<StanAktualnyDto>> Handle(StanAktualnyMagazynuQuery request, CancellationToken cancellationToken)
        {
            var listaProduktow = await _produktRepository.GetList(new PrzedsiebiorstwoSpecification<Produkt>(request.PrzedsiebiorstwoId));
            var stanAktualny = await wydanieService.GetStanMagazynu(request.MagazynId, request.PrzedsiebiorstwoId);

            return stanAktualny.GroupBy(x => x.ProduktId).Select((x, idx) => new StanAktualnyDto
            {
                Id = idx + 1,
                ProduktId = x.Key,
                Nazwa = listaProduktow.Single(p => p.Id == x.Key).Nazwa,
                Skrot = listaProduktow.Single(p => p.Id == x.Key).Skrot,
                JednostkaMiary = listaProduktow.Single(p => p.Id == x.Key).JednostkaMiary.Nazwa,
                Ilosc = x.Sum(pd => pd.Ilosc),
                WartoscNetto = x.Sum(pd => pd.WartoscNetto),
                WartoscVat = x.Sum(pd => pd.WartoscVat),
                WartoscBrutto = x.Sum(pd => pd.WartoscBrutto)
            }).ToList();
        }
    }
}