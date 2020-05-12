using MagazynManager.Domain.Entities.Produkty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagazynManager.Tests.UnitTests.Fakes
{
    internal class InMemoryJednostkaMiaryRepository : IJednostkaMiaryRepository
    {
        private List<JednostkaMiary> _jednostkiMiary;

        public InMemoryJednostkaMiaryRepository()
        {
            _jednostkiMiary = new List<JednostkaMiary>();
        }

        public Task Delete(Guid id)
        {
            _jednostkiMiary = _jednostkiMiary.Where(x => x.Id != id).ToList();

            return Task.CompletedTask;
        }

        public Task<List<JednostkaMiary>> GetList(Guid przedsiebiorstwoId)
        {
            return Task.FromResult(_jednostkiMiary.Where(x => x.PrzedsiebiorstwoId == przedsiebiorstwoId).ToList());
        }

        public Task<Guid> Save(JednostkaMiary jednostkaMiary)
        {
            _jednostkiMiary.Add(jednostkaMiary);
            return Task.FromResult(jednostkaMiary.Id);
        }
    }
}