using System.Data;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Dapper
{
    public interface IDbConnectionFactory
    {
        Task<IDbConnection> GetOpenedSqlConnectionAsync();
    }
}