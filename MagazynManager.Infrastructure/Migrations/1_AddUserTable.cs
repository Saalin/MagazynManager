using FluentMigrator;
using FluentMigrator.SqlServer;
using MagazynManager.Domain.Entities.Uzytkownicy;
using System;

namespace MagazynManager.Infrastructure.Migrations
{
    [Migration(1)]
    public class AddUserTable : Migration
    {
        public override void Up()
        {
            Create.Table("User")
                .WithColumn("Id").AsGuid()
                .WithColumn("Email").AsString(255).NotNullable().Unique("UQ_User_Email")
                .WithColumn("PasswordHash").AsString(4000).NotNullable()
                .WithColumn("Age").AsInt32().Nullable()
                .WithColumn("Salt").AsString(255).NotNullable();

            Create.PrimaryKey("PK_User").OnTable("User").Column("Id").NonClustered();

            var user = User.RegisterUser("demo@demo.com", 18);
            var userId = Guid.Parse("a13a3cb3-928b-4c09-9fd6-739bce18edf0");
            var hash = user.GetPasswordHash("demo");
            Execute.Sql("INSERT INTO [dbo].[User] (Id, Email, PasswordHash, Salt, Age) VALUES ('" +
                $"{userId}', '{user.Email}', '{hash}', '{user.Salt}', {user.Age});");

            var admin = User.RegisterUser("admin@admin.com", 42);
            var hashAdmin = admin.GetPasswordHash("admin");
            var adminId = Guid.Parse("3a7c2e38-385e-4c6a-8bfa-1767a3ebcccc");
            Execute.Sql("INSERT INTO [dbo].[User] (Id, Email, PasswordHash, Salt, Age) VALUES ('" +
                $"{adminId}', '{admin.Email}', '{hashAdmin}', '{admin.Salt}', {admin.Age});");
        }

        public override void Down()
        {
            Delete.Table("User");
        }
    }
}