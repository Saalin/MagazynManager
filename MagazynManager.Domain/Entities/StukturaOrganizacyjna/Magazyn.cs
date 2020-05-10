using MagazynManager.Domain.Entities.Produkty;
using System;

namespace MagazynManager.Domain.Entities.StukturaOrganizacyjna
{
    public class Magazyn : BaseEntity<Guid>, IPrzedsiebiorstwo
    {
        public string Skrot { get; set; }
        public string Nazwa { get; set; }
        public Guid PrzedsiebiorstwoId { get; set; }
    }
}