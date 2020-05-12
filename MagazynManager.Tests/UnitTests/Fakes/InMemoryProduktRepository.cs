using MagazynManager.Domain.Entities.Produkty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagazynManager.Tests.UnitTests.Fakes
{
    internal class InMemoryProduktRepository : IProduktRepository
    {
        private List<Produkt> _produktList;

        public InMemoryProduktRepository()
        {
            _produktList = new List<Produkt>();
        }

        public Task Delete(Guid id)
        {
            _produktList = _produktList.Where(x => x.Id != id).ToList();

            return Task.CompletedTask;
        }

        public Task<List<Produkt>> GetList(Guid przedsiebiorstwoId)
        {
            return Task.FromResult(_produktList.Where(x => x.Kategoria.PrzedsiebiorstwoId == przedsiebiorstwoId).ToList());
        }

        public Task<Guid> Save(Produkt produkt)
        {
            _produktList.Add(produkt);
            return Task.FromResult(produkt.Id);
        }
    }
}