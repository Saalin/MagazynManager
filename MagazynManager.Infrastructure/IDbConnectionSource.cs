using System.Data;

namespace MagazynManager.Infrastructure
{
    public interface IDbConnectionSource
    {
        IDbConnection GetConnection();
    }
}