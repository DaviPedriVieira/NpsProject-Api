using NpsApi.Data;
using NpsApi.Models;
using System.Data;
using System.Data.SqlClient;

namespace NpsApi.Repositories
{
  public class QuestionsRepository
  {
    private readonly DataBaseConnection _connection;

    public QuestionsRepository(DataBaseConnection connection)
    {
      _connection = connection;
    }

    public async Task<Questions> CreateQuestion(Questions question)
    {
      using (SqlConnection connection = _connection.GetConnectionString())
      {
        await connection.OpenAsync();

        string query = "INSERT INTO perguntas (conteudo, idFormulario) VALUES (@Content, @FormId); SELECT SCOPE_IDENTITY();";

        using (SqlCommand command = new SqlCommand(query, connection))
        {
          command.Parameters.AddWithValue("@Content", question.Content);
          command.Parameters.AddWithValue("@FormId", question.FormId);

          var id = await command.ExecuteScalarAsync();
          question.Id = Convert.ToInt32(id);

          return question;
        }
      }
    }

    public async Task<Questions?> GetQuestionById(int id)
    {
      using (SqlConnection connection = _connection.GetConnectionString())
      {
        await connection.OpenAsync();

        string query = "SELECT * FROM perguntas WHERE id = @Id";

        using (SqlCommand command = new SqlCommand(query, connection))
        {
          command.Parameters.AddWithValue("@Id", id);

          using (SqlDataReader reader = await command.ExecuteReaderAsync())
          {
            if (await reader.ReadAsync())
            {
              Questions question = new Questions
              {
                Id = reader.GetInt32("id"),
                FormId = reader.GetInt32("idFormulario"),
                Content = reader.GetString("conteudo"),
                Answers = new List<Answers>(),
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

    public async Task<List<Questions>> GetQuestions()
    {
      using (SqlConnection connection = _connection.GetConnectionString())
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
                Id = reader.GetInt32("id"),
                FormId = reader.GetInt32("idFormulario"),
                Content = reader.GetString("conteudo"),
                Answers = new List<Answers>(),
              };

              allQuestions.Add(question);
            }
          }
        }

        return allQuestions;
      }
    }

    public async Task<bool> DeleteQuestion(int id)
    {
      using (SqlConnection connection = _connection.GetConnectionString())
      {
        await connection.OpenAsync();

        string query = "DELETE FROM perguntas WHERE id = @Id";

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

    public async Task<bool> UpdateQuestion(int id, Questions question)
    {
      using (SqlConnection connection = _connection.GetConnectionString())
      {
        await connection.OpenAsync();

        string query = "UPDATE perguntas SET conteudo = @Content WHERE id = @Id";

        using (SqlCommand command = new SqlCommand(query, connection))
        {
          command.Parameters.AddWithValue("@Id", id);
          command.Parameters.AddWithValue("@Content", question.Content);

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

    public async Task<List<Questions>> GetQuestionsByFormId(int formId)
    {
      using (SqlConnection connection = _connection.GetConnectionString())
      {
        await connection.OpenAsync();

        string query = "SELECT * FROM perguntas WHERE idFormulario = @FormId";

        List<Questions> questionsList = new List<Questions>();

        using (SqlCommand command = new SqlCommand(query, connection))
        {
          command.Parameters.AddWithValue("@FormId", formId);

          using (SqlDataReader reader = await command.ExecuteReaderAsync())
          {
            while (await reader.ReadAsync())
            {
              Questions question = new Questions
              {
                Id = reader.GetInt32("id"),
                FormId = reader.GetInt32("idFormulario"),
                Content = reader.GetString("conteudo"),
                Answers = new List<Answers>(),
              };

              questionsList.Add(question);
            }
          }
        }

        return questionsList;
      }
    }
  }
}
