using MagazynManager.Domain.Entities.StukturaOrganizacyjna;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagazynManager.Tests.UnitTests
{
    public class InMemoryMagazynRepository : IMagazynRepository
    {
        private readonly List<Magazyn> _magazyny;

        public InMemoryMagazynRepository()
        {
            _magazyny = new List<Magazyn>();
        }

        public Task Delete(Guid id)
        {
            foreach(var m in _magazyny)
            {
                if(m.Id == id)
                {
                    _magazyny.Remove(m);
                }
            }

            return Task.CompletedTask;
        }

        public Task<List<Magazyn>> GetList(Guid przedsiebiorstwoId)
        {
            return Task.FromResult(_magazyny.Where(x => x.PrzedsiebiorstwoId == przedsiebiorstwoId).ToList());
        }

        public Task<Guid> Save(Magazyn magazyn)
        {
            _magazyny.Add(magazyn);
            return Task.FromResult(magazyn.Id);
        }
    }
}