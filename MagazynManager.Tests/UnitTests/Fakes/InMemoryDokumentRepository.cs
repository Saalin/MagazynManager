using MagazynManager.Domain.Entities.Dokumenty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagazynManager.Tests.UnitTests
{
    public class InMemoryDokumentRepository : IDokumentRepository
    {
        private readonly List<Dokument> _dokumenty;

        public InMemoryDokumentRepository()
        {
            _dokumenty = new List<Dokument>();
        }

        public Task<List<Dokument>> GetList(TypDokumentu typ, Guid przedsiebiorstwoId)
        {
            return Task.FromResult(_dokumenty.Where(x => x.TypDokumentu == typ).ToList());
        }

        public Task<Guid> Save(Dokument dokument)
        {
            _dokumenty.Add(dokument);

            return Task.FromResult(dokument.Id);
        }
    }
}