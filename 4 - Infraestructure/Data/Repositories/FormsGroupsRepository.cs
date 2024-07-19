using NpsApi.Data;
using NpsApi.Models;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace NpsApi.Repositories
{
  public class FormsGroupsRepository
  {
    private readonly DataBaseConnection _connection;

    public FormsGroupsRepository(DataBaseConnection connection)
    {
      _connection = connection;
    }

    public async Task<FormsGroup> CreateGroup(FormsGroup group)
    {
      using (SqlConnection connection = _connection.GetConnectionString())
      {
        await connection.OpenAsync();

        string query = "INSERT INTO grupoFormularios (nome) VALUES (@Name); SELECT SCOPE_IDENTITY();";

        using (SqlCommand command = new SqlCommand(query, connection))
        {
          command.Parameters.AddWithValue("@Name", group.Name);

          group.Id = Convert.ToInt32(await command.ExecuteScalarAsync());
          return group;
        }
      }
    }

    public async Task<FormsGroup?> GetGroupById(int id)
    {
      using (SqlConnection connection = _connection.GetConnectionString())
      {
        await connection.OpenAsync();

        string query = "SELECT * FROM grupoFormularios WHERE id = @Id";

        FormsGroup? group = null;

        using (SqlCommand command = new SqlCommand(query, connection))
        {
          command.Parameters.AddWithValue("@Id", id);

          using (SqlDataReader reader = await command.ExecuteReaderAsync()) 
          {
            if(await reader.ReadAsync())
            {
              group = new FormsGroup
              {
                Id = reader.GetInt32("id"),
                Name = reader.GetString("nome"),
                Forms = new List<Form>(),
              };
            }

            return group;
          }
        }
      }
    }

    public async Task<List<FormsGroup>> GetGroups()
    {
      using (SqlConnection connection = _connection.GetConnectionString())
      {
        await connection.OpenAsync();

        string query = "SELECT * FROM grupoFormularios";

        List<FormsGroup> groupsList = new List<FormsGroup>();

        using (SqlCommand command = new SqlCommand(query, connection))
        {
          using (SqlDataReader reader = await command.ExecuteReaderAsync())
          {
            while (await reader.ReadAsync())
            {
              FormsGroup group = new FormsGroup
              {
                Id = reader.GetInt32("id"),
                Name = reader.GetString("nome"),
                Forms = new List<Form>(),
              };

              groupsList.Add(group);
            }
          }
        }

        return groupsList;
      }
    }

    public async Task<bool> DeleteGroup(int id)
    {
      using (SqlConnection connection = _connection.GetConnectionString())
      {
        await connection.OpenAsync();

        string query = "DELETE FROM grupoFormularios WHERE id = @Id";

        using (SqlCommand command = new SqlCommand(query, connection))
        {
          command.Parameters.AddWithValue("@Id", id);

          return await command.ExecuteNonQueryAsync() > 0;
        }
      }
    }

    public async Task<bool> UpdateGroup(int id, FormsGroup group)
    {
      using (SqlConnection connection = _connection.GetConnectionString())
      {
        await connection.OpenAsync();

        string query = "UPDATE grupoFormularios SET nome = @Name WHERE id = @Id";

        using (SqlCommand command = new SqlCommand(query, connection))
        {
          command.Parameters.AddWithValue("@Id", id);
          command.Parameters.AddWithValue("@Name", group.Name);

          return await command.ExecuteNonQueryAsync() > 0;
        }
      }
    }
  }
}
