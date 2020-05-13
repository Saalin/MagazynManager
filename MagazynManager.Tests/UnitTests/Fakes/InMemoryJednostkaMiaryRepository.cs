using MagazynManager.Domain.Entities;
using MagazynManager.Domain.Entities.Produkty;
using MagazynManager.Domain.Specification.Technical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagazynManager.Tests.UnitTests.Fakes
{
    internal class InMemoryJednostkaMiaryRepository : ISlownikRepository<JednostkaMiary>
    {
        private readonly List<JednostkaMiary> _jednostkiMiary;

        public InMemoryJednostkaMiaryRepository()
        {
            _jednostkiMiary = new List<JednostkaMiary>();
        }

        public Task Delete(JednostkaMiary entity)
        {
            foreach (var j in _jednostkiMiary)
            {
                if (j.Id == entity.Id)
                {
                    _jednostkiMiary.Remove(j);
                    return Task.CompletedTask;
                }
            }

            return Task.CompletedTask;
        }

        public Task<List<JednostkaMiary>> GetList(Specification<JednostkaMiary> specification)
        {
            return Task.FromResult(_jednostkiMiary.Where(specification.ToExpression().Compile()).ToList());
        }

        public Task<Guid> Save(JednostkaMiary entity)
        {
            _jednostkiMiary.Add(entity);
            return Task.FromResult(entity.Id);
        }
    }
}