using MagazynManager.Domain.Entities.Produkty;
using System;

namespace MagazynManager.Domain.Entities.Kontrahent
{
    public class Kontrahent : BaseEntity<Guid>, IPrzedsiebiorstwo
    {
        public Guid PrzedsiebiorstwoId { get; set; }
        public string Nip { get; set; }
        public string Nazwa { get; set; }
        public string Skrot { get; set; }
        public TypKontrahenta TypKontrahenta { get; set; }
        public DaneAdresowe DaneAdresowe { get; set; }
    }
}