using System;

namespace MagazynManager.Infrastructure.Dto.Ewidencja
{
    public class StanAktualnyDto : BaseDto<int>
    {
        public Guid ProduktId { get; set; }
        public string Nazwa { get; set; }
        public string Skrot { get; set; }
        public string JednostkaMiary { get; set; }
        public decimal Ilosc { get; set; }
        public decimal WartoscNetto { get; set; }
        public decimal WartoscVat { get; set; }
        public decimal WartoscBrutto { get; set; }
    }
}