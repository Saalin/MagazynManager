using MagazynManager.Infrastructure.InputModel.Rezerwacje;
using System;
using System.Collections.Generic;

namespace MagazynManager.Tests.ObjectMothers
{
    public static class RezerwacjaObjectMother
    {
        public static RezerwacjaCreateModel GetPrzedawnionaRezerwacja(Guid produktId)
        {
            return new RezerwacjaCreateModel
            {
                DataRezerwacji = DateTime.Now.AddDays(-14),
                DataWaznosci = DateTime.Now.AddDays(-7),
                Opis = string.Empty,
                Pozycje = new List<PozycjaRezerwacjiCreateModel>
                {
                    new PozycjaRezerwacjiCreateModel
                    {
                        ProduktId = produktId,
                        Ilosc = 42
                    }
                }
            };
        }

        public static RezerwacjaCreateModel GetPoprawanaRezerwacja(Guid produktId)
        {
            return new RezerwacjaCreateModel
            {
                DataRezerwacji = DateTime.Now,
                DataWaznosci = DateTime.Now.AddDays(7),
                Opis = string.Empty,
                Pozycje = new List<PozycjaRezerwacjiCreateModel>
                {
                    new PozycjaRezerwacjiCreateModel
                    {
                        ProduktId = produktId,
                        Ilosc = 42
                    }
                }
            };
        }
    }
}