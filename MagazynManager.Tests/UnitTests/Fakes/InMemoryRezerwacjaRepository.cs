using MagazynManager.Domain.Entities.Rezerwacje;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagazynManager.Tests.UnitTests
{
    public class InMemoryRezerwacjaRepository : IRezerwacjaRepository
    {
        private readonly List<Rezerwacja> _rezerwacje;

        public InMemoryRezerwacjaRepository()
        {
            _rezerwacje = new List<Rezerwacja>();
        }

        public Task Delete(Rezerwacja rezerwacja)
        {
            foreach (var r in _rezerwacje)
            {
                if (r.Id == rezerwacja.Id)
                {
                    _rezerwacje.Remove(r);
                    return Task.CompletedTask;
                }
            }

            return Task.CompletedTask;
        }

        public Task<List<Rezerwacja>> GetList(Guid przedsiebiorstwoId)
        {
            return Task.FromResult(_rezerwacje.Where(x => x.PrzedsiebiorstwoId == przedsiebiorstwoId).ToList());
        }

        public Task<Guid> Save(Rezerwacja rezerwacja)
        {
            _rezerwacje.Add(rezerwacja);

            return Task.FromResult(rezerwacja.Id);
        }
    }
}