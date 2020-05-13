using MagazynManager.Application.Queries.Ewidencja;
using MagazynManager.Domain.Entities.Dokumenty;
using MagazynManager.Domain.Specification.Specifications;
using MagazynManager.Domain.Specification.Technical;
using MediatR;
using System;
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
            var przesunieciaPlus = await _dokumentRepository.GetList(GetSpecification(request.PrzedsiebiorstwoId, TypDokumentu.PrzesuniecieMiedzymagazynoweDodatnie));
            var przesunieciaMinus = await _dokumentRepository.GetList(GetSpecification(request.PrzedsiebiorstwoId, TypDokumentu.PrzesuniecieMiedzymagazynoweUjemne));

            return przesunieciaMinus.Concat(przesunieciaPlus).ToList();
        }

        private Specification<Dokument> GetSpecification(Guid przedsiebiorstwoId, TypDokumentu typDokumentu)
        {
            return new AndSpecification<Dokument>(new PrzedsiebiorstwoIdSpecification<Dokument>(przedsiebiorstwoId),
                new DokumentTypSpecification(typDokumentu));
        }
    }
}