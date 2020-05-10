using MagazynManager.Domain.Entities.Slowniki;
using System;

namespace MagazynManager.Infrastructure.InputModel.Ewidencja
{
    public class PrzyjeciePozycjaDokumentuCreateModel
    {
        public Guid ProduktId { get; set; }
        public StawkaVat StawkaVat { get; set; }
        public decimal CenaNetto { get; set; }
        public decimal Ilosc { get; set; }
    }
}