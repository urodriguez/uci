using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Persistence
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        public IDbConnection GetSqlConnection()
        {
            var sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["InventappContext"].ConnectionString);
            sqlConnection.Open();

            return sqlConnection;
        }
    }
}