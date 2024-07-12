using NpsApi.Data;
using NpsApi.Models;
using System.Data;
using System.Data.SqlClient;

namespace NpsApi.Repositories
{
  public class FormsGroupsRepository
  {
    private readonly DataBaseConnection _databaseConnection;

    public FormsGroupsRepository(DataBaseConnection connection)
    {
      _databaseConnection = connection;
    }

    public async Task<FormsGroups> CreateGroup(FormsGroups group)
    {
      using (SqlConnection connection = _databaseConnection.GetConnectionString())
      {
        await connection.OpenAsync();

        string query = "INSERT INTO grupoFormularios (nome) VALUES (@Name); SELECT SCOPE_IDENTITY();";

        using (SqlCommand command = new SqlCommand(query, connection))
        {
          command.Parameters.AddWithValue("@Name", group.Name);

          var id = await command.ExecuteScalarAsync();
          group.Id = Convert.ToInt32(id);

          return group;
        }
      }
    }

    public async Task<FormsGroups?> GetGroupById(int id)
    {
      using (SqlConnection connection = _databaseConnection.GetConnectionString())
      {
        await connection.OpenAsync();

        string query = "SELECT * FROM grupoFormularios WHERE id = @Id";

        using (SqlCommand command = new SqlCommand(query, connection))
        {
          command.Parameters.AddWithValue("@Id", id);

          using (SqlDataReader reader = await command.ExecuteReaderAsync())
          {
            if (await reader.ReadAsync())
            {
              FormsGroups group = new FormsGroups
              {
                Id = reader.GetInt32("id"),
                Name = reader.GetString("nome"),
                Forms = new List<Forms>(),
              };

              return group;
            }
            else
            {
              return null;
            }
          }
        }
      }
    }

    public async Task<List<FormsGroups>> GetGroups()
    {
      using (SqlConnection connection = _databaseConnection.GetConnectionString())
      {
        await connection.OpenAsync();

        string query = "SELECT * FROM grupoFormularios";

        List<FormsGroups> allGroups = new List<FormsGroups>();

        using (SqlCommand command = new SqlCommand(query, connection))
        {
          using (SqlDataReader reader = await command.ExecuteReaderAsync())
          {
            while (await reader.ReadAsync())
            {
              FormsGroups group = new FormsGroups
              {
                Id = reader.GetInt32("id"),
                Name = reader.GetString("nome"),
                Forms = new List<Forms>(),
              };

              allGroups.Add(group);
            }
          }
        }

        return allGroups;
      }
    }

    public async Task<bool> DeleteGroup(int id)
    {
      using (SqlConnection connection = _databaseConnection.GetConnectionString())
      {
        await connection.OpenAsync();

        string query = "DELETE FROM grupoFormularios WHERE id = @Id";

        using (SqlCommand command = new SqlCommand(query, connection))
        {
          command.Parameters.AddWithValue("@Id", id);

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

    public async Task<bool> UpdateGroup(int id, FormsGroups group)
    {
      using (SqlConnection connection = _databaseConnection.GetConnectionString())
      {
        await connection.OpenAsync();

        string query = "UPDATE grupoFormularios SET nome = @Name WHERE id = @Id";

        using (SqlCommand command = new SqlCommand(query, connection))
        {
          command.Parameters.AddWithValue("@Id", id);
          command.Parameters.AddWithValue("@Name", group.Name);

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
