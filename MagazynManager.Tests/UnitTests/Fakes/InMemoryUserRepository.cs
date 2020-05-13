using MagazynManager.Domain.Entities.Uzytkownicy;
using MagazynManager.Domain.Specification.Technical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MagazynManager.Tests.UnitTests
{
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly List<User> _users;

        public InMemoryUserRepository()
        {
            var przedsiebiorstwoId = Guid.Parse("cf5cbb85-dcb0-470d-b8b9-9a29a097a73d");

            var claimValue = "lrcuda";
            var standardClaims = new List<Claim>
            {
                new Claim("Permission.Ewidencjonowanie", claimValue),
                new Claim("Permission.Slowniki", claimValue),
                new Claim("Permission.Rezerwacje", claimValue),
                new Claim("PrzedsiebiorstwoId", przedsiebiorstwoId.ToString()),
            };

            _users = new List<User>
            {
                new User(Guid.Parse("3A7C2E38-385E-4C6A-8BFA-1767A3EBCCCC"), "admin@admin.com", "32548419AC44CF75C9214238F72F72132AE24CE8", 42, "858bfd79-da59-4756-897f-2aa5eb6029da",
                standardClaims.Concat(new List<Claim> { new Claim("Permission.Administracja", claimValue), new Claim(ClaimTypes.NameIdentifier, "3A7C2E38-385E-4C6A-8BFA-1767A3EBCCCC") }).ToList(), przedsiebiorstwoId),
                new User(Guid.Parse("A13A3CB3-928B-4C09-9FD6-739BCE18EDF0"), "demo@demo.com", "423AEC62E306E2EC1D0146C2CD7F44734437E1B3", 18, "3acd21a8-62a8-4829-a144-6c5b30d9b782",
                standardClaims.Concat(new List<Claim> { new Claim(ClaimTypes.NameIdentifier, "A13A3CB3-928B-4C09-9FD6-739BCE18EDF0") }).ToList(), przedsiebiorstwoId)
            };
        }

        public User GetUser(Specification<User> specification, Guid przedsiebiorstwoId)
        {
            return _users.AsQueryable().Where(specification.ToExpression()).SingleOrDefault(x => x.PrzedsiebiorstwoId == przedsiebiorstwoId);
        }

        public void RegisterWithPassword(User user, string password)
        {
            _users.Add(user);
        }

        public Task SetPermissions(Guid przedsiebiorstwoId, Guid userId, IEnumerable<KeyValuePair<string, string>> enumerable)
        {
            var user = _users.Where(x => x.Id == userId).SingleOrDefault(x => x.PrzedsiebiorstwoId == przedsiebiorstwoId);

            user.Claims = enumerable.Select(x => new Claim(x.Key, x.Value));

            return Task.CompletedTask;
        }
    }
}