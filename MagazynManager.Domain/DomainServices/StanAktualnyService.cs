using MagazynManager.Domain.Entities.Dokumenty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagazynManager.Domain.DomainServices
{
    [DomainService]
    public class StanAktualnyService
    {
        private readonly IDokumentRepository _dokumentRepository;

        public StanAktualnyService(IDokumentRepository dokumentRepository)
        {
            _dokumentRepository = dokumentRepository;
        }

        public async Task<List<StanMagazynu>> GetStanMagazynu(Guid magazynId, Guid przedsiebiorstwoId)
        {
            var przyjecia = await _dokumentRepository.GetList(TypDokumentu.DokumentPrzyjecia, przedsiebiorstwoId);
            var wydania = await _dokumentRepository.GetList(TypDokumentu.DokumentWydania, przedsiebiorstwoId);
            var przesunieciaPlus = await _dokumentRepository.GetList(TypDokumentu.PrzesuniecieMiedzymagazynoweDodatnie, przedsiebiorstwoId);
            var przesunieciaMinus = await _dokumentRepository.GetList(TypDokumentu.PrzesuniecieMiedzymagazynoweUjemne, przedsiebiorstwoId);

            var stanAktualny = przyjecia.Where(x => x.MagazynId == magazynId)
                .Concat(przesunieciaPlus.Where(x => x.MagazynId == magazynId))
                .SelectMany(x => x.PozycjeDokumentu).GroupBy(x => new { x.ProduktId, x.CenaNetto, x.CenaBrutto, x.StawkaVat }).Select(x => new StanMagazynu
                {
                    ProduktId = x.Key.ProduktId,
                    CenaNetto = x.Key.CenaNetto,
                    CenaBrutto = x.Key.CenaBrutto,
                    StawkaVat = x.Key.StawkaVat,
                    Ilosc = x.Sum(pd => pd.Ilosc),
                    WartoscNetto = x.Sum(pd => pd.WartoscNetto),
                    WartoscVat = x.Sum(pd => pd.WartoscVat),
                    WartoscBrutto = x.Sum(pd => pd.WartoscBrutto)
                }).ToList();

            var pozycjeWydania = wydania.Where(x => x.MagazynId == magazynId)
                .Concat(przesunieciaMinus.Where(x => x.MagazynId == magazynId)).SelectMany(x => x.PozycjeDokumentu);
            foreach (var wydanie in pozycjeWydania)
            {
                var odpowiadajacyStan = stanAktualny.Single(x => x.ProduktId == wydanie.ProduktId && x.CenaNetto == wydanie.CenaNetto && x.CenaBrutto == wydanie.CenaBrutto);
                odpowiadajacyStan.Ilosc -= wydanie.Ilosc;
                odpowiadajacyStan.WartoscNetto -= wydanie.WartoscNetto;
                odpowiadajacyStan.WartoscVat -= wydanie.WartoscVat;
                odpowiadajacyStan.WartoscBrutto -= wydanie.WartoscBrutto;
            }

            return stanAktualny;
        }
    }
}