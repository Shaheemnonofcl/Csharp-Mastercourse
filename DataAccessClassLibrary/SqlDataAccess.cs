using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace DataAccessClassLibrary;

public class SqlDataAccess
{

    public List<T> LoadData<T, U>(string sqlStatement, U parameters, string ConnectionString)
    {
        using (IDbConnection connection = new SqlConnection(ConnectionString))
        {
            List<T> rows = connection.Query<T>(sqlStatement, parameters).ToList();
            return rows;
        }
    }
}
