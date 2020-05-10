using Dapper;
using MagazynManager.Domain.Entities.Dokumenty;
using MagazynManager.Domain.Entities.Slowniki;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagazynManager.Infrastructure.Repositories.Ewidencja
{
    [Repository]
    public class DokumentRepository : IDokumentRepository
    {
        private readonly IDbConnectionSource _dbConnectionSource;

        public DokumentRepository(IDbConnectionSource dbConnectionSource)
        {
            _dbConnectionSource = dbConnectionSource;
        }

        public async Task<List<Dokument>> GetList(TypDokumentu typ, Guid przedsiebiorstwoId)
        {
            var sql = "select d.Id as DokumentId, d.Numer, d.MagazynId, d.Data, d.KontrahentId, " +
                "d.TypDokumentu, PrzedsiebiorstwoId, " +
                "pd.Id as PozycjaId, pd.ProduktId, pd.StawkaVat, " +
                "pd.CenaNetto, pd.CenaBrutto, " +
                "pd.Ilosc, pd.WartoscNetto, pd.WartoscBrutto, pd.WartoscVat from Dokument d " +
                "inner join Magazyn M on d.MagazynId = M.Id " +
                "inner join PozycjaDokumentu PD on d.Id = PD.DokumentId where PrzedsiebiorstwoId = @PrzedsiebiorstwoId AND TypDokumentu = @TypDokumentu";

            using (var conn = _dbConnectionSource.GetConnection())
            {
                var result = await conn.QueryAsync(sql, new { PrzedsiebiorstwoId = przedsiebiorstwoId, TypDokumentu = typ });
                return result.GroupBy(x => x.DokumentId).Select(x => new Dokument
                {
                    Id = x.Key,
                    Data = x.First().Data,
                    MagazynId = x.First().MagazynId,
                    TypDokumentu = (TypDokumentu)x.First().TypDokumentu,
                    Numer = x.First().Numer,
                    KontrahentId = x.First().KontrahentId,
                    PozycjeDokumentu = x.Select(pd => new PozycjaDokumentu
                    {
                        Id = pd.PozycjaId,
                        ProduktId = pd.ProduktId,
                        CenaNetto = pd.CenaNetto,
                        CenaBrutto = pd.CenaBrutto,
                        Ilosc = pd.Ilosc,
                        WartoscNetto = pd.WartoscNetto,
                        WartoscVat = pd.WartoscVat,
                        WartoscBrutto = pd.WartoscBrutto,
                        StawkaVat = (StawkaVat)pd.StawkaVat
                    }).ToList()
                }).ToList();
            }
        }

        public async Task<Guid> Save(Dokument dokument)
        {
            var insertDokumentSql = "INSERT INTO Dokument (Id, Numer, MagazynId, Data, TypDokumentu, KontrahentId) " +
                "VALUES (@Id, @Numer, @MagazynId, @Data, @TypDokumentu, @KontrahentId)";
            var insertPozycjaSql = "insert into PozycjaDokumentu (Id, ProduktId, DokumentId, StawkaVat, CenaNetto, CenaBrutto, Ilosc, WartoscNetto, WartoscVat, WartoscBrutto) VALUES " +
                "(@Id, @ProduktId, @DokumentId, @StawkaVat, @CenaNetto, @CenaBrutto, @Ilosc, @WartoscNetto, @WartoscVat, @WartoscBrutto)";

            using (var conn = _dbConnectionSource.GetConnection())
            {
                await conn.ExecuteAsync(insertDokumentSql, dokument);
                await conn.ExecuteAsync(insertPozycjaSql, dokument.PozycjeDokumentu.Select(x => new
                {
                    Id = x.Id,
                    ProduktId = x.ProduktId,
                    DokumentId = dokument.Id,
                    StawkaVat = x.StawkaVat,
                    CenaNetto = x.CenaNetto,
                    CenaBrutto = x.CenaBrutto,
                    Ilosc = x.Ilosc,
                    WartoscNetto = x.WartoscNetto,
                    WartoscVat = x.WartoscVat,
                    WartoscBrutto = x.WartoscBrutto
                }));

                return dokument.Id;
            }
        }
    }
}