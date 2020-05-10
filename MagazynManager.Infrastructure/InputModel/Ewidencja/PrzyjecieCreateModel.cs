using System;
using System.Collections.Generic;

namespace MagazynManager.Infrastructure.InputModel.Ewidencja
{
    public class PrzyjecieCreateModel
    {
        public Guid MagazynId { get; set; }
        public Guid? KontrahentId { get; set; }
        public DateTime Data { get; set; }
        public List<PrzyjeciePozycjaDokumentuCreateModel> Pozycje { get; set; }
    }
}