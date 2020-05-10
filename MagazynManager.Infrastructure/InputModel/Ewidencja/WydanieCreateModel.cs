using System;
using System.Collections.Generic;

namespace MagazynManager.Infrastructure.InputModel.Ewidencja
{
    public class WydanieCreateModel
    {
        public DateTime Data { get; set; }
        public Guid MagazynId { get; set; }
        public Guid? KontrahentId { get; set; }
        public List<PozycjaWydaniaModel> Pozycje { get; set; }
    }
}