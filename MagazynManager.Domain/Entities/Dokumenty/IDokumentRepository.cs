using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MagazynManager.Domain.Entities.Dokumenty
{
    public interface IDokumentRepository
    {
        Task<List<Dokument>> GetList(TypDokumentu typ, Guid przedsiebiorstwoId);

        Task<Guid> Save(Dokument dokument);
    }
}