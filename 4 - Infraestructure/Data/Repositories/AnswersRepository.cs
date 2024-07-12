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

    public async Task<Answers> SubmitAnswer(Answers answer)
    {
      using(SqlConnection connection = _connection.GetConnectionString())
      {
        await connection.OpenAsync();

        string query = "INSERT INTO respostas (idPergunta, idUsuario, resposta, descricao) VALUES (@QuestionId, @UserId, @Grade, @Notes); SELECT SCOPE_IDENTITY();";

        using(SqlCommand command = new SqlCommand (query, connection))
        {
          command.Parameters.AddWithValue("@QuestionId", answer.QuestionId);
          command.Parameters.AddWithValue("@UserId", answer.UserId);
          command.Parameters.AddWithValue("@Grade", answer.Grade);
          command.Parameters.AddWithValue("@Notes", answer.Notes);

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

    public async Task<List<Answers>> GetAnswersByClientId(int userId)
    {
      List<Answers> answersList = new List<Answers>();

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
              Answers answer = new Answers
              {
                Id = reader.GetInt32("id"),
                QuestionId = reader.GetInt32("idPergunta"),
                UserId = reader.GetInt32("idUsuario"),
                Grade = reader.GetInt32("resposta"),
                Notes = reader.GetString("descricao")
              };

              answersList.Add(answer);
            }
          }
        }

        return answersList;
      }
    }

    public async Task<List<Answers>> GetAnswers()
    {
      using(SqlConnection connection = _connection.GetConnectionString())
      {
        await connection.OpenAsync();

        string query = "SELECT * FROM  respostas";

        List<Answers> answersList = new List<Answers>();

        using(SqlCommand command = new SqlCommand(query, connection))
        {
          using(SqlDataReader reader = await command.ExecuteReaderAsync())
          {
            while(await reader.ReadAsync())
            {
              Answers answer = new Answers
              {
                Id = reader.GetInt32("id"),
                QuestionId = reader.GetInt32("idPergunta"),
                UserId = reader.GetInt32("idUsuario"),
                Grade = reader.GetInt32("resposta"),
                Notes = reader.GetString("descricao")
              };

              answersList.Add(answer);
            }
          }
        }

        return answersList;
      }
    }

    public async Task<List<Answers>> GetAnswersByQuestionId(int questionId)
    {
      using (SqlConnection connection = _connection.GetConnectionString())
      {
        await connection.OpenAsync();

        string query = "SELECT * FROM respostas WHERE idPergunta = @QuestionId";

        List<Answers> answersList = new List<Answers>();

        using (SqlCommand command = new SqlCommand(query, connection))
        {
          command.Parameters.AddWithValue("@QuestionId", questionId);

          using (SqlDataReader reader = await command.ExecuteReaderAsync())
          {
            while (await reader.ReadAsync())
            {
              Answers answer = new Answers
              {
                Id = reader.GetInt32("id"),
                QuestionId = reader.GetInt32("idPergunta"),
                UserId = reader.GetInt32("idUsuario"),
                Grade = reader.GetInt32("resposta"),
                Notes = reader.GetString("descricao"),
              };

              answersList.Add(answer);
            }
          }
        }

        return answersList;
      }
    }
  }
}
