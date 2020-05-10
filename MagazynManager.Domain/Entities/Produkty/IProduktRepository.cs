using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MagazynManager.Domain.Entities.Produkty
{
    public interface IProduktRepository
    {
        Task<List<Produkt>> GetList(Guid przedsiebiorstwoId);

        Task<Guid> Save(Produkt produkt);

        Task Delete(Guid id);
    }
}