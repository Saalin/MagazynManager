using MagazynManager.Application.Commands.Ewidencja;
using MagazynManager.Domain.DomainServices;
using MagazynManager.Domain.Entities;
using MagazynManager.Domain.Entities.Dokumenty;
using MagazynManager.Domain.Entities.Produkty;
using MagazynManager.Domain.Entities.Slowniki;
using MagazynManager.Domain.Entities.StukturaOrganizacyjna;
using MagazynManager.Domain.Specification.Specifications;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MagazynManager.Application.CommandHandlers.Ewidencja
{
    [CommandHandler]
    public class PrzesunCommandHandler : IRequestHandler<PrzesunCommand, Guid>
    {
        private readonly IDokumentRepository _dokumentRepository;
        private readonly StanAktualnyService _stanyAktualneService;
        private readonly ISlownikRepository<Produkt> _produktRepository;
        private readonly ISlownikRepository<Magazyn> _magazynRepository;

        public PrzesunCommandHandler(IDokumentRepository dokumentRepository, StanAktualnyService stanyAktualneService,
            ISlownikRepository<Produkt> produktRepository, ISlownikRepository<Magazyn> magazynRepository)
        {
            _dokumentRepository = dokumentRepository;
            _stanyAktualneService = stanyAktualneService;
            _produktRepository = produktRepository;
            _magazynRepository = magazynRepository;
        }

        public async Task<Guid> Handle(PrzesunCommand request, CancellationToken cancellationToken)
        {
            var stanyAktualne = await _stanyAktualneService.GetStanMagazynu(request.Model.MagazynWydajacyId, request.PrzedsiebiorstwoId);
            var orderedStanyAktualne = stanyAktualne.Where(x => x.Ilosc > 0)
                .OrderBy(x => x.CenaNetto).ThenBy(x => x.CenaBrutto).ToList();

            var produkty = await _produktRepository.GetList(new PrzedsiebiorstwoIdSpecification<Produkt>(request.PrzedsiebiorstwoId));

            var pozycjeDokumentuWydania = new List<PozycjaDokumentu>();
            var pozycjeDokumentuPrzyjecia = new List<PozycjaDokumentu>();
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

                    var pozycjaDokumentuPrzyjecia = new PozycjaDokumentu
                    {
                        Id = Guid.NewGuid(),
                        CenaNetto = stan.CenaNetto,
                        CenaBrutto = stan.CenaBrutto,
                        Ilosc = iloscWydawana,
                        ProduktId = await UtworzProduktNaMagazyniePrzyjecia(produkty, p.ProduktId, request.Model.MagazynPrzyjmujacyId),
                        StawkaVat = stan.StawkaVat,
                        WartoscBrutto = pozycjaDokumentuWydania.WartoscBrutto,
                        WartoscNetto = pozycjaDokumentuWydania.WartoscNetto,
                        WartoscVat = pozycjaDokumentuWydania.WartoscVat
                    };
                    pozycjeDokumentuPrzyjecia.Add(pozycjaDokumentuPrzyjecia);

                    iloscDoWydania -= pozycjaDokumentuWydania.Ilosc;
                }

                if (iloscDoWydania > 0)
                {
                    throw new BussinessException("Niewystarczający stan magazynowy");
                }
            }

            var magazynWydajacy = await _magazynRepository.GetList(new IdSpecification<Magazyn, Guid>(request.Model.MagazynWydajacyId));
            var magazynPrzyjmujacy = await _magazynRepository.GetList(new IdSpecification<Magazyn, Guid>(request.Model.MagazynPrzyjmujacyId));

            var dokumentMinus = new Dokument
            {
                Id = Guid.NewGuid(),
                Data = request.Model.Data,
                Magazyn = magazynWydajacy.Single(),
                Numer = $"MM-/{await GetLicznikDokumentu(request.PrzedsiebiorstwoId, request.Model.Data.Year)}/{request.Model.Data.Year}",
                TypDokumentu = TypDokumentu.PrzesuniecieMiedzymagazynoweUjemne,
                PozycjeDokumentu = pozycjeDokumentuWydania
            };

            var dokumentPlus = new Dokument
            {
                Id = Guid.NewGuid(),
                Data = request.Model.Data,
                Magazyn = magazynPrzyjmujacy.Single(),
                Numer = $"MM+/{await GetLicznikDokumentu(request.PrzedsiebiorstwoId, request.Model.Data.Year)}/{request.Model.Data.Year}",
                TypDokumentu = TypDokumentu.PrzesuniecieMiedzymagazynoweDodatnie,
                PozycjeDokumentu = pozycjeDokumentuPrzyjecia
            };

            await _dokumentRepository.Save(dokumentMinus);
            await _dokumentRepository.Save(dokumentPlus);

            return dokumentMinus.Id;
        }

        private async Task<Guid> UtworzProduktNaMagazyniePrzyjecia(List<Produkt> produkty, Guid produktId, Guid magazynPrzyjmujacyId)
        {
            var produktWydawany = produkty.Single(x => x.Id == produktId);
            var produktPrzyjmowany = produkty.SingleOrDefault(x => x.MagazynId == magazynPrzyjmujacyId && x.Skrot == produktWydawany.Skrot && x.Nazwa == produktWydawany.Nazwa);

            if (produktPrzyjmowany != null)
            {
                return produktPrzyjmowany.Id;
            }

            return await _produktRepository.Save(new Produkt
            {
                Id = Guid.NewGuid(),
                Kategoria = produktWydawany.Kategoria,
                JednostkaMiary = produktWydawany.JednostkaMiary,
                MagazynId = magazynPrzyjmujacyId,
                Nazwa = produktWydawany.Nazwa,
                Skrot = produktWydawany.Skrot
            });
        }

        private async Task<int> GetLicznikDokumentu(Guid przedsiebiorstwoId, int rok)
        {
            var dokumenty = await _dokumentRepository.GetList(new PrzedsiebiorstwoIdSpecification<Dokument>(przedsiebiorstwoId).And(
                new DokumentTypSpecification(TypDokumentu.DokumentPrzyjecia)));
            return dokumenty.Count(x => x.Data.Year == rok) + 1;
        }
    }
}