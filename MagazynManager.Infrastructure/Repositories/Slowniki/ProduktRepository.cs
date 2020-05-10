using Dapper;
using MagazynManager.Domain.Entities.Produkty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagazynManager.Infrastructure.Repositories.Slowniki
{
    [Repository]
    public class ProduktRepository : IProduktRepository
    {
        private readonly IDbConnectionSource _dbConnectionSource;

        public ProduktRepository(IDbConnectionSource dbConnectionSource)
        {
            _dbConnectionSource = dbConnectionSource;
        }

        public async Task<List<Produkt>> GetList(Guid przedsiebiorstwoId)
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
                            LEFT JOIN Kategoria K on P.KategoriaId = K.Id
                            WHERE K.PrzedsiebiorstwoId = @PrzedsiebiorstwoId";

            using (var conn = _dbConnectionSource.GetConnection())
            {
                var result = await conn.QueryAsync(sql, new { PrzedsiebiorstwoId = przedsiebiorstwoId });
                return result.Select(x => new Produkt
                {
                    Id = x.ProduktId,
                    Nazwa = x.ProduktNazwa,
                    Skrot = x.ProduktSkrot,
                    Kategoria = new Kategoria(x.KategoriaId, x.KategoriaNazwa, x.PrzedsiebiorstwoId),
                    JednostkaMiary = new JednostkaMiary(x.JednostkaMiaryId, x.JednostkaMiaryNazwa, x.PrzedsiebiorstwoId),
                    MagazynId = x.MagazynId
                }).ToList();
            }
        }

        public async Task Delete(Guid id)
        {
            var sql = "DELETE FROM [dbo].[Produkt] WHERE Id = @Id";

            using (var conn = _dbConnectionSource.GetConnection())
            {
                await conn.ExecuteAsync(sql, new { Id = id });
            }
        }

        public async Task<Guid> Save(Produkt produkt)
        {
            var sqlInsert = "INSERT INTO dbo.Produkt (Id, ShortName, Name, KategoriaId, UnitId, MagazynId) VALUES (@Id, @ShortName, @Name, @KategoriaId, @UnitId, @MagazynId)";

            using (var conn = _dbConnectionSource.GetConnection())
            {
                await conn.ExecuteAsync(sqlInsert,
                    new { Id = produkt.Id, ShortName = produkt.Skrot, Name = produkt.Nazwa, KategoriaId = produkt.Kategoria.Id, UnitId = produkt.JednostkaMiary.Id, MagazynId = produkt.MagazynId });
                return produkt.Id;
            }
        }
    }
}