using MagazynManager.Domain.Specification.Technical;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MagazynManager.Domain.Entities.Uzytkownicy
{
    public interface IUserRepository
    {
        User GetUser(Specification<User> specification, Guid przedsiebiorstwoId);

        void RegisterWithPassword(User user, string password);

        Task SetPermissions(Guid przedsiebiorstwoId, Guid userId, IEnumerable<KeyValuePair<string, string>> enumerable);
    }
}