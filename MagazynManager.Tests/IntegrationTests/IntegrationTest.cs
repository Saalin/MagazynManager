using Bogus;
using Dapper;
using MagazynManager.Infrastructure;
using MagazynManager.Server;
using MagazynManager.Tests.Technical;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NUnit.Framework;
using Respawn;
using Serilog;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;

namespace MagazynManager.Tests.IntegrationTests
{
    [TestFixture]
    public abstract class IntegrationTest<T> where T : class
    {
        protected WebApplicationFactory<T> _factory;
        protected string _connectionString;

        protected string ConnectionString
        {
            get
            {
                if (_connectionString == null)
                {
                    using (var sr = new StreamReader(GetConfigPath()))
                    {
                        var result = sr.ReadToEnd();
                        dynamic stuff = JsonConvert.DeserializeObject(result);
                        _connectionString = (string)stuff.ConnectionStrings.SqlServerConnection;
                    }
                }
                return _connectionString;
            }
        }

        [OneTimeSetUp]
        public void SetUpApplicationFactory()
        {
            _factory = CreateWebAppliationFactory(GetConfigPath());
            new DatabaseMigrator(ConnectionString).MigrateUp();
            SetUpLogging();
            Faker.DefaultStrictMode = true;
        }

        [SetUp]
        public void RefreshDatabase()
        {
            Respawn(ConnectionString).Wait();

            var inserts = new[]
            {
                "INSERT INTO [dbo].[User] (Id, Email, PasswordHash, Age, Salt) VALUES (N'3A7C2E38-385E-4C6A-8BFA-1767A3EBCCCC', N'admin@admin.com', N'32548419AC44CF75C9214238F72F72132AE24CE8', 42, N'858bfd79-da59-4756-897f-2aa5eb6029da')",
                "INSERT INTO [dbo].[User] (Id, Email, PasswordHash, Age, Salt) VALUES(N'A13A3CB3-928B-4C09-9FD6-739BCE18EDF0', N'demo@demo.com', N'423AEC62E306E2EC1D0146C2CD7F44734437E1B3', 18, N'3acd21a8-62a8-4829-a144-6c5b30d9b782')",
                "INSERT INTO [dbo].[Przedsiebiorstwo] (Id, Nazwa) VALUES(N'CF5CBB85-DCB0-470D-B8B9-9A29A097A73D', N'Overcity')",
                "INSERT INTO [dbo].[UserClaims] (Id, UserId, PrzedsiebiorstwoId, Claim, ClaimValue) VALUES(N'D87C1748-90E7-4E52-AA33-82E940A066BA', N'A13A3CB3-928B-4C09-9FD6-739BCE18EDF0', N'CF5CBB85-DCB0-470D-B8B9-9A29A097A73D', N'Permission.Ewidencjonowanie', N'lrcuda')",
                "INSERT INTO [dbo].[UserClaims] (Id, UserId, PrzedsiebiorstwoId, Claim, ClaimValue) VALUES(N'4126EEBE-51E6-4CFD-8C12-8EAC2F54D28C', N'A13A3CB3-928B-4C09-9FD6-739BCE18EDF0', N'CF5CBB85-DCB0-470D-B8B9-9A29A097A73D', N'Permission.Slowniki', N'lrcuda')",
                "INSERT INTO [dbo].[UserClaims] (Id, UserId, PrzedsiebiorstwoId, Claim, ClaimValue) VALUES(N'B6205F3A-E4C2-4A83-B46C-8F171DDAC1BA', N'3A7C2E38-385E-4C6A-8BFA-1767A3EBCCCC', N'CF5CBB85-DCB0-470D-B8B9-9A29A097A73D', N'Permission.Slowniki', N'lrcuda')",
                "INSERT INTO [dbo].[UserClaims] (Id, UserId, PrzedsiebiorstwoId, Claim, ClaimValue) VALUES(N'92D1F7A5-0A8D-4D05-8B8C-C3189DFC93FE', N'A13A3CB3-928B-4C09-9FD6-739BCE18EDF0', N'CF5CBB85-DCB0-470D-B8B9-9A29A097A73D', N'Permission.Rezerwacje', N'lrcuda')",
                "INSERT INTO [dbo].[UserClaims] (Id, UserId, PrzedsiebiorstwoId, Claim, ClaimValue) VALUES(N'4DA88ECF-5094-40F4-A810-E124A251E30D', N'3A7C2E38-385E-4C6A-8BFA-1767A3EBCCCC', N'CF5CBB85-DCB0-470D-B8B9-9A29A097A73D', N'Permission.Ewidencjonowanie', N'lrcuda')",
                "INSERT INTO [dbo].[UserClaims] (Id, UserId, PrzedsiebiorstwoId, Claim, ClaimValue) VALUES(N'BF26FCEB-D196-44C4-B4AB-F4B3276F42D8', N'3A7C2E38-385E-4C6A-8BFA-1767A3EBCCCC', N'CF5CBB85-DCB0-470D-B8B9-9A29A097A73D', N'Permission.Rezerwacje', N'lrcuda')",
                "INSERT INTO [dbo].[UserClaims] (Id, UserId, PrzedsiebiorstwoId, Claim, ClaimValue) VALUES(N'2AB2B508-2E78-4370-BBB2-FE20A53589A9', N'3A7C2E38-385E-4C6A-8BFA-1767A3EBCCCC', N'CF5CBB85-DCB0-470D-B8B9-9A29A097A73D', N'Permission.Administracja', N'lrcuda')"
            };

            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                foreach (var i in inserts)
                {
                    conn.Execute(i);
                }
            }
        }

        protected async Task Respawn(string connectionString)
        {
            var checkpoint = new Checkpoint
            {
                SchemasToInclude = new[]
                {
                    "dbo"
                },
                TablesToIgnore = new[]
                {
                    "VersionInfo"
                }
            };

            await checkpoint.Reset(connectionString);
        }

        private void SetUpLogging()
        {
            SerilogHelper.ConfigureSerilog();
            Log.Information("Staring tests");
        }

        private string GetConfigPath()
        {
            var projectDir = Directory.GetCurrentDirectory();
            return Path.Combine(projectDir, "appsettings.json");
        }

        private WebApplicationFactory<T> CreateWebAppliationFactory(string configPath)
        {
            var factory = new CustomApplicationFactory<T>();
            return factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((context, conf) =>
                {
                    conf.AddJsonFile(configPath);
                });
            });
        }
    }
}