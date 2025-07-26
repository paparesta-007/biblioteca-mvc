using System.Data;
using Microsoft.Data.SqlClient;
namespace Biblioteca.Data;

public class Database(string connectionString)
{
    public SqlConnection GetConnection()
    {
        return new SqlConnection(connectionString);
    }
    public SqlDataReader GetExecuteReader(string query, SqlParameter[]? parameters = null)
    {
        var conn = GetConnection();
        using var cmd = new SqlCommand(query, conn);
        
        if (parameters != null)
            cmd.Parameters.AddRange(parameters);

        conn.Open();
        return cmd.ExecuteReader(CommandBehavior.CloseConnection);
    }



    public int ExecuteNonQuery(string sql, SqlParameter[]? parameters = null)
    {
        var connection = GetConnection();
        using var command = new SqlCommand(sql, connection);
        if (parameters != null)
        {
            command.Parameters.AddRange(parameters);
        }
        connection.Open();
        return command.ExecuteNonQuery();
    }


}