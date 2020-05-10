using Dapper;
using MagazynManager.Domain.Entities.Kontrahent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagazynManager.Infrastructure.Repositories.Slowniki
{
    [Repository]
    public class KontrahentRepository : IKontrahentRepository
    {
        private readonly IDbConnectionSource _dbConnectionSource;

        public KontrahentRepository(IDbConnectionSource dbConnectionSource)
        {
            _dbConnectionSource = dbConnectionSource;
        }

        public async Task Delete(Guid id)
        {
            var sql = "DELETE FROM Kontrahent where Id = @Id";

            using (var conn = _dbConnectionSource.GetConnection())
            {
                await conn.ExecuteAsync(sql, new { Id = id });
            }
        }

        public async Task<List<Kontrahent>> GetList(Guid przedsiebiorstwoId)
        {
            var sql = "select Id, Nip, Nazwa, Skrot, TypKontrahenta, " +
                "Ulica, Miejscowosc, KodPocztowy, PrzedsiebiorstwoId from Kontrahent WHERE PrzedsiebiorstwoId = @PrzedsiebiorstwoId";

            using (var conn = _dbConnectionSource.GetConnection())
            {
                var list = await conn.QueryAsync(sql, new { PrzedsiebiorstwoId = przedsiebiorstwoId });
                return list.Select(x => new Kontrahent
                {
                    Id = x.Id,
                    Nip = x.Nip,
                    Nazwa = x.Nazwa,
                    Skrot = x.Skrot,
                    TypKontrahenta = (TypKontrahenta)x.TypKontrahenta,
                    DaneAdresowe = new DaneAdresowe
                    {
                        Miejscowosc = x.Miejscowosc,
                        Ulica = x.Ulica,
                        KodPocztowy = x.KodPocztowy
                    }
                }).ToList();
            }
        }

        public async Task<Guid> Save(Kontrahent kontrahent)
        {
            var sql = "insert into Kontrahent (Id, Nip, Nazwa, Skrot, TypKontrahenta, Ulica, Miejscowosc, KodPocztowy, PrzedsiebiorstwoId) " +
                "VALUES  (@Id, @Nip, @Nazwa, @Skrot, @TypKontrahenta, @Ulica, @Miejscowosc, @KodPocztowy, @PrzedsiebiorstwoId)";

            using (var conn = _dbConnectionSource.GetConnection())
            {
                await conn.ExecuteAsync(sql, new
                {
                    Id = kontrahent.Id,
                    Nip = kontrahent.Nip,
                    Nazwa = kontrahent.Nazwa,
                    Skrot = kontrahent.Skrot,
                    TypKontrahenta = (int)kontrahent.TypKontrahenta,
                    Ulica = kontrahent.DaneAdresowe.Ulica,
                    Miejscowosc = kontrahent.DaneAdresowe.Miejscowosc,
                    KodPocztowy = kontrahent.DaneAdresowe.KodPocztowy,
                    PrzedsiebiorstwoId = kontrahent.PrzedsiebiorstwoId
                });

                return kontrahent.Id;
            }
        }
    }
}