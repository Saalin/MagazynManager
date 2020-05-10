using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MagazynManager.Domain.Entities.Rezerwacje
{
    public interface IRezerwacjaRepository
    {
        Task<List<Rezerwacja>> GetList(Guid przedsiebiorstwoId);

        Task<Guid> Save(Rezerwacja rezerwacja);

        Task Delete(Rezerwacja rezerwacja);
    }
}