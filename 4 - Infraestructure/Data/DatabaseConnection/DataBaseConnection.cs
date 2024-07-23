using System.Data.SqlClient;

namespace NpsApi.Data
{
  public class DatabaseConnection
  {
    private readonly string _connectionString;

    public DatabaseConnection(string sqlConnectionString)
    {
      _connectionString = sqlConnectionString;
    }

    public SqlConnection GetConnectionString()
    {
      return new SqlConnection(_connectionString);
    }
  }
}
