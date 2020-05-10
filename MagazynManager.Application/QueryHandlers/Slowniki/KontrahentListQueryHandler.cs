using MagazynManager.Application.Queries.Slowniki;
using MagazynManager.Domain.Entities.Kontrahent;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MagazynManager.Application.QueryHandlers.Slowniki
{
    [QueryHandler]
    public class KontrahentListQueryHandler : IRequestHandler<KontrahentListQuery, List<Kontrahent>>
    {
        private readonly IKontrahentRepository _repository;

        public KontrahentListQueryHandler(IKontrahentRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Kontrahent>> Handle(KontrahentListQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetList(request.PrzedsiebiorstwoId);
        }
    }
}