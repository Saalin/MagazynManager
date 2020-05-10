using System;

namespace MagazynManager.Infrastructure.Dto.Slowniki
{
    public class KategoriaDto : BaseDto<Guid>
    {
        public string Name { get; set; }
    }
}