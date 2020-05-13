using System;

namespace MagazynManager.Domain.Entities.Produkty
{
    public class Produkt : BaseEntity<Guid>, IPrzedsiebiorstwo
    {
        public string Skrot { get; set; }
        public string Nazwa { get; set; }
        public JednostkaMiary JednostkaMiary { get; set; }
        public Kategoria Kategoria { get; set; }
        public Guid MagazynId { get; set; }
        public Guid PrzedsiebiorstwoId { get => Kategoria.PrzedsiebiorstwoId; }
    }
}