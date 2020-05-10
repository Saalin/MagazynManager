using System;
using System.Collections.Generic;

namespace MagazynManager.Infrastructure.InputModel.Ewidencja
{
    public class PrzesuniecieCreateModel
    {
        public DateTime Data { get; set; }
        public Guid MagazynWydajacyId { get; set; }
        public Guid MagazynPrzyjmujacyId { get; set; }
        public List<PozycjaWydaniaModel> Pozycje { get; set; }
    }
}