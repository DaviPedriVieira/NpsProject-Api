using NpsApi.Data;
using NpsApi.Models;
using System.Data;
using System.Data.SqlClient;

namespace NpsApi.Repositories
{
  public class QuestionsRepository
  {
    private readonly DatabaseConnection _databaseConnection;

    public QuestionsRepository(DatabaseConnection sqlConnection)
    {
      _databaseConnection = sqlConnection;
    }

    public async Task<Question> CreateQuestion(Question question)
    {
      using (SqlConnection sqlConnection = _databaseConnection.GetConnectionString())
      {
        await sqlConnection.OpenAsync();

        string query = "INSERT INTO perguntas (conteudo, idFormulario) VALUES (@Content, @FormId); SELECT SCOPE_IDENTITY();";

        using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
        {
          sqlCommand.Parameters.AddWithValue("@Content", question.Content);
          sqlCommand.Parameters.AddWithValue("@FormId", question.FormId);

          question.Id = Convert.ToInt32(await sqlCommand.ExecuteScalarAsync());
          return question;
        }
      }
    }

    public async Task<Question?> GetQuestionById(int id)
    {
      using (SqlConnection sqlConnection = _databaseConnection.GetConnectionString())
      {
        await sqlConnection.OpenAsync();

        string query = "SELECT * FROM perguntas WHERE id = @Id";

        Question? question = null;

        using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
        {
          sqlCommand.Parameters.AddWithValue("@Id", id);

          using (SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync())
          {
            if (await sqlDataReader.ReadAsync())
            {
              question = new Question
              {
                Id = sqlDataReader.GetInt32("id"),
                FormId = sqlDataReader.GetInt32("idFormulario"),
                Content = sqlDataReader.GetString("conteudo"),
                Answers = new List<Answer>(),
              };
            }

          return question;
          }
        }
      }
    }

    public async Task<List<Question>> GetQuestions()
    {
      using (SqlConnection sqlConnection = _databaseConnection.GetConnectionString())
      {
        await sqlConnection.OpenAsync();

        string query = "SELECT * FROM perguntas";

        List<Question> questionsList = new List<Question>();

        using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
        {
          using (SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync())
          {
            while (await sqlDataReader.ReadAsync())
            {
              Question question = new Question
              {
                Id = sqlDataReader.GetInt32("id"),
                FormId = sqlDataReader.GetInt32("idFormulario"),
                Content = sqlDataReader.GetString("conteudo"),
                Answers = new List<Answer>(),
              };

              questionsList.Add(question);
            }
          }
        }

        return questionsList;
      }
    }

    public async Task<bool> DeleteQuestion(int id)
    {
      using (SqlConnection sqlConnection = _databaseConnection.GetConnectionString())
      {
        await sqlConnection.OpenAsync();

        string query = "DELETE FROM perguntas WHERE id = @Id";

        using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
        {
          sqlCommand.Parameters.AddWithValue("@Id", id);

          return await sqlCommand.ExecuteNonQueryAsync() > 0;
        }
      }
    }

    public async Task<bool> UpdateQuestion(int id, Question question)
    {
      using (SqlConnection sqlConnection = _databaseConnection.GetConnectionString())
      {
        await sqlConnection.OpenAsync();

        string query = "UPDATE perguntas SET conteudo = @Content WHERE id = @Id";

        using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
        {
          sqlCommand.Parameters.AddWithValue("@Id", id);
          sqlCommand.Parameters.AddWithValue("@Content", question.Content);

          return await sqlCommand.ExecuteNonQueryAsync() > 0;
        }
      }
    }

    public async Task<List<Question>> GetQuestionsByFormId(int formId)
    {
      using (SqlConnection sqlConnection = _databaseConnection.GetConnectionString())
      {
        await sqlConnection.OpenAsync();

        string query = "SELECT * FROM perguntas WHERE idFormulario = @FormId";

        List<Question> questionsList = new List<Question>();

        using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
        {
          sqlCommand.Parameters.AddWithValue("@FormId", formId);

          using (SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync())
          {
            while (await sqlDataReader.ReadAsync())
            {
              Question question = new Question
              {
                Id = sqlDataReader.GetInt32("id"),
                FormId = sqlDataReader.GetInt32("idFormulario"),
                Content = sqlDataReader.GetString("conteudo"),
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
