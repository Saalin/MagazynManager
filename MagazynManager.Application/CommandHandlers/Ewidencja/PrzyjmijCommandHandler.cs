using MagazynManager.Application.Commands.Ewidencja;
using MagazynManager.Domain.Entities;
using MagazynManager.Domain.Entities.Dokumenty;
using MagazynManager.Domain.Entities.Slowniki;
using MagazynManager.Domain.Entities.StukturaOrganizacyjna;
using MagazynManager.Domain.Specification.Specifications;
using MagazynManager.Domain.Specification.Technical;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MagazynManager.Application.CommandHandlers.Ewidencja
{
    [CommandHandler]
    public class PrzyjmijCommandHandler : IRequestHandler<PrzyjmijCommand, Guid>
    {
        private readonly IDokumentRepository _dokumentRepository;
        private readonly ISlownikRepository<Magazyn> _magazynRepository;

        public PrzyjmijCommandHandler(IDokumentRepository dokumentRepository, ISlownikRepository<Magazyn> magazynRepository)
        {
            _dokumentRepository = dokumentRepository;
            _magazynRepository = magazynRepository;
        }

        public async Task<Guid> Handle(PrzyjmijCommand request, CancellationToken cancellationToken)
        {
            var numer = request.Model.KontrahentId.HasValue ?
                await GetNumerDokumentuPZ(request.PrzedsiebiorstwoId, request.Model.Data.Year) :
                await GetNumerDokumentuPW(request.PrzedsiebiorstwoId, request.Model.Data.Year);

            var magazyn = await _magazynRepository.GetList(new IdSpecification<Magazyn, Guid>(request.Model.MagazynId));

            var dokument = new Dokument
            {
                Id = Guid.NewGuid(),
                Data = request.Model.Data,
                Magazyn = magazyn.Single(),
                KontrahentId = request.Model.KontrahentId,
                PozycjeDokumentu = request.Model.Pozycje.Select(x => new PozycjaDokumentu
                {
                    Id = Guid.NewGuid(),
                    ProduktId = x.ProduktId,
                    StawkaVat = x.StawkaVat,
                    Ilosc = x.Ilosc,
                    CenaNetto = x.CenaNetto,
                    CenaBrutto = CalculateCenaBrutto(x.CenaNetto, x.StawkaVat),
                    WartoscNetto = decimal.Round(x.CenaNetto * x.Ilosc, 2),
                    WartoscVat = decimal.Round(x.CenaNetto * x.Ilosc * x.StawkaVat.GetStawkaVat(), 2),
                    WartoscBrutto = decimal.Round(x.CenaNetto * x.Ilosc, 2) + decimal.Round(x.CenaNetto * x.Ilosc * x.StawkaVat.GetStawkaVat(), 2),
                }).ToList(),
                TypDokumentu = TypDokumentu.DokumentPrzyjecia,
                Numer = numer
            };

            return await _dokumentRepository.Save(dokument);
        }

        private async Task<string> GetNumerDokumentuPW(Guid przedsiebiorstwoId, int rok)
        {
            var spec = new AndSpecification<Dokument>(new PrzedsiebiorstwoIdSpecification<Dokument>(przedsiebiorstwoId),
                new DokumentTypSpecification(TypDokumentu.DokumentPrzyjecia));
            var dokumenty = await _dokumentRepository.GetList(spec);
            var liczbaDokumentow = dokumenty.Count(x => x.Data.Year == rok && !x.KontrahentId.HasValue);

            return $"PW/{liczbaDokumentow + 1}/{rok}";
        }

        private async Task<string> GetNumerDokumentuPZ(Guid przedsiebiorstwoId, int rok)
        {
            var spec = new AndSpecification<Dokument>(new PrzedsiebiorstwoIdSpecification<Dokument>(przedsiebiorstwoId),
                new DokumentTypSpecification(TypDokumentu.DokumentPrzyjecia));
            var dokumenty = await _dokumentRepository.GetList(spec);
            var liczbaDokumentow = dokumenty.Count(x => x.Data.Year == rok && x.KontrahentId.HasValue);

            return $"PZ/{liczbaDokumentow + 1}/{rok}";
        }

        private decimal CalculateCenaBrutto(decimal cenaNetto, StawkaVat stawkaVat)
        {
            return decimal.Round(cenaNetto + cenaNetto * stawkaVat.GetStawkaVat(), 2);
        }
    }
}