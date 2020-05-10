using FluentMigrator;
using FluentMigrator.SqlServer;
using System;
using System.Collections.Generic;

namespace MagazynManager.Infrastructure.Migrations
{
    [Migration(2)]
    public class PrzedsiebiorstwoMigration : Migration
    {
        private Guid userId = Guid.Parse("a13a3cb3-928b-4c09-9fd6-739bce18edf0");
        private Guid adminId = Guid.Parse("3a7c2e38-385e-4c6a-8bfa-1767a3ebcccc");
        private Guid przedsiebiorstwoId = Guid.Parse("cf5cbb85-dcb0-470d-b8b9-9a29a097a73d");

        public override void Up()
        {
            Create.Table("Przedsiebiorstwo")
                .WithColumn("Id").AsGuid()
                .WithColumn("Nazwa").AsString(255).NotNullable().Unique("UQ_Przedsiebiorstwo_Nazwa");

            Create.PrimaryKey("PK_Przedsiebiorstwo").OnTable("Przedsiebiorstwo").Column("Id").NonClustered();

            Insert.IntoTable("Przedsiebiorstwo").Row(new { Id = przedsiebiorstwoId, Nazwa = "Overcity" });

            Create.Table("Magazyn")
                .WithColumn("Id").AsGuid()
                .WithColumn("Skrot").AsString(255).NotNullable()
                .WithColumn("Nazwa").AsString(255).NotNullable()
                .WithColumn("PrzedsiebiorstwoId").AsGuid().NotNullable();

            Create.PrimaryKey("PK_Magazyn").OnTable("Magazyn").Column("Id").NonClustered();

            Create.UniqueConstraint("UQ_Magazyn_Skrot").OnTable("Magazyn").Columns(new[] { "Skrot", "PrzedsiebiorstwoId" });
            Create.UniqueConstraint("UQ_Magazyn_Nazwa").OnTable("Magazyn").Columns(new[] { "Nazwa", "PrzedsiebiorstwoId" });

            Create.ForeignKey("FK_Magazyn_Przedsiebiorstwo")
                .FromTable("Magazyn").ForeignColumn("PrzedsiebiorstwoId")
                .ToTable("Przedsiebiorstwo").PrimaryColumn("Id");

            Create.Table("UserClaims")
                .WithColumn("Id").AsGuid()
                .WithColumn("UserId").AsGuid().NotNullable()
                .WithColumn("PrzedsiebiorstwoId").AsGuid().NotNullable()
                .WithColumn("Claim").AsString(255).NotNullable()
                .WithColumn("ClaimValue").AsString(32).NotNullable();

            Create.PrimaryKey("PK_UserClaims").OnTable("UserClaims").Column("Id").NonClustered();

            Create.UniqueConstraint("UQ_UserClaims_User_Claim").OnTable("UserClaims").Columns(new[] { "UserId", "Claim" });

            Create.ForeignKey("FK_UserClaims_Przedsiebiorstwo")
                .FromTable("UserClaims").ForeignColumn("PrzedsiebiorstwoId")
                .ToTable("Przedsiebiorstwo").PrimaryColumn("Id");

            Create.ForeignKey("FK_UserClaims_User")
                .FromTable("UserClaims").ForeignColumn("UserId")
                .ToTable("User").PrimaryColumn("Id");

            AddUserClaims();
            AddAdminClaims();

            Create.Table("Kategoria")
                .WithColumn("Id").AsGuid()
                .WithColumn("Name").AsString(255).NotNullable()
                .WithColumn("PrzedsiebiorstwoId").AsGuid().NotNullable();

            Create.PrimaryKey("PK_Kategoria").OnTable("Kategoria").Column("Id").NonClustered();

            Create.ForeignKey("FK_Kategoria_Przedsiebiorstwo")
                .FromTable("Kategoria").ForeignColumn("PrzedsiebiorstwoId")
                .ToTable("Przedsiebiorstwo").PrimaryColumn("Id");

            Create.UniqueConstraint("UQ_Kategoria_Name").OnTable("Kategoria").Columns(new[] { "Name", "PrzedsiebiorstwoId" });

            Create.Table("JednostkaMiary")
                .WithColumn("Id").AsGuid()
                .WithColumn("Name").AsString(255).NotNullable()
                .WithColumn("PrzedsiebiorstwoId").AsGuid().NotNullable();

            Create.PrimaryKey("PK_JednostkaMiary").OnTable("JednostkaMiary").Column("Id").NonClustered();

            Create.UniqueConstraint("UQ_JednostkaMiarya_Name").OnTable("JednostkaMiary").Columns(new[] { "Name", "PrzedsiebiorstwoId" });

            Create.ForeignKey("FK_JednostkaMiary_Przedsiebiorstwo")
                .FromTable("JednostkaMiary").ForeignColumn("PrzedsiebiorstwoId")
                .ToTable("Przedsiebiorstwo").PrimaryColumn("Id");

            Create.Table("Produkt")
                .WithColumn("Id").AsGuid()
                .WithColumn("ShortName").AsString(255).NotNullable()
                .WithColumn("Name").AsString(1024).NotNullable()
                .WithColumn("KategoriaId").AsGuid().Nullable()
                .WithColumn("UnitId").AsGuid().NotNullable()
                .WithColumn("MagazynId").AsGuid().NotNullable();

            Create.PrimaryKey("PK_Produkt").OnTable("Produkt").Column("Id").NonClustered();

            Create.UniqueConstraint("UQ_Produkt_Name").OnTable("Produkt").Columns(new[] { "Name", "MagazynId" });
            Create.UniqueConstraint("UQ_Produkt_ShortName").OnTable("Produkt").Columns(new[] { "ShortName", "MagazynId" });

            Create.ForeignKey("FK_Produkt_KategoriaId_Kategoria_Id")
                .FromTable("Produkt").ForeignColumn("KategoriaId")
                .ToTable("Kategoria").PrimaryColumn("Id");

            Create.ForeignKey("FK_Produkt_UnitId_JednostkaMiary_Id")
                .FromTable("Produkt").ForeignColumn("UnitId")
                .ToTable("JednostkaMiary").PrimaryColumn("Id");

            Create.ForeignKey("FK_Produkt_UnitId_MagazynId")
                .FromTable("Produkt").ForeignColumn("MagazynId")
                .ToTable("Magazyn").PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.Table("Przedsiebiorstwo");
            Delete.Table("Magazyn");
            Delete.Table("UserClaims");

            Delete.Table("Produkt");
            Delete.Table("Kategoria");
            Delete.Table("JednostkaMiary");
        }

        private void AddUserClaims()
        {
            var claims = new[]
            {
                ("Permission.Ewidencjonowanie", "lrcuda"),
                ("Permission.Slowniki", "lrcuda"),
                ("Permission.Rezerwacje", "lrcuda")
            };

            AddClaims(userId, claims);
        }

        private void AddAdminClaims()
        {
            var claims = new[]
            {
                ("Permission.Administracja", "lrcuda"),
                ("Permission.Ewidencjonowanie", "lrcuda"),
                ("Permission.Slowniki", "lrcuda"),
                ("Permission.Rezerwacje", "lrcuda")
            };

            AddClaims(adminId, claims);
        }

        private void AddClaims(Guid id, IEnumerable<(string, string)> claims)
        {
            foreach (var c in claims)
            {
                Insert.IntoTable("UserClaims").Row(new
                {
                    Id = Guid.NewGuid(),
                    UserId = id,
                    PrzedsiebiorstwoId = przedsiebiorstwoId,
                    Claim = c.Item1,
                    ClaimValue = c.Item2
                });
            }
        }
    }
}