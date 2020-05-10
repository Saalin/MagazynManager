using System;

namespace MagazynManager.Domain.Entities.Produkty
{
    public class JednostkaMiary : BaseEntity<Guid>, IPrzedsiebiorstwo
    {
        public string Nazwa { get; set; }
        public Guid PrzedsiebiorstwoId { get; set; }

        public JednostkaMiary(string nazwa, Guid przedsiebiorstwoId)
        {
            Id = Guid.NewGuid();
            Nazwa = nazwa;
            PrzedsiebiorstwoId = przedsiebiorstwoId;
        }

        public JednostkaMiary(Guid id, string nazwa, Guid przedsiebiorstwoId)
        {
            Id = id;
            Nazwa = nazwa;
            PrzedsiebiorstwoId = przedsiebiorstwoId;
        }
    }
}