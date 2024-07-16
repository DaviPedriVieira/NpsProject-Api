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

    public async Task<Form> CreateForm(Form form)
    {
      using (SqlConnection connection = _databaseConnection.GetConnectionString())
      {
        await connection.OpenAsync();

        string query = "INSERT INTO formularios (nome, idGrupo) VALUES (@Name, @GroupId); SELECT SCOPE_IDENTITY();";

        using (SqlCommand command = new SqlCommand(query, connection))
        {
          command.Parameters.AddWithValue("@Name", form.Name);
          command.Parameters.AddWithValue("@GroupId", form.GroupId);

          var id = await command.ExecuteScalarAsync();
          form.Id = Convert.ToInt32(id);

          return form;
        }
      }
    }

    public async Task<Form?> GetFormById(int id)
    {
      using (SqlConnection connection = _databaseConnection.GetConnectionString())
      {
        await connection.OpenAsync();

        string query = "SELECT * FROM formularios WHERE id = @Id";

        Form? form = null;

        using (SqlCommand command = new SqlCommand(query, connection))
        {
          command.Parameters.AddWithValue("@Id", id);

          using (SqlDataReader reader = await command.ExecuteReaderAsync())
          {
            if(await reader.ReadAsync())
            {
              form = new Form
              {
                Id = reader.GetInt32("id"),
                GroupId = reader.GetInt32("idGrupo"),
                Name = reader.GetString("nome"),
                Questions = new List<Question>(),
              };
            }

            return form;
          }
        }
      }
    }

    public async Task<List<Form>> GetForms()
    {
      using (SqlConnection connection = _databaseConnection.GetConnectionString())
      {
        await connection.OpenAsync();

        string query = "SELECT * FROM formularios";

        List<Form> allForms = new List<Form>();

        using (SqlCommand command = new SqlCommand(query, connection))
        {
          using (SqlDataReader reader = await command.ExecuteReaderAsync())
          {
            while (await reader.ReadAsync())
            {
              Form form = new Form
              {
                Id = reader.GetInt32("id"),
                GroupId = reader.GetInt32("idGrupo"),
                Name = reader.GetString("nome"),
                Questions = new List<Question>(),
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

        string query = "DELETE FROM formularios WHERE id = @Id";

        using (SqlCommand command = new SqlCommand(query, connection))
        {
          command.Parameters.AddWithValue("@Id", id);

          return await command.ExecuteNonQueryAsync() > 0;
        }
      }
    }

    public async Task<bool> UpdateForm(int id, Form form)
    {
      using (SqlConnection connection = _databaseConnection.GetConnectionString())
      {
        await connection.OpenAsync();

        string query = "UPDATE formularios SET nome = @Name WHERE id = @Id";

        using (SqlCommand command = new SqlCommand(query, connection))
        {
          command.Parameters.AddWithValue("@Id", id);
          command.Parameters.AddWithValue("@Name", form.Name);

          return await command.ExecuteNonQueryAsync() > 0;
        }
      }
    }

    public async Task<List<Form>> GetFormsByGroupId(int groupId)
    {
      using (SqlConnection connection = _databaseConnection.GetConnectionString())
      {
        await connection.OpenAsync();

        string query = "SELECT * FROM formularios WHERE idGrupo = @GroupId";

        List<Form> formsList = new List<Form>();

        using (SqlCommand command = new SqlCommand(query, connection))
        {
          command.Parameters.AddWithValue("@GroupId", groupId);

          using (SqlDataReader reader = await command.ExecuteReaderAsync())
          {
            while (await reader.ReadAsync())
            {
              Form form = new Form
              {
                Id = reader.GetInt32("id"),
                Name = reader.GetString("nome"),
                Questions = new List<Question>(),
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
