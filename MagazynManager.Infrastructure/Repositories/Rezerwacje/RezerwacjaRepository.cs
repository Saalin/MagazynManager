using Dapper;
using MagazynManager.Domain.Entities.Rezerwacje;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagazynManager.Infrastructure.Repositories.Rezerwacje
{
    [Repository]
    public class RezerwacjaRepository : IRezerwacjaRepository
    {
        private readonly IDbConnectionSource _dbConnectionSource;

        public RezerwacjaRepository(IDbConnectionSource dbConnectionSource)
        {
            _dbConnectionSource = dbConnectionSource;
        }

        public async Task Delete(Rezerwacja rezerwacja)
        {
            var deleteRezerwacja = "DELETE FROM Rezerwacja WHERE Id = @Id";
            var deletePozycje = "DELETE FROM PozycjaRezerwacji WHERE RezerwacjaId = @Id";

            using (var conn = _dbConnectionSource.GetConnection())
            {
                using (var tx = conn.BeginTransaction())
                {
                    await conn.ExecuteAsync(deletePozycje, new { Id = rezerwacja.Id }, tx);
                    await conn.ExecuteAsync(deleteRezerwacja, new { Id = rezerwacja.Id }, tx);

                    tx.Commit();
                }
            }
        }

        public async Task<List<Rezerwacja>> GetList(Guid przedsiebiorstwoId)
        {
            var sql = "select R.Id as RezerwacjaId, PrzedsiebiorstwoId, " +
                "UzytkownikRezerwujacyId, DataWaznosci, DataRezerwacji, Opis, DokumentWydaniaId, PR.Id as PozycjaId, PR.ProduktId, PR.Ilosc " +
                "FROM Rezerwacja R INNER JOIN PozycjaRezerwacji PR on R.Id = PR.RezerwacjaId " +
                "WHERE PrzedsiebiorstwoId = @PrzedsiebiorstwoId";

            using (var conn = _dbConnectionSource.GetConnection())
            {
                var list = await conn.QueryAsync(sql, new { PrzedsiebiorstwoId = przedsiebiorstwoId });

                return list.GroupBy(x => x.RezerwacjaId).Select(x =>
                {
                    var r = new Rezerwacja(x.Key)
                    {
                        PrzedsiebiorstwoId = x.First().PrzedsiebiorstwoId,
                        UzytkownikRezerwujacyId = x.First().UzytkownikRezerwujacyId,
                        DataWaznosci = x.First().DataWaznosci,
                        DataRezerwacji = x.First().DataRezerwacji,
                        Opis = x.First().Opis,
                        DokumentWydaniaId = x.First().DokumentWydaniaId
                    };
                    foreach (var pozycja in x)
                    {
                        r.DodajPozycjeRezerwacji(new PozycjaRezerwacji(pozycja.PozycjaId, pozycja.ProduktId, pozycja.Ilosc));
                    }
                    return r;
                }).ToList();
            }
        }

        public async Task<Guid> Save(Rezerwacja rezerwacja)
        {
            var insertDokumentSql = "INSERT INTO Rezerwacja (Id, UzytkownikRezerwujacyId, DataRezerwacji, DataWaznosci, Opis, DokumentWydaniaId, PrzedsiebiorstwoId)  " +
                "VALUES (@Id, @UzytkownikRezerwujacyId, @DataRezerwacji, @DataWaznosci, @Opis, @DokumentWydaniaId, @PrzedsiebiorstwoId)";
            var insertPozycjaSql = "INSERT INTO PozycjaRezerwacji (Id, RezerwacjaId, ProduktId, Ilosc) " +
                "VALUES (@Id, @RezerwacjaId, @ProduktId, @Ilosc)";

            using (var conn = _dbConnectionSource.GetConnection())
            {
                await conn.ExecuteAsync(insertDokumentSql, rezerwacja);
                await conn.ExecuteAsync(insertPozycjaSql, rezerwacja.PozycjeRezerwacji.Select(x => new
                {
                    Id = x.Id,
                    RezerwacjaId = rezerwacja.Id,
                    ProduktId = x.ProduktId,
                    Ilosc = x.Ilosc
                }));

                return rezerwacja.Id;
            }
        }
    }
}