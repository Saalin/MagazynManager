using System;

namespace MagazynManager.Infrastructure.Dto.Rezerwacje
{
    public class PozycjaRezerwacjiDto : BaseDto<Guid>
    {
        public Guid ProduktId { get; set; }
        public decimal Ilosc { get; set; }
    }
}