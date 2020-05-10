using MagazynManager.Application.Queries.Ewidencja;
using MagazynManager.Domain.Entities.Dokumenty;
using MediatR;
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
            return await _dokumentRepository.GetList(TypDokumentu.DokumentWydania, request.PrzedsiebiorstwoId);
        }
    }
}