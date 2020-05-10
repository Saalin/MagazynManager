using System;

namespace MagazynManager.Domain.Entities.Produkty
{
    public class Kategoria : BaseEntity<Guid>, IPrzedsiebiorstwo
    {
        public string Nazwa { get; }
        public Guid PrzedsiebiorstwoId { get; set; }

        public Kategoria(Guid id, string nazwa, Guid przedsiebiorstwoId)
        {
            Id = id;
            Nazwa = nazwa;
            PrzedsiebiorstwoId = przedsiebiorstwoId;
        }

        public Kategoria(string nazwa, Guid przedsiebiorstwoId)
        {
            Id = Guid.NewGuid();
            Nazwa = nazwa;
            PrzedsiebiorstwoId = przedsiebiorstwoId;
        }
    }
}