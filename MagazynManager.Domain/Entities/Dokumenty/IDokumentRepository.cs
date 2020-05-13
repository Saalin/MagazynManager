using MagazynManager.Domain.Specification.Technical;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MagazynManager.Domain.Entities.Dokumenty
{
    public interface IDokumentRepository
    {
        Task<List<Dokument>> GetList(Specification<Dokument> specification);

        Task<Guid> Save(Dokument dokument);
    }
}