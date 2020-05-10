using System;

namespace MagazynManager.Infrastructure.InputModel.Rezerwacje
{
    public class PozycjaRezerwacjiCreateModel
    {
        public Guid ProduktId { get; set; }
        public decimal Ilosc { get; set; }
    }
}