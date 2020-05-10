using Dapper;
using MagazynManager.Domain.Entities.Uzytkownicy;
using MagazynManager.Domain.Specification;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MagazynManager.Infrastructure.Repositories
{
    [Repository]
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnectionSource _dbConnectionSource;

        public UserRepository(IDbConnectionSource dbConnectionSource)
        {
            _dbConnectionSource = dbConnectionSource;
        }

        public User GetUser(Specification<User> specification, Guid przedsiebiorstwoId)
        {
            var sql = "SELECT Id, Email, PasswordHash, Age, Salt FROM [dbo].[User] WHERE " + specification.ToSql();

            using (var conn = _dbConnectionSource.GetConnection())
            {
                var parameters = new DynamicParameters();

                foreach (var dynamicParamsFunc in specification.GetDynamicParameters())
                {
                    dynamicParamsFunc(parameters);
                }

                var user = conn.QuerySingleOrDefault(sql, parameters);

                if (user == null)
                {
                    return null;
                }

                var userClaims = GetClaims(conn, user.Id, user.Email, przedsiebiorstwoId);
                return new User(user.Id, user.Email, user.PasswordHash, user.Age, user.Salt, userClaims, przedsiebiorstwoId);
            }
        }

        public void RegisterWithPassword(User user, string password)
        {
            var sql = "INSERT INTO [dbo].[User] (Id, Email, Age, Salt, PasswordHash) " +
                "VALUES (@Id, @Email, @Age, @Salt, @PasswordHash)";

            using (var conn = _dbConnectionSource.GetConnection())
            {
                conn.Execute(sql, new
                {
                    user.Id,
                    user.Email,
                    PasswordHash = user.GetPasswordHash(password),
                    user.Salt,
                    user.Age
                });
            }
        }

        public async Task SetPermissions(Guid przedsiebiorstwoId, Guid userId, IEnumerable<KeyValuePair<string, string>> enumerable)
        {
            var deleteSql = "DELETE FROM UserClaims WHERE UserId = @UserId AND PrzedsiebiorstwoId = @PrzedsiebiorstwoId " +
                "AND ClaimValue LIKE 'Permission.%'";

            var insertSql = "insert into UserClaims (Id, UserId, PrzedsiebiorstwoId, Claim, ClaimValue) VALUES " +
                "(@Id, @UserId, @PrzedsiebiorstwoId, @Claim, @ClaimValue)";

            using (var conn = _dbConnectionSource.GetConnection())
            {
                await conn.ExecuteAsync(deleteSql, new
                {
                    UserId = userId,
                    PrzedsiebiorstwoId = przedsiebiorstwoId,
                });

                await conn.ExecuteAsync(insertSql, enumerable.Where(x => x.Key.StartsWith("Permission.")).Select(x => new
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    PrzedsiebiorstwoId = przedsiebiorstwoId,
                    Claim = x.Key,
                    ClaimValue = x.Value
                }));
            }
        }

        private IEnumerable<Claim> GetClaims(IDbConnection conn, Guid userId, string email, Guid przedsiebiorstwoId)
        {
            var sql = "SELECT Claim, ClaimValue FROM UserClaims WHERE UserId = @UserId AND PrzedsiebiorstwoId = @PrzedsiebiorstwoId";

            var claims = conn.Query(sql, new { UserId = userId, PrzedsiebiorstwoId = przedsiebiorstwoId });

            return (new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.NameId, userId.ToString()),
                new Claim("PrzedsiebiorstwoId", przedsiebiorstwoId.ToString())
            }).Concat(claims.Select(x => new Claim(x.Claim, x.ClaimValue)));
        }
    }
}