using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace MagazynManager.Infrastructure
{
    public class DbConnectionSource : IDbConnectionSource
    {
        private readonly string _connectionString;

        public DbConnectionSource(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SqlServerConnection");
        }

        public IDbConnection GetConnection()
        {
            IDbConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            return connection;
        }
    }
}