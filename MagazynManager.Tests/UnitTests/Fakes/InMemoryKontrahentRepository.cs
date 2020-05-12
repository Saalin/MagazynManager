using MagazynManager.Domain.Entities.Kontrahent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagazynManager.Tests.UnitTests
{
    public class InMemoryKontrahentRepository : IKontrahentRepository
    {
        private readonly List<Kontrahent> _kontrahenci;

        public InMemoryKontrahentRepository()
        {
            _kontrahenci = new List<Kontrahent>();
        }

        public Task Delete(Guid id)
        {
            foreach (var r in _kontrahenci)
            {
                if (r.Id == id)
                {
                    _kontrahenci.Remove(r);
                }
            }

            return Task.CompletedTask;
        }

        public Task<List<Kontrahent>> GetList(Guid przedsiebiorstwoId)
        {
            return Task.FromResult(_kontrahenci.Where(x => x.PrzedsiebiorstwoId == przedsiebiorstwoId).ToList());
        }

        public Task<Guid> Save(Kontrahent kontrahent)
        {
            _kontrahenci.Add(kontrahent);

            return Task.FromResult(kontrahent.Id);
        }
    }
}