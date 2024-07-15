using NpsApi.Data;
using NpsApi.Models;
using System.Data;
using System.Data.SqlClient;

namespace NpsApi.Repositories
{
  public class AnswersRepository
  {
    private readonly DataBaseConnection _connection;

    public AnswersRepository(DataBaseConnection connection)
    {
      _connection = connection;
    }

    public async Task<Answer> SubmitAnswer(Answer answer)
    {
      using(SqlConnection connection = _connection.GetConnectionString())
      {
        await connection.OpenAsync();

        string query = "INSERT INTO respostas (idPergunta, idUsuario, resposta, descricao, dataHora) VALUES (@QuestionId, @UserId, @Grade, @Notes, @Date); SELECT SCOPE_IDENTITY();";

        using(SqlCommand command = new SqlCommand (query, connection))
        {
          command.Parameters.AddWithValue("@QuestionId", answer.QuestionId);
          command.Parameters.AddWithValue("@UserId", answer.UserId);
          command.Parameters.AddWithValue("@Grade", answer.Grade);
          command.Parameters.AddWithValue("@Notes", answer.Notes);
          command.Parameters.AddWithValue("@Date", DateTime.Now);

          var id = await command.ExecuteScalarAsync();
          answer.Id = Convert.ToInt32(id);

          return answer;
        }
      }
    }

    public async Task<bool> DeleteAnswer(int id)
    {
      using (SqlConnection connection = _connection.GetConnectionString())
      {
        await connection.OpenAsync();

        string query = "DELETE FROM respostas WHERE id = @Id";

        using (SqlCommand command = new SqlCommand(query, connection))
        {
          command.Parameters.AddWithValue("@Id", id);

          if (await command.ExecuteNonQueryAsync() > 0)
          {
            return true;
          }
          else
          {
            return false;
          }
        }
      }
    }

    public async Task<List<Answer>> GetAnswersByClientId(int userId)
    {
      List<Answer> answersList = new List<Answer>();

      using (SqlConnection connection = _connection.GetConnectionString())
      {
        await connection.OpenAsync();

        string query = "SELECT * FROM  respostas WHERE idUsuario = @UserId";

        using (SqlCommand command = new SqlCommand(query, connection))
        {
          command.Parameters.AddWithValue("@UserId", userId);

          using (SqlDataReader reader = await command.ExecuteReaderAsync())
          {
            while (await reader.ReadAsync())
            {
              Answer answer = new Answer
              {
                Id = reader.GetInt32("id"),
                QuestionId = reader.GetInt32("idPergunta"),
                UserId = reader.GetInt32("idUsuario"),
                Grade = reader.GetInt32("resposta"),
                Notes = reader.GetString("descricao"),
                Date = reader.GetDateTime("dataHora")
              };

              answersList.Add(answer);
            }
          }
        }

        return answersList;
      }
    }

    public async Task<List<Answer>> GetAnswers()
    {
      using(SqlConnection connection = _connection.GetConnectionString())
      {
        await connection.OpenAsync();

        string query = "SELECT * FROM  respostas";

        List<Answer> answersList = new List<Answer>();

        using(SqlCommand command = new SqlCommand(query, connection))
        {
          using(SqlDataReader reader = await command.ExecuteReaderAsync())
          {
            while(await reader.ReadAsync())
            {
              Answer answer = new Answer
              {
                Id = reader.GetInt32("id"),
                QuestionId = reader.GetInt32("idPergunta"),
                UserId = reader.GetInt32("idUsuario"),
                Grade = reader.GetInt32("resposta"),
                Notes = reader.GetString("descricao"),
                Date = reader.GetDateTime("dataHora")
              };

              answersList.Add(answer);
            }
          }
        }
        
        return answersList;
      }
    }

    public async Task<bool> DeleteAnswersByQuestionId(int questionId)
    {
      using (SqlConnection connection = _connection.GetConnectionString())
      {
        await connection.OpenAsync();

        string query = "DELETE * FROM respostas WHERE idPergunta = @QuestionId";

        List<Answer> answersList = new List<Answer>();

        using (SqlCommand command = new SqlCommand(query, connection))
        {
          command.Parameters.AddWithValue("@QuestionId", questionId);

          if (await command.ExecuteNonQueryAsync() > 0)
          {
            return true;
          }
          else
          {
            return false;
          }
        }
      }
    }
  }
}
