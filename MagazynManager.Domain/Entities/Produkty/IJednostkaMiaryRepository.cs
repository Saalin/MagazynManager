using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MagazynManager.Domain.Entities.Produkty
{
    public interface IJednostkaMiaryRepository
    {
        Task<List<JednostkaMiary>> GetList(Guid przedsiebiorstwoId);

        Task<Guid> Save(JednostkaMiary jednostkaMiary);

        Task Delete(Guid id);
    }
}