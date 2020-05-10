using System;

namespace MagazynManager.Domain.Entities.Rezerwacje
{
    public class PozycjaRezerwacji : BaseEntity<Guid>
    {
        public Guid ProduktId { get; }
        public decimal Ilosc { get; }

        public PozycjaRezerwacji(Guid id, Guid produktId, decimal ilosc)
        {
            Id = id;
            ProduktId = produktId;
            Ilosc = ilosc;
        }

        public PozycjaRezerwacji(Guid produktId, decimal ilosc)
        {
            Id = Guid.NewGuid();
            ProduktId = produktId;
            Ilosc = ilosc;
        }
    }
}