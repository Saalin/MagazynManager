using MagazynManager.Domain.Entities.Slowniki;
using MagazynManager.Infrastructure.InputModel.Ewidencja;
using System;
using System.Collections.Generic;

namespace MagazynManager.Tests.ObjectMothers
{
    public static class DokumentObjectMother
    {
        public static PrzyjecieCreateModel GetDokumentPrzyjeciaZJednaPozycja(Guid magazynId, Guid produktId, decimal ilosc, Guid? kontrahentId )
        {
            return new PrzyjecieCreateModel
            {
                MagazynId = magazynId,
                Data = DateTime.Now,
                KontrahentId = kontrahentId,
                Pozycje = new List<PrzyjeciePozycjaDokumentuCreateModel>
                {
                    new PrzyjeciePozycjaDokumentuCreateModel
                    {
                        ProduktId = produktId,
                        CenaNetto = 1M,
                        Ilosc = ilosc,
                        StawkaVat = StawkaVat.DwadziesciaTrzyProcent
                    }
                }
            };
        }

        public static PrzyjecieCreateModel GetDokumentPrzyjeciaZJednaPozycja(Guid magazynId, Guid produktId, decimal ilosc)
        {
            return GetDokumentPrzyjeciaZJednaPozycja(magazynId, produktId, ilosc, null);
        }

        public static WydanieCreateModel GetDokumentWydaniaZJednaPozycja(Guid magazynId, Guid produktId, decimal ilosc, Guid? kontrahentId)
        {
            return new WydanieCreateModel
            {
                MagazynId = magazynId,
                Data = DateTime.Now,
                KontrahentId = kontrahentId,
                Pozycje = new List<PozycjaWydaniaModel>
                {
                    new PozycjaWydaniaModel
                    {
                        ProduktId = produktId,
                        Ilosc = ilosc
                    }
                }
            };
        }

        public static WydanieCreateModel GetDokumentWydaniaZJednaPozycja(Guid magazynId, Guid produktId, decimal ilosc)
        {
            return GetDokumentWydaniaZJednaPozycja(magazynId, produktId, ilosc, null);
        }

        public static PrzesuniecieCreateModel GetPrzesuniecieZJednaPozycja(Guid magazynWydaniaId, Guid magazynPrzyjeciaId, Guid produktId, decimal ilosc)
        {
            return new PrzesuniecieCreateModel
            {
                MagazynWydajacyId = magazynWydaniaId,
                MagazynPrzyjmujacyId = magazynPrzyjeciaId,
                Data = DateTime.Now,
                Pozycje = new List<PozycjaWydaniaModel>
                {
                    new PozycjaWydaniaModel
                    {
                        ProduktId = produktId,
                        Ilosc = ilosc
                    }
                }
            };
        }
    }
}