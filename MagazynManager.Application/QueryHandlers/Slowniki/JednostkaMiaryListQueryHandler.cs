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
    public class JednostkaMiaryListQueryHandler : IRequestHandler<JednostkaMiaryListQuery, List<JednostkaMiaryDto>>
    {
        private readonly ISlownikRepository<JednostkaMiary> _repository;

        public JednostkaMiaryListQueryHandler(ISlownikRepository<JednostkaMiary> repository)
        {
            _repository = repository;
        }

        public async Task<List<JednostkaMiaryDto>> Handle(JednostkaMiaryListQuery request, CancellationToken cancellationToken)
        {
            var jednostkiMiary = await _repository.GetList(new PrzedsiebiorstwoIdSpecification<JednostkaMiary>(request.PrzedsiebiorstwoId));

            return jednostkiMiary.Select(x => new JednostkaMiaryDto
            {
                Id = x.Id,
                Name = x.Nazwa
            }).ToList();
        }
    }
}