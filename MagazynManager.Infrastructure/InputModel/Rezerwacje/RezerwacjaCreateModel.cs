using System;
using System.Collections.Generic;

namespace MagazynManager.Infrastructure.InputModel.Rezerwacje
{
    public class RezerwacjaCreateModel
    {
        public DateTime DataRezerwacji { get; set; }
        public DateTime DataWaznosci { get; set; }
        public string Opis { get; set; }
        public List<PozycjaRezerwacjiCreateModel> Pozycje { get; set; }
    }
}