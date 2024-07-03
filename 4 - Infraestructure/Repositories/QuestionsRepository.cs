using NpsApi.Data;
using NpsApi.Models;
using System.Data;
using System.Data.SqlClient;

namespace NpsApi.Repositories
{
  public class QuestionsRepository
  {
    private readonly DataBaseConnection _databaseConnection;

    public QuestionsRepository(DataBaseConnection connection)
    {
      _databaseConnection = connection;
    }

    public async Task<int> Create(Questions question)
    {
      using (SqlConnection connection = _databaseConnection.GetConnectionString())
      {
        await connection.OpenAsync();

        string query = "INSERT INTO perguntas (conteudo, idFormulario) VALUES (@conteudo, @idFormulario); SELECT SCOPE_IDENTITY();";

        using (SqlCommand command = new SqlCommand(query, connection))
        {
          command.Parameters.AddWithValue("@conteudo", question.Content);
          command.Parameters.AddWithValue("@idFormulario", question.FormId);

          var id = await command.ExecuteScalarAsync();
          return Convert.ToInt32(id);
        }
      }
    }

    public async Task<Questions?> GetById(int id)
    {
      using (SqlConnection connection = _databaseConnection.GetConnectionString())
      {
        await connection.OpenAsync();

        string query = "SELECT * FROM perguntas WHERE id = @id";

        using (SqlCommand command = new SqlCommand(query, connection))
        {
          command.Parameters.AddWithValue("@id", id);

          using (SqlDataReader reader = await command.ExecuteReaderAsync())
          {
            if (await reader.ReadAsync())
            {
              Questions question = new Questions
              {
                Id = reader.GetInt32("id"),
                FormId = reader.GetInt32("idFormulario"),
                Content = reader.GetString("conteudo"),
              };

              return question;
            }
            else
            {
              return null;
            }
          }
        }
      }
    }

    public async Task<List<Questions>> GetByFormId(int formId)
    {
      using (SqlConnection connection = _databaseConnection.GetConnectionString())
      {
        await connection.OpenAsync();

        string query = "SELECT * FROM perguntas WHERE idFormulario = @idFormulario";

        List<Questions> questionsList = new List<Questions>();

        using (SqlCommand command = new SqlCommand(query, connection))
        {
          command.Parameters.AddWithValue("@idFormulario", formId);

          using (SqlDataReader reader = await command.ExecuteReaderAsync())
          {
            while (await reader.ReadAsync())
            {
              Questions question = new Questions
              {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                Content = reader.GetString(reader.GetOrdinal("conteudo")),
              };

              questionsList.Add(question);
            }
          }
        }

        return questionsList;
      }
    }

    public async Task<List<Questions>> Get()
    {
      using (SqlConnection connection = _databaseConnection.GetConnectionString())
      {
        await connection.OpenAsync();

        string query = "SELECT * FROM perguntas";

        List<Questions> allQuestions = new List<Questions>();

        using (SqlCommand command = new SqlCommand(query, connection))
        {
          using (SqlDataReader reader = await command.ExecuteReaderAsync())
          {
            while (await reader.ReadAsync())
            {
              Questions question = new Questions
              {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                FormId = reader.GetInt32(reader.GetOrdinal("idFormulario")),
                Content = reader.GetString(reader.GetOrdinal("conteudo")),
              };

              allQuestions.Add(question);
            }
          }
        }

        return allQuestions;
      }
    }

    public async Task<bool> Delete(int id)
    {
      using (SqlConnection connection = _databaseConnection.GetConnectionString())
      {
        await connection.OpenAsync();

        string query = "DELETE FROM perguntas WHERE id = @id";

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

    public async Task<bool> Update(int id, Questions question)
    {
      using (SqlConnection connection = _databaseConnection.GetConnectionString())
      {
        await connection.OpenAsync();

        string query = "UPDATE perguntas SET conteudo = @conteudo WHERE id = @id";

        using (SqlCommand command = new SqlCommand(query, connection))
        {
          command.Parameters.AddWithValue("@id", id);
          command.Parameters.AddWithValue("@conteudo", question.Content);

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
