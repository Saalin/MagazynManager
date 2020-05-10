using System;

namespace MagazynManager.Infrastructure.Dto.Slowniki
{
    public class ProduktDto : BaseDto<Guid>
    {
        public string Nazwa { get; set; }
        public string Skrot { get; set; }
        public JednostkaMiaryDto JednostkaMiary { get; set; }
        public KategoriaDto Kategoria { get; set; }
    }
}