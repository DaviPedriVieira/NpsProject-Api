using System.Data.SqlClient;

namespace NpsApi.Data
{
  public class DataBaseConnection
  {
    private readonly string _connectionString;

    public DataBaseConnection(string connectionString)
    {
      _connectionString = connectionString;
    }

    public SqlConnection GetConnectionString()
    {
      return new SqlConnection(_connectionString);
    }
  }
}
