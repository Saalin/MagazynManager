using Dapper;
using MagazynManager.Domain.Entities;
using MagazynManager.Domain.Entities.StukturaOrganizacyjna;
using MagazynManager.Domain.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagazynManager.Infrastructure.Repositories.Slowniki
{
    [Repository]
    public class MagazynRepository : ISlownikRepository<Magazyn>
    {
        private readonly IDbConnectionSource _dbConnectionSource;

        public MagazynRepository(IDbConnectionSource dbConnectionSource)
        {
            _dbConnectionSource = dbConnectionSource;
        }
        public async Task<List<Magazyn>> GetList(Specification<Magazyn> specification)
        {
            var sql = "SELECT Id, Skrot, Nazwa, PrzedsiebiorstwoId FROM [dbo].[Magazyn] WHERE " + specification.ToSql();

            using (var conn = _dbConnectionSource.GetConnection())
            {
                var parameters = new DynamicParameters();

                foreach (var dynamicParamsFunc in specification.GetDynamicParameters())
                {
                    dynamicParamsFunc(parameters);
                }

                var result = await conn.QueryAsync(sql, parameters);
                return result.Select(x => new Magazyn
                {
                    Id = x.Id,
                    Nazwa = x.Nazwa,
                    Skrot = x.Skrot,
                    PrzedsiebiorstwoId = x.PrzedsiebiorstwoId
                }).ToList();
            }
        }

        public async Task<Guid> Save(Magazyn magazyn)
        {
            var sqlInsert = "INSERT INTO dbo.Magazyn (Id, Skrot, Nazwa, PrzedsiebiorstwoId) VALUES (@Id, @Skrot, @Nazwa, @PrzedsiebiorstwoId)";

            using (var conn = _dbConnectionSource.GetConnection())
            {
                await conn.ExecuteAsync(sqlInsert, magazyn);
                return magazyn.Id;
            }
        }

        public async Task Delete(Magazyn entity)
        {
            var sql = "DELETE FROM [dbo].[Magazyn] WHERE Id = @Id";

            using (var conn = _dbConnectionSource.GetConnection())
            {
                await conn.ExecuteAsync(sql, new { Id = entity.Id });
            }
        }
    }
}