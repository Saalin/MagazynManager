namespace MagazynManager.Application.DataProviders
{
    public partial class TokenManager
    {
        public class TokenValidationResult
        {
            public bool IsValid { get; set; }
            public IdentificationAggregate Identity { get; set; }
        }
    }
}