using MagazynManager.Domain.Entities.Kontrahent;

namespace MagazynManager.Infrastructure.InputModel.Slowniki
{
    public class KontrahentCreateModel
    {
        public string Nip { get; set; }
        public string Nazwa { get; set; }
        public string Skrot { get; set; }
        public TypKontrahenta TypKontrahenta { get; set; }
        public DaneAdresoweCreateModel DaneAdresowe { get; set; }
    }
}