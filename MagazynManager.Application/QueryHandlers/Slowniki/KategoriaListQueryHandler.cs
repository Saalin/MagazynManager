using MagazynManager.Application.Queries.Slowniki;
using MagazynManager.Domain.Entities;
using MagazynManager.Domain.Entities.Produkty;
using MagazynManager.Domain.Specification.Specifications;
using MagazynManager.Infrastructure.Dto.Slowniki;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MagazynManager.Application.QueryHandlers.Slowniki
{
    [QueryHandler]
    public class KategoriaListQueryHandler : IRequestHandler<KategoriaListQuery, List<KategoriaDto>>
    {
        private readonly ISlownikRepository<Kategoria> _repository;

        public KategoriaListQueryHandler(ISlownikRepository<Kategoria> repository)
        {
            _repository = repository;
        }

        public async Task<List<KategoriaDto>> Handle(KategoriaListQuery request, CancellationToken cancellationToken)
        {
            var kategorie = await _repository.GetList(new PrzedsiebiorstwoIdSpecification<Kategoria>(request.PrzedsiebiorstwoId));
            return kategorie.Select(x => new KategoriaDto
            {
                Id = x.Id,
                Name = x.Nazwa
            }).ToList();
        }
    }
}