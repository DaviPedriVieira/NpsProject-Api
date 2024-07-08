using NpsApi.Data;
using NpsApi.Models;
using System.Data.SqlClient;

namespace NpsApi.Generics
{
  public class Crud
  {
    private readonly DataBaseConnection _connection;

    public Crud(DataBaseConnection connection)
    {
      _connection = connection;
    }

    public async Task<T> GenericCreate<T>(T item, string query, Dictionary<string, object> parameters)
    {
      using (SqlConnection connection = _connection.GetConnectionString())
      {
        await connection.OpenAsync();

        using (SqlCommand command = new SqlCommand(query, connection))
        {
          foreach (var parameter in parameters)
          {
            command.Parameters.AddWithValue(parameter.Key, parameter.Value);
          }

          var id = await command.ExecuteScalarAsync();
          item.Id = Convert.ToInt32(id);

          return item;
        }
      }
    }

    public async Task<bool> GenericDelete(int id, string query)
    {
      using (SqlConnection connection = _connection.GetConnectionString())
      {
        await connection.OpenAsync();

        using (SqlCommand command = new SqlCommand(query, connection))
        {
          command.Parameters.AddWithValue("@id", id);

          try
          {
            await command.ExecuteNonQueryAsync();
            return true;
          }
          catch (SqlException)
          {
            return false;
          }
        }
      }
    }

    public async Task<bool> GenericUpdate<T>(string query, Dictionary<string, object> parameters)
    {
      using (SqlConnection connection = _connection.GetConnectionString())
      {
        await connection.OpenAsync();

        using (SqlCommand command = new SqlCommand(query, connection))
        {
          foreach (var parameter in parameters)
          {
            command.Parameters.AddWithValue(parameter.Key, parameter.Value);
          }

          try
          {
            await command.ExecuteNonQueryAsync();
            return true;
          }
          catch (SqlException)
          {
            return false;
          }
        }
      }
    }

  }
}
