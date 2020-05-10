using System;
using System.Collections.Generic;

namespace MagazynManager.Domain.Entities.Dokumenty
{
    public class Dokument : BaseEntity<Guid>
    {
        public string Numer { get; set; }
        public Guid? KontrahentId { get; set; }
        public Guid MagazynId { get; set; }
        public DateTime Data { get; set; }
        public List<PozycjaDokumentu> PozycjeDokumentu { get; set; }
        public TypDokumentu TypDokumentu { get; set; }
    }
}