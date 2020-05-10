using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MagazynManager.Infrastructure
{
    public class DatabaseMigrator
    {
        private readonly string connectionString;

        public DatabaseMigrator(string connection)
        {
            connectionString = connection;
        }

        public void MigrateUp()
        {
            var serviceProvider = CreateServices();
            using (var scope = serviceProvider.CreateScope())
            {
                UpdateDatabase(scope.ServiceProvider);
            }
        }

        /// <summary>
        /// Configure the dependency injection services
        /// </summary>
        private IServiceProvider CreateServices()
        {
            return new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddSqlServer()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(typeof(InfrastructureModule).Assembly).For.Migrations())
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                .BuildServiceProvider(false);
        }

        /// <summary>
        /// Update the database
        /// </summary>
        private void UpdateDatabase(IServiceProvider serviceProvider)
        {
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
            runner.MigrateUp();
        }
    }
}