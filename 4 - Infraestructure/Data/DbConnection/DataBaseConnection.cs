using System.Data.SqlClient;

namespace NpsApi.Data
{
  public class DataBaseConnection(string connectionString)
  {
    private readonly string _connectionString = connectionString;

    public SqlConnection GetConnectionString()
    {
      return new SqlConnection(_connectionString);
    }
  }
}
