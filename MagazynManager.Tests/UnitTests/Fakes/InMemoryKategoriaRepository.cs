using MagazynManager.Domain.Entities;
using MagazynManager.Domain.Entities.Produkty;
using MagazynManager.Domain.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagazynManager.Tests.UnitTests.Fakes
{
    internal class InMemoryKategoriaRepository : ISlownikRepository<Kategoria>
    {
        private List<Kategoria> _kategorie;

        public InMemoryKategoriaRepository()
        {
            _kategorie = new List<Kategoria>();
        }

        public Task Delete(Kategoria entity)
        {
            _kategorie = _kategorie.Where(x => x.Id != entity.Id).ToList();

            return Task.CompletedTask;
        }

        public Task<List<Kategoria>> GetList(Specification<Kategoria> specification)
        {
            return Task.FromResult(_kategorie.Where(specification.ToExpression().Compile()).ToList());
        }

        public Task<Guid> Save(Kategoria kategoria)
        {
            _kategorie.Add(kategoria);
            return Task.FromResult(kategoria.Id);
        }
    }
}