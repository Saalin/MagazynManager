using MagazynManager.Application.Queries.Ewidencja;
using MagazynManager.Domain.Entities.Dokumenty;
using MagazynManager.Domain.Specification.Specifications;
using MagazynManager.Domain.Specification.Technical;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MagazynManager.Application.QueryHandlers.Ewidencja
{
    [QueryHandler]
    public class DokumentWydaniaListQueryHandler : IRequestHandler<DokumentWydaniaListQuery, List<Dokument>>
    {
        private readonly IDokumentRepository _dokumentRepository;

        public DokumentWydaniaListQueryHandler(IDokumentRepository dokumentRepository)
        {
            _dokumentRepository = dokumentRepository;
        }

        public async Task<List<Dokument>> Handle(DokumentWydaniaListQuery request, CancellationToken cancellationToken)
        {
            var spec = new AndSpecification<Dokument>(new IdSpecification<Dokument, Guid>(request.PrzedsiebiorstwoId),
                new DokumentTypSpecification(TypDokumentu.DokumentWydania));
            return await _dokumentRepository.GetList(spec);
        }
    }
}