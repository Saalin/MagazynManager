using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MagazynManager.Domain.Entities.Produkty
{
    public interface IKategoriaRepository
    {
        Task<List<Kategoria>> GetList(Guid przedsiebiorstwoId);

        Task Delete(Guid id);

        Task<Guid> Save(Kategoria kategoria);
    }
}