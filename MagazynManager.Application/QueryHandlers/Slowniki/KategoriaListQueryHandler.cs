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
    public class KategoriaListQueryHandler : IRequestHandler<KategoriaListQuery, List<KategoriaDto>>
    {
        private readonly IKategoriaRepository _repository;

        public KategoriaListQueryHandler(IKategoriaRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<KategoriaDto>> Handle(KategoriaListQuery request, CancellationToken cancellationToken)
        {
            var kategorie = await _repository.GetList(request.PrzedsiebiorstwoId);
            return kategorie.Select(x => new KategoriaDto
            {
                Id = x.Id,
                Name = x.Nazwa
            }).ToList();
        }
    }
}