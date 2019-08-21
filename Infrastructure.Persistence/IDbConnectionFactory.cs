using System.Data;

namespace Infrastructure.Persistence
{
    public interface IDbConnectionFactory
    {
        IDbConnection GetSqlConnection();
    }
}