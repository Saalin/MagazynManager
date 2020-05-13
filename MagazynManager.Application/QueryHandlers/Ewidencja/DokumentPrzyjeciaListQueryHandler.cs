using MagazynManager.Application.Queries.Ewidencja;
using MagazynManager.Domain.Entities.Dokumenty;
using MagazynManager.Domain.Specification.Specifications;
using MagazynManager.Domain.Specification.Technical;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MagazynManager.Application.QueryHandlers.Ewidencja
{
    [QueryHandler]
    public class DokumentPrzyjeciaListQueryHandler : IRequestHandler<DokumentPrzyjeciaListQuery, List<Dokument>>
    {
        private readonly IDokumentRepository _dokumentRepository;

        public DokumentPrzyjeciaListQueryHandler(IDokumentRepository dokumentRepository)
        {
            _dokumentRepository = dokumentRepository;
        }

        public async Task<List<Dokument>> Handle(DokumentPrzyjeciaListQuery request, CancellationToken cancellationToken)
        {
            var spec = new AndSpecification<Dokument>(new PrzedsiebiorstwoIdSpecification<Dokument>(request.PrzedsiebiorstwoId),
                new DokumentTypSpecification(TypDokumentu.DokumentPrzyjecia));
            return await _dokumentRepository.GetList(spec);
        }
    }
}