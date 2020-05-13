using MagazynManager.Domain.Entities.Dokumenty;
using MagazynManager.Domain.Specification.Technical;
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

        public Task<List<Dokument>> GetList(Specification<Dokument> specification)
        {
            return Task.FromResult(_dokumenty.AsQueryable().Where(specification.ToExpression()).ToList());
        }

        public Task<Guid> Save(Dokument dokument)
        {
            _dokumenty.Add(dokument);

            return Task.FromResult(dokument.Id);
        }
    }
}