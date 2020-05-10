using MagazynManager.Application.Queries.Ewidencja;
using MagazynManager.Domain.Entities.Dokumenty;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MagazynManager.Application.QueryHandlers.Ewidencja
{
    [QueryHandler]
    public class DokumentPrzesunieciaListQueryHandler : IRequestHandler<DokumentPrzesunieciaListQuery, List<Dokument>>
    {
        private readonly IDokumentRepository _dokumentRepository;

        public DokumentPrzesunieciaListQueryHandler(IDokumentRepository dokumentRepository)
        {
            _dokumentRepository = dokumentRepository;
        }

        public async Task<List<Dokument>> Handle(DokumentPrzesunieciaListQuery request, CancellationToken cancellationToken)
        {
            var przesunieciaPlus = await _dokumentRepository.GetList(TypDokumentu.PrzesuniecieMiedzymagazynoweDodatnie, request.PrzedsiebiorstwoId);
            var przesunieciaMinus = await _dokumentRepository.GetList(TypDokumentu.PrzesuniecieMiedzymagazynoweUjemne, request.PrzedsiebiorstwoId);

            return przesunieciaMinus.Concat(przesunieciaPlus).ToList();
        }
    }
}