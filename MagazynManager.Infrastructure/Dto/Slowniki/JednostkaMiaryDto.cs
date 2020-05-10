using System;

namespace MagazynManager.Infrastructure.Dto.Slowniki
{
    public class JednostkaMiaryDto : BaseDto<Guid>
    {
        public string Name { get; set; }
    }
}