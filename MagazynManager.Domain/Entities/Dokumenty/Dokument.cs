using MagazynManager.Domain.Entities.Produkty;
using MagazynManager.Domain.Entities.StukturaOrganizacyjna;
using System;
using System.Collections.Generic;

namespace MagazynManager.Domain.Entities.Dokumenty
{
    public class Dokument : BaseEntity<Guid>, IPrzedsiebiorstwo
    {
        public string Numer { get; set; }
        public Guid? KontrahentId { get; set; }
        public Magazyn Magazyn { get; set; }
        public DateTime Data { get; set; }
        public List<PozycjaDokumentu> PozycjeDokumentu { get; set; }
        public TypDokumentu TypDokumentu { get; set; }
        public Guid PrzedsiebiorstwoId => Magazyn.PrzedsiebiorstwoId;
    }
}