using Dapper;
using DataAccessClassLibrary.Models;
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

    public void SaveData<T>(string sql, T parameters, string connectionString)
    {
        using(IDbConnection connection = new SqlConnection(connectionString))
        {
            connection.Execute(sql, parameters);
        }
    }
}
