using NpsApi.Data;
using NpsApi.Models;
using System.Data;
using System.Data.SqlClient;

namespace NpsApi.Repositories
{
  public class FormsGroupsRepository
  {
    private readonly DatabaseConnection _connection;

    public FormsGroupsRepository(DatabaseConnection sqlConnection)
    {
      _connection = sqlConnection;
    }

    public async Task<FormsGroup> CreateGroup(FormsGroup group)
    {
      using (SqlConnection sqlConnection = _connection.GetConnectionString())
      {
        await sqlConnection.OpenAsync();

        string query = "INSERT INTO grupoFormularios (nome) VALUES (@Name); SELECT SCOPE_IDENTITY();";

        using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
        {
          sqlCommand.Parameters.AddWithValue("@Name", group.Name);

          group.Id = Convert.ToInt32(await sqlCommand.ExecuteScalarAsync());
          return group;
        }
      }
    }

    public async Task<FormsGroup?> GetGroupById(int id)
    {
      using (SqlConnection sqlConnection = _connection.GetConnectionString())
      {
        await sqlConnection.OpenAsync();

        string query = "SELECT * FROM grupoFormularios WHERE id = @Id";

        FormsGroup? group = null;

        using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
        {
          sqlCommand.Parameters.AddWithValue("@Id", id);

          using (SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync()) 
          {
            if (await sqlDataReader.ReadAsync())
            {
              group = new FormsGroup
              {
                Id = sqlDataReader.GetInt32("id"),
                Name = sqlDataReader.GetString("nome"),
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
      using (SqlConnection sqlConnection = _connection.GetConnectionString())
      {
        await sqlConnection.OpenAsync();

        string query = "SELECT * FROM grupoFormularios";

        List<FormsGroup> groupsList = new List<FormsGroup>();

        using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
        {
          using (SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync())
          {
            while (await sqlDataReader.ReadAsync())
            {
              FormsGroup group = new FormsGroup
              {
                Id = sqlDataReader.GetInt32("id"),
                Name = sqlDataReader.GetString("nome"),
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
      using (SqlConnection sqlConnection = _connection.GetConnectionString())
      {
        await sqlConnection.OpenAsync();

        string query = "DELETE FROM grupoFormularios WHERE id = @Id";

        using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
        {
          sqlCommand.Parameters.AddWithValue("@Id", id);

          return await sqlCommand.ExecuteNonQueryAsync() > 0;
        }
      }
    }

    public async Task<bool> UpdateGroup(int id, FormsGroup group)
    {
      using (SqlConnection sqlConnection = _connection.GetConnectionString())
      {
        await sqlConnection.OpenAsync();

        string query = "UPDATE grupoFormularios SET nome = @Name WHERE id = @Id";

        using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
        {
          sqlCommand.Parameters.AddWithValue("@Id", id);
          sqlCommand.Parameters.AddWithValue("@Name", group.Name);

          return await sqlCommand.ExecuteNonQueryAsync() > 0;
        }
      }
    }
  }
}
