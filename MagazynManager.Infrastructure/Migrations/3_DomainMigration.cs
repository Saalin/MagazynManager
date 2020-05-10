using FluentMigrator;
using System.Data;

namespace MagazynManager.Infrastructure.Migrations
{
    [Migration(3)]
    public class DomainMigration : Migration
    {
        public override void Up()
        {
            Create.Table("Kontrahent")
                .WithColumn("Id").AsGuid()
                .WithColumn("Nip").AsString(14).NotNullable()
                .WithColumn("Nazwa").AsString(255).NotNullable()
                .WithColumn("Skrot").AsString(255).NotNullable()
                .WithColumn("TypKontrahenta").AsInt32().NotNullable()
                .WithColumn("Ulica").AsString(255).Nullable()
                .WithColumn("Miejscowosc").AsString(255).Nullable()
                .WithColumn("KodPocztowy").AsString(6).Nullable()
                .WithColumn("PrzedsiebiorstwoId").AsGuid().NotNullable();

            Create.PrimaryKey("PK_Kontrahent").OnTable("Kontrahent").Column("Id");

            Create.ForeignKey("FK_Kontrahent_Przedsiebiorstwo")
                .FromTable("Kontrahent").ForeignColumn("PrzedsiebiorstwoId")
                .ToTable("Przedsiebiorstwo").PrimaryColumn("Id");

            Create.UniqueConstraint("UQ_Kontrahent_Przedsiebiorstwo1").OnTable("Kontrahent")
                .Columns(new[] { "Nazwa", "PrzedsiebiorstwoId" });
            Create.UniqueConstraint("UQ_Kontrahent_Przedsiebiorstwo2").OnTable("Kontrahent")
                .Columns(new[] { "Skrot", "PrzedsiebiorstwoId" });

            Create.Table("Dokument")
                .WithColumn("Id").AsGuid()
                .WithColumn("Numer").AsString(255).NotNullable()
                .WithColumn("KontrahentId").AsGuid().Nullable()
                .WithColumn("MagazynId").AsGuid().NotNullable()
                .WithColumn("Data").AsDateTime2().NotNullable()
                .WithColumn("TypDokumentu").AsInt32().NotNullable();

            Create.PrimaryKey("PK_Dokument").OnTable("Dokument").Column("Id");

            Create.UniqueConstraint("UQ_Dokument_Numer")
                .OnTable("Dokument").Columns(new[] { "Numer", "MagazynId" });

            Create.ForeignKey("FK_Dokument_Kontrahent")
                .FromTable("Dokument").ForeignColumn("KontrahentId")
                .ToTable("Kontrahent").PrimaryColumn("Id");

            Create.ForeignKey("FK_Dokument_Magazyn")
                .FromTable("Dokument").ForeignColumn("MagazynId")
                .ToTable("Magazyn").PrimaryColumn("Id");

            Create.Table("PozycjaDokumentu")
                .WithColumn("Id").AsGuid()
                .WithColumn("ProduktId").AsGuid().NotNullable()
                .WithColumn("DokumentId").AsGuid().NotNullable()
                .WithColumn("StawkaVat").AsInt32().NotNullable()
                .WithColumn("CenaNetto").AsDecimal(10, 2).NotNullable()
                .WithColumn("CenaBrutto").AsDecimal(10, 2).NotNullable()
                .WithColumn("Ilosc").AsDecimal(10, 4).NotNullable()
                .WithColumn("WartoscNetto").AsDecimal(10, 2).NotNullable()
                .WithColumn("WartoscVat").AsDecimal(10, 2).NotNullable()
                .WithColumn("WartoscBrutto").AsDecimal(10, 2).NotNullable();

            Create.PrimaryKey("PK_PozycjaDokumentu").OnTable("PozycjaDokumentu").Column("Id");

            Create.ForeignKey("FK_PozycjaDokumentu_Produkt")
                .FromTable("PozycjaDokumentu").ForeignColumn("ProduktId")
                .ToTable("Produkt").PrimaryColumn("Id");

            Create.ForeignKey("FK_PozycjaDokumentu_Dokument")
                .FromTable("PozycjaDokumentu").ForeignColumn("DokumentId")
                .ToTable("Dokument").PrimaryColumn("Id");

            Create.Table("Rezerwacja")
                .WithColumn("Id").AsGuid()
                .WithColumn("UzytkownikRezerwujacyId").AsGuid().NotNullable()
                .WithColumn("DataRezerwacji").AsDate().NotNullable()
                .WithColumn("DataWaznosci").AsDate().NotNullable()
                .WithColumn("Opis").AsString(1024).NotNullable()
                .WithColumn("DokumentWydaniaId").AsGuid().Nullable()
                .WithColumn("PrzedsiebiorstwoId").AsGuid().NotNullable();

            Create.PrimaryKey("PK_Rezerwacja").OnTable("Rezerwacja").Column("Id");

            Create.ForeignKey("FK_Rezerwacja_UzytkownikRezerwujacy")
                .FromTable("Rezerwacja").ForeignColumn("UzytkownikRezerwujacyId")
                .ToTable("User").PrimaryColumn("Id");

            Create.ForeignKey("FK_Rezerwacja_Przedsiebiorstwo")
                .FromTable("Rezerwacja").ForeignColumn("PrzedsiebiorstwoId")
                .ToTable("Przedsiebiorstwo").PrimaryColumn("Id");

            Create.ForeignKey("FK_Rezerwacja_DokumentWydania")
                 .FromTable("Rezerwacja").ForeignColumn("DokumentWydaniaId")
                 .ToTable("Dokument").PrimaryColumn("Id");

            Create.Table("PozycjaRezerwacji")
                .WithColumn("Id").AsGuid()
                .WithColumn("RezerwacjaId").AsGuid().NotNullable()
                .WithColumn("ProduktId").AsGuid().NotNullable()
                .WithColumn("Ilosc").AsDecimal(10, 4).NotNullable();

            Create.PrimaryKey("PK_PozycjaRezerwacji").OnTable("PozycjaRezerwacji").Column("Id");
            Create.UniqueConstraint("UQ_PozycjaRezerwacji_ProduktId").OnTable("PozycjaRezerwacji")
                .Columns(new[] { "RezerwacjaId", "ProduktId" });

            Create.ForeignKey("FK_PozycjaRezerwacji_Produkt")
                .FromTable("PozycjaRezerwacji").ForeignColumn("ProduktId")
                .ToTable("Produkt").PrimaryColumn("Id");

            Create.ForeignKey("UQ_PozycjaRezerwacji_Rezerwacja")
                .FromTable("PozycjaRezerwacji").ForeignColumn("RezerwacjaId")
                .ToTable("Rezerwacja").PrimaryColumn("Id")
                .OnDeleteOrUpdate(Rule.Cascade);
        }

        public override void Down()
        {
            Delete.Table("Dokument");
            Delete.Table("PozycjaDokumentu");
            Delete.Table("Kontrahent");
            Delete.Table("Rezerwacja");
            Delete.Table("PozycjaRezerwacji");
        }
    }
}