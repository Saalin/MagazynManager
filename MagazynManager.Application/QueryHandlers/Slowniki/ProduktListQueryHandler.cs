using MagazynManager.Application.Queries.Slowniki;
using MagazynManager.Domain.Entities.Produkty;
using MagazynManager.Infrastructure.Dto.Slowniki;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MagazynManager.Application.QueryHandlers.Slowniki
{
    [QueryHandler]
    public class ProduktListQueryHandler : IRequestHandler<ProduktListQuery, List<ProduktDto>>
    {
        private readonly IProduktRepository _produktRepository;

        public ProduktListQueryHandler(IProduktRepository produktRepository)
        {
            _produktRepository = produktRepository;
        }

        public async Task<List<ProduktDto>> Handle(ProduktListQuery request, CancellationToken cancellationToken)
        {
            var result = await _produktRepository.GetList(request.PrzedsiebiorstwoId);
            return result.Select(x => new ProduktDto
            {
                Id = x.Id,
                Skrot = x.Skrot,
                Nazwa = x.Nazwa,
                JednostkaMiary = new JednostkaMiaryDto
                {
                    Id = x.JednostkaMiary.Id,
                    Name = x.JednostkaMiary.Nazwa
                },
                Kategoria = new KategoriaDto
                {
                    Id = x.Kategoria.Id,
                    Name = x.Kategoria.Nazwa
                }
            }).ToList();
        }
    }
}