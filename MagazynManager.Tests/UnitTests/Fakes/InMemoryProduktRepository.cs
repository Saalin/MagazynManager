using MagazynManager.Domain.Entities;
using MagazynManager.Domain.Entities.Produkty;
using MagazynManager.Domain.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagazynManager.Tests.UnitTests.Fakes
{
    internal class InMemoryProduktRepository : ISlownikRepository<Produkt>
    {
        private readonly List<Produkt> _produktList;

        public InMemoryProduktRepository()
        {
            _produktList = new List<Produkt>();
        }

        public Task Delete(Produkt entity)
        {
            foreach (var m in _produktList)
            {
                if (m.Id == entity.Id)
                {
                    _produktList.Remove(m);
                    return Task.CompletedTask;
                }
            }

            return Task.CompletedTask;
        }

        public Task<List<Produkt>> GetList(Specification<Produkt> specification)
        {
            return Task.FromResult(_produktList.Where(specification.ToExpression().Compile()).ToList());
        }

        public Task<Guid> Save(Produkt produkt)
        {
            _produktList.Add(produkt);
            return Task.FromResult(produkt.Id);
        }
    }
}