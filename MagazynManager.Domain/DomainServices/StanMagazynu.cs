using MagazynManager.Domain.Entities.Slowniki;
using System;

namespace MagazynManager.Domain.DomainServices
{
    public class StanMagazynu
    {
        public Guid ProduktId { get; set; }
        public decimal Ilosc { get; set; }
        public decimal CenaNetto { get; set; }
        public decimal CenaBrutto { get; set; }
        public decimal WartoscNetto { get; set; }
        public decimal WartoscVat { get; set; }
        public decimal WartoscBrutto { get; set; }
        public StawkaVat StawkaVat { get; internal set; }
    }
}