using MagazynManager.Application.Queries.Slowniki;
using MagazynManager.Domain.Entities.StukturaOrganizacyjna;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MagazynManager.Application.QueryHandlers.Slowniki
{
    [QueryHandler]
    public class MagazynListQueryHandler : IRequestHandler<MagazynListQuery, List<Magazyn>>
    {
        private readonly IMagazynRepository _repository;

        public MagazynListQueryHandler(IMagazynRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Magazyn>> Handle(MagazynListQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetList(request.PrzedsiebiorstwoId);
        }
    }
}