using System;

namespace MagazynManager.Application.DataProviders
{
    public partial class TokenManager
    {
        public class IdentificationAggregate
        {
            public Guid UserId { get; set; }
            public Guid PrzedsiebiorstwoId { get; set; }
        }
    }
}