using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MagazynManager.Domain.Entities.StukturaOrganizacyjna
{
    public interface IMagazynRepository
    {
        Task<List<Magazyn>> GetList(Guid przedsiebiorstwoId);

        Task<Guid> Save(Magazyn magazyn);

        Task Delete(Guid id);
    }
}