using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MagazynManager.Domain.Entities.Kontrahent
{
    public interface IKontrahentRepository
    {
        Task<List<Kontrahent>> GetList(Guid przedsiebiorstwoId);

        Task<Guid> Save(Kontrahent kontrahent);

        Task Delete(Guid id);
    }
}