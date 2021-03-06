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
    public class ProduktRepository : ISlownikRepository<Produkt>
    {
        private readonly IDbConnectionSource _dbConnectionSource;

        public ProduktRepository(IDbConnectionSource dbConnectionSource)
        {
            _dbConnectionSource = dbConnectionSource;
        }

        public async Task<List<Produkt>> GetList(Specification<Produkt> specification)
        {
            var sql = @"SELECT P.Id as ProduktId,
                            p.ShortName as ProduktSkrot,
                            p.Name as ProduktNazwa,
                            p.MagazynId as MagazynId,
                            k.PrzedsiebiorstwoId as PrzedsiebiorstwoId,
                            k.Id as KategoriaId,
                            k.Name as KategoriaNazwa,
                            JM.Id as JednostkaMiaryId,
                            JM.Name as JednostkaMiaryNazwa
                            FROM Produkt P
                            INNER JOIN Magazyn M on P.MagazynId = M.Id
                            INNER JOIN JednostkaMiary JM on P.UnitId = JM.Id
                            LEFT JOIN Kategoria K on P.KategoriaId = K.Id";

            using (var conn = _dbConnectionSource.GetConnection())
            {
                var result = await conn.QueryAsync(sql);
                return result.Select(x => new Produkt
                {
                    Id = x.ProduktId,
                    Nazwa = x.ProduktNazwa,
                    Skrot = x.ProduktSkrot,
                    Kategoria = new Kategoria(x.KategoriaId, x.KategoriaNazwa, x.PrzedsiebiorstwoId),
                    JednostkaMiary = new JednostkaMiary(x.JednostkaMiaryId, x.JednostkaMiaryNazwa, x.PrzedsiebiorstwoId),
                    MagazynId = x.MagazynId
                }).Where(specification.ToExpression().Compile()).ToList();
            }
        }

        public async Task Delete(Produkt entity)
        {
            var sql = "DELETE FROM [dbo].[Produkt] WHERE Id = @Id";

            using (var conn = _dbConnectionSource.GetConnection())
            {
                await conn.ExecuteAsync(sql, new { Id = entity.Id });
            }
        }

        public async Task<Guid> Save(Produkt entity)
        {
            var sqlInsert = "INSERT INTO dbo.Produkt (Id, ShortName, Name, KategoriaId, UnitId, MagazynId) VALUES (@Id, @ShortName, @Name, @KategoriaId, @UnitId, @MagazynId)";

            using (var conn = _dbConnectionSource.GetConnection())
            {
                await conn.ExecuteAsync(sqlInsert,
                    new { Id = entity.Id, ShortName = entity.Skrot, Name = entity.Nazwa, KategoriaId = entity.Kategoria.Id, UnitId = entity.JednostkaMiary.Id, MagazynId = entity.MagazynId });
                return entity.Id;
            }
        }
    }
}