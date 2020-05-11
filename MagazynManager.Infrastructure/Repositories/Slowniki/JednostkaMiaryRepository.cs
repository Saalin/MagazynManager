using Dapper;
using MagazynManager.Domain.Entities.Produkty;
using MagazynManager.Domain.Specification;
using MagazynManager.Infrastructure.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagazynManager.Infrastructure.Repositories.Slowniki
{
    [Repository]
    public class JednostkaMiaryRepository : IJednostkaMiaryRepository
    {
        private readonly IDbConnectionSource _dbConnectionSource;

        public JednostkaMiaryRepository(IDbConnectionSource dbConnectionSource)
        {
            _dbConnectionSource = dbConnectionSource;
        }

        public Task<List<JednostkaMiary>> GetList(Guid przedsiebiorstwoId)
        {
            return GetKategorieListAsync(new PrzedsiebiorstwoSpecification<JednostkaMiary>(przedsiebiorstwoId));
        }

        private async Task<List<JednostkaMiary>> GetKategorieListAsync(Specification<JednostkaMiary> specification)
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

        public async Task Delete(Guid id)
        {
            var sql = "DELETE FROM [dbo].[JednostkaMiary] WHERE Id = @Id";

            using (var conn = _dbConnectionSource.GetConnection())
            {
                await conn.ExecuteAsync(sql, new { Id = id });
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
    }
}