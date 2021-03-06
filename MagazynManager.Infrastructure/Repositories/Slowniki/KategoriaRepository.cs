﻿using Dapper;
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
    public class KategoriaRepository : ISlownikRepository<Kategoria>
    {
        private readonly IDbConnectionSource _dbConnectionSource;

        public KategoriaRepository(IDbConnectionSource dbConnectionSource)
        {
            _dbConnectionSource = dbConnectionSource;
        }

        public async Task<List<Kategoria>> GetList(Specification<Kategoria> specification)
        {
            var sql = "SELECT Id, Name, PrzedsiebiorstwoId FROM [dbo].[Kategoria] WHERE " + specification.ToSql();

            using (var conn = _dbConnectionSource.GetConnection())
            {
                var parameters = new DynamicParameters();

                foreach (var dynamicParamsFunc in specification.GetDynamicParameters())
                {
                    dynamicParamsFunc(parameters);
                }

                var result = await conn.QueryAsync(sql, parameters);
                return result.Select(x => new Kategoria(x.Id, x.Name, x.PrzedsiebiorstwoId)).ToList();
            }
        }

        public async Task Delete(Kategoria kategoria)
        {
            var sql = "DELETE FROM [dbo].[Kategoria] WHERE Id = @Id";

            using (var conn = _dbConnectionSource.GetConnection())
            {
                await conn.ExecuteAsync(sql, new { Id = kategoria.Id });
            }
        }

        public async Task<Guid> Save(Kategoria kategoria)
        {
            var sqlInsert = "INSERT INTO dbo.Kategoria (Id, Name, PrzedsiebiorstwoId) VALUES (@Id, @Nazwa, @PrzedsiebiorstwoId)";

            using (var conn = _dbConnectionSource.GetConnection())
            {
                await conn.ExecuteAsync(sqlInsert, kategoria);
                return kategoria.Id;
            }
        }
    }
}