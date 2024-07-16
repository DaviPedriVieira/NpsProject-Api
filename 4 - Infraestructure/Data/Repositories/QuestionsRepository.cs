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

    public async Task<Question> CreateQuestion(Question question)
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

    public async Task<Question?> GetQuestionById(int id)
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
              Question question = new Question
              {
                Id = reader.GetInt32("id"),
                FormId = reader.GetInt32("idFormulario"),
                Content = reader.GetString("conteudo"),
                Answers = new List<Answer>(),
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

    public async Task<List<Question>> GetQuestions()
    {
      using (SqlConnection connection = _connection.GetConnectionString())
      {
        await connection.OpenAsync();

        string query = "SELECT * FROM perguntas";

        List<Question> questions = new List<Question>();

        using (SqlCommand command = new SqlCommand(query, connection))
        {
          using (SqlDataReader reader = await command.ExecuteReaderAsync())
          {
            while (await reader.ReadAsync())
            {
              Question question = new Question
              {
                Id = reader.GetInt32("id"),
                FormId = reader.GetInt32("idFormulario"),
                Content = reader.GetString("conteudo"),
                Answers = new List<Answer>(),
              };

              questions.Add(question);
            }
          }
        }

        return questions;
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

          return await command.ExecuteNonQueryAsync() > 0;
        }
      }
    }

    public async Task<bool> UpdateQuestion(int id, Question question)
    {
      using (SqlConnection connection = _connection.GetConnectionString())
      {
        await connection.OpenAsync();

        string query = "UPDATE perguntas SET conteudo = @Content WHERE id = @Id";

        using (SqlCommand command = new SqlCommand(query, connection))
        {
          command.Parameters.AddWithValue("@Id", id);
          command.Parameters.AddWithValue("@Content", question.Content);

          return await command.ExecuteNonQueryAsync() > 0;
        }
      }
    }

    public async Task<List<Question>> GetQuestionsByFormId(int formId)
    {
      using (SqlConnection connection = _connection.GetConnectionString())
      {
        await connection.OpenAsync();

        string query = "SELECT * FROM perguntas WHERE idFormulario = @FormId";

        List<Question> questionsList = new List<Question>();

        using (SqlCommand command = new SqlCommand(query, connection))
        {
          command.Parameters.AddWithValue("@FormId", formId);

          using (SqlDataReader reader = await command.ExecuteReaderAsync())
          {
            while (await reader.ReadAsync())
            {
              Question question = new Question
              {
                Id = reader.GetInt32("id"),
                FormId = reader.GetInt32("idFormulario"),
                Content = reader.GetString("conteudo"),
                Answers = new List<Answer>(),
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
