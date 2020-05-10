using MagazynManager.Domain.Entities.Produkty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagazynManager.Tests.UnitTests.Fakes
{
    internal class InMemoryKategoriaRepository : IKategoriaRepository
    {
        private List<Kategoria> _kategorie;

        public InMemoryKategoriaRepository()
        {
            _kategorie = new List<Kategoria>();
        }

        public Task Delete(Guid id)
        {
            _kategorie = _kategorie.Where(x => x.Id != id).ToList();

            return Task.CompletedTask;
        }

        public Task<List<Kategoria>> GetList(Guid przedsiebiorstwoId)
        {
            return Task.FromResult(_kategorie.Where(x => x.PrzedsiebiorstwoId == przedsiebiorstwoId).ToList());
        }

        public Task<Guid> Save(Kategoria kategoria)
        {
            _kategorie.Add(kategoria);
            return Task.FromResult(kategoria.Id);
        }
    }
}