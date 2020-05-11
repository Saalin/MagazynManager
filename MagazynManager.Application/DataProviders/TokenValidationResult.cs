namespace MagazynManager.Application.DataProviders
{
    public class TokenValidationResult
    {
        public bool IsValid { get; set; }
        public IdentificationAggregate Identity { get; set; }
    }
}