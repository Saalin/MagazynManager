using MagazynManager.Domain.Specification.Technical;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MagazynManager.Domain.Entities
{
    public interface ISlownikRepository<T>
    {
        Task<List<T>> GetList(Specification<T> specification);

        Task Delete(T entity);

        Task<Guid> Save(T entity);
    }
}