using NpsApi.Data;
using NpsApi.Models;
using System.Data;
using System.Data.SqlClient;

namespace NpsApi.Repositories
{
  public class FormsRepository
  {
    private readonly DataBaseConnection _databaseConnection;

    public FormsRepository(DataBaseConnection connection)
    {
      _databaseConnection = connection;
    }

    public async Task<Forms> CreateForm(Forms form)
    {
      using (SqlConnection connection = _databaseConnection.GetConnectionString())
      {
        await connection.OpenAsync();

        string query = "INSERT INTO formularios (nome, idGrupo) VALUES (@nome, @idGrupo); SELECT SCOPE_IDENTITY();";

        using (SqlCommand command = new SqlCommand(query, connection))
        {
          command.Parameters.AddWithValue("@nome", form.Name);
          command.Parameters.AddWithValue("@idGrupo", form.GroupId);

          var id = await command.ExecuteScalarAsync();

          Forms newForm = new Forms
          {
            Id = Convert.ToInt32(id),
            GroupId = form.GroupId,
            Name = form.Name,
          };

          return form;
        }
      }
    }

    public async Task<Forms?> GetFormById(int id)
    {
      using (SqlConnection connection = _databaseConnection.GetConnectionString())
      {
        await connection.OpenAsync();

        string query = "SELECT * FROM formularios WHERE id = @id";

        using (SqlCommand command = new SqlCommand(query, connection))
        {
          command.Parameters.AddWithValue("@id", id);

          using (SqlDataReader reader = await command.ExecuteReaderAsync())
          {
            if (await reader.ReadAsync())
            {
              Forms form = new Forms
              {
                Id = reader.GetInt32("id"),
                GroupId = reader.GetInt32("idGrupo"),
                Name = reader.GetString("nome"),
                Questions = new List<Questions>(),
              };

              return form;
            }
            else
            {
              return null;
            }
          }
        }
      }
    }

    public async Task<List<Forms>> GetForms()
    {
      using (SqlConnection connection = _databaseConnection.GetConnectionString())
      {
        await connection.OpenAsync();

        string query = "SELECT * FROM formularios";

        List<Forms> allForms = new List<Forms>();

        using (SqlCommand command = new SqlCommand(query, connection))
        {
          using (SqlDataReader reader = await command.ExecuteReaderAsync())
          {
            while (await reader.ReadAsync())
            {
              Forms form = new Forms
              {
                Id = reader.GetInt32("id"),
                GroupId = reader.GetInt32("idGrupo"),
                Name = reader.GetString("nome"),
                Questions = new List<Questions>(),
              };

              allForms.Add(form);
            }
          }
        }

        return allForms;
      }
    }

    public async Task<bool> DeleteForm(int id)
    {
      using (SqlConnection connection = _databaseConnection.GetConnectionString())
      {
        await connection.OpenAsync();

        string query = "DELETE FROM formularios WHERE id = @id";

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

    public async Task<bool> UpdateForm(int id, Forms form)
    {
      using (SqlConnection connection = _databaseConnection.GetConnectionString())
      {
        await connection.OpenAsync();

        string query = "UPDATE formularios SET nome = @nome WHERE id = @id";

        using (SqlCommand command = new SqlCommand(query, connection))
        {
          command.Parameters.AddWithValue("@id", id);
          command.Parameters.AddWithValue("@nome", form.Name);

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

    public async Task<List<Forms>> GetFormsByGroupId(int groupId)
    {
      using (SqlConnection connection = _databaseConnection.GetConnectionString())
      {
        await connection.OpenAsync();

        string query = "SELECT * FROM formularios WHERE idGrupo = @idGrupo";

        List<Forms> formsList = new List<Forms>();

        using (SqlCommand command = new SqlCommand(query, connection))
        {
          command.Parameters.AddWithValue("@idGrupo", groupId);

          using (SqlDataReader reader = await command.ExecuteReaderAsync())
          {
            while (await reader.ReadAsync())
            {
              Forms form = new Forms
              {
                Id = reader.GetInt32("id"),
                Name = reader.GetString("nome"),
                Questions = new List<Questions>(),
              };

              formsList.Add(form);
            }
          }
        }

        return formsList;
      }
    }

  }
}
