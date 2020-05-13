using MagazynManager.Domain.Entities;
using MagazynManager.Domain.Entities.StukturaOrganizacyjna;
using MagazynManager.Domain.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagazynManager.Tests.UnitTests
{
    public class InMemoryMagazynRepository : ISlownikRepository<Magazyn>
    {
        private readonly List<Magazyn> _magazyny;

        public InMemoryMagazynRepository()
        {
            _magazyny = new List<Magazyn>();
        }

        public Task Delete(Magazyn entity)
        {
            foreach (var m in _magazyny)
            {
                if (m.Id == entity.Id)
                {
                    _magazyny.Remove(m);
                    return Task.CompletedTask;
                }
            }
            return Task.CompletedTask;
        }

        public Task<List<Magazyn>> GetList(Specification<Magazyn> specification)
        {
            return Task.FromResult(_magazyny.Where(specification.ToExpression().Compile()).ToList());
        }

        public Task<Guid> Save(Magazyn magazyn)
        {
            _magazyny.Add(magazyn);
            return Task.FromResult(magazyn.Id);
        }
    }
}