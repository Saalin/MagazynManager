using MagazynManager.Application.Commands.Ewidencja;
using MagazynManager.Domain.DomainServices;
using MagazynManager.Domain.Entities;
using MagazynManager.Domain.Entities.Dokumenty;
using MagazynManager.Domain.Entities.Slowniki;
using MagazynManager.Domain.Entities.StukturaOrganizacyjna;
using MagazynManager.Domain.Specification.Specifications;
using MagazynManager.Domain.Specification.Technical;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MagazynManager.Application.CommandHandlers.Ewidencja
{
    [CommandHandler]
    public class WydajCommandHandler : IRequestHandler<WydajCommand, Guid>
    {
        private readonly IDokumentRepository _dokumentRepository;
        private readonly ISlownikRepository<Magazyn> _magazynRepository;
        private readonly StanAktualnyService _stanyAktualneService;

        public WydajCommandHandler(IDokumentRepository dokumentRepository, StanAktualnyService stanyAktualneService, ISlownikRepository<Magazyn> magazynRepository)
        {
            _dokumentRepository = dokumentRepository;
            _stanyAktualneService = stanyAktualneService;
            _magazynRepository = magazynRepository;
        }

        public async Task<Guid> Handle(WydajCommand request, CancellationToken cancellationToken)
        {
            var numer = request.Model.KontrahentId.HasValue ?
                await GetNumerDokumentuWZ(request.PrzedsiebiorstwoId, request.Model.Data.Year) :
                await GetNumerDokumentuRW(request.PrzedsiebiorstwoId, request.Model.Data.Year);

            var stanyAktualne = await _stanyAktualneService.GetStanMagazynu(request.Model.MagazynId, request.PrzedsiebiorstwoId);
            var orderedStanyAktualne = stanyAktualne.Where(x => x.Ilosc > 0)
                .OrderBy(x => x.CenaNetto).ThenBy(x => x.CenaBrutto).ToList();

            var pozycjeDokumentuWydania = new List<PozycjaDokumentu>();
            foreach (var p in request.Model.Pozycje)
            {
                var iloscDoWydania = p.Ilosc;
                for (var i = 0; i < orderedStanyAktualne.Count(x => x.ProduktId == p.ProduktId) && iloscDoWydania > 0; i++)
                {
                    var stan = orderedStanyAktualne.Where(x => x.ProduktId == p.ProduktId).ToList()[i];
                    var iloscWydawana = iloscDoWydania > stan.Ilosc ? stan.Ilosc : iloscDoWydania;
                    var pozycjaDokumentuWydania = new PozycjaDokumentu
                    {
                        Id = Guid.NewGuid(),
                        CenaNetto = stan.CenaNetto,
                        CenaBrutto = stan.CenaBrutto,
                        Ilosc = iloscWydawana,
                        ProduktId = stan.ProduktId,
                        StawkaVat = stan.StawkaVat,
                        WartoscNetto = decimal.Round(stan.Ilosc * iloscWydawana),
                        WartoscVat = decimal.Round(stan.Ilosc * iloscWydawana * stan.StawkaVat.GetStawkaVat()),
                        WartoscBrutto = decimal.Round(stan.Ilosc * iloscWydawana) + decimal.Round(stan.Ilosc * iloscWydawana * stan.StawkaVat.GetStawkaVat())
                    };
                    pozycjeDokumentuWydania.Add(pozycjaDokumentuWydania);

                    iloscDoWydania -= pozycjaDokumentuWydania.Ilosc;
                }

                if (iloscDoWydania > 0)
                {
                    throw new BussinessException("Niewystarczający stan magazynowy");
                }
            }

            var magazyn = await _magazynRepository.GetList(new IdSpecification<Magazyn, Guid>(request.Model.MagazynId));
            var dokument = new Dokument
            {
                Id = Guid.NewGuid(),
                Data = request.Model.Data,
                Magazyn = magazyn.Single(),
                Numer = numer,
                TypDokumentu = TypDokumentu.PrzesuniecieMiedzymagazynoweUjemne,
                PozycjeDokumentu = pozycjeDokumentuWydania
            };

            return await _dokumentRepository.Save(dokument);
        }

        private async Task<string> GetNumerDokumentuRW(Guid przedsiebiorstwoId, int rok)
        {
            var spec = new AndSpecification<Dokument>(new PrzedsiebiorstwoIdSpecification<Dokument>(przedsiebiorstwoId),
                new DokumentTypSpecification(TypDokumentu.DokumentPrzyjecia));
            var dokumenty = await _dokumentRepository.GetList(spec);
            var licznik = dokumenty.Count(x => x.Data.Year == rok && !x.KontrahentId.HasValue);
            return $"RW/{licznik + 1}/{rok}";
        }

        private async Task<string> GetNumerDokumentuWZ(Guid przedsiebiorstwoId, int rok)
        {
            var spec = new AndSpecification<Dokument>(new PrzedsiebiorstwoIdSpecification<Dokument>(przedsiebiorstwoId),
                new DokumentTypSpecification(TypDokumentu.DokumentPrzyjecia));
            var dokumenty = await _dokumentRepository.GetList(spec);
            var liczbaDokumentow = dokumenty.Count(x => x.Data.Year == rok && x.KontrahentId.HasValue);

            return $"PZ/{liczbaDokumentow + 1}/{rok}";
        }
    }
}