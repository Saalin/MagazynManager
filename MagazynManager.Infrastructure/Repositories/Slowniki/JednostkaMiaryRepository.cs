using Dapper;
using MagazynManager.Domain.Entities;
using MagazynManager.Domain.Entities.Produkty;
using MagazynManager.Domain.Specification.Technical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagazynManager.Infrastructure.Repositories.Slowniki
{
    [Repository]
    public class JednostkaMiaryRepository : ISlownikRepository<JednostkaMiary>
    {
        private readonly IDbConnectionSource _dbConnectionSource;

        public JednostkaMiaryRepository(IDbConnectionSource dbConnectionSource)
        {
            _dbConnectionSource = dbConnectionSource;
        }

        public async Task<List<JednostkaMiary>> GetList(Specification<JednostkaMiary> specification)
        {
            var sql = "SELECT Id, Name as Nazwa, PrzedsiebiorstwoId FROM [dbo].[JednostkaMiary] WHERE " + specification.ToSql();

            using (var conn = _dbConnectionSource.GetConnection())
            {
                var parameters = new DynamicParameters();

                foreach (var dynamicParamsFunc in specification.GetDynamicParameters())
                {
                    dynamicParamsFunc(parameters);
                }

                var result = await conn.QueryAsync<JednostkaMiary>(sql, parameters);
                return result.ToList();
            }
        }

        public async Task<Guid> Save(JednostkaMiary jednostkaMiary)
        {
            var sqlInsert = "INSERT INTO dbo.JednostkaMiary (Id, Name, PrzedsiebiorstwoId) VALUES (@Id, @Nazwa, @PrzedsiebiorstwoId)";

            using (var conn = _dbConnectionSource.GetConnection())
            {
                await conn.ExecuteAsync(sqlInsert, jednostkaMiary);
                return jednostkaMiary.Id;
            }
        }

        public async Task Delete(JednostkaMiary entity)
        {
            var sql = "DELETE FROM [dbo].[JednostkaMiary] WHERE Id = @Id";

            using (var conn = _dbConnectionSource.GetConnection())
            {
                await conn.ExecuteAsync(sql, new { Id = entity.Id });
            }
        }
    }
}