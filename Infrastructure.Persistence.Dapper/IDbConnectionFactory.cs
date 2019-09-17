using System.Data;

namespace Infrastructure.Persistence.Dapper
{
    public interface IDbConnectionFactory
    {
        IDbConnection GetSqlConnection();
    }
}