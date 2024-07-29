using System.Data.SqlClient;

namespace NpsApi.Data
{
  public class DatabaseConnection
  {
    private readonly string _databaseConnectionString;

    public DatabaseConnection(string sqlConnectionString)
    {
      _databaseConnectionString = sqlConnectionString;
    }

    public SqlConnection GetConnectionString()
    {
      return new SqlConnection(_databaseConnectionString);
    }
  }
}
