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

    public async Task<int> SubmitAnswer(Answers answer)
    {
      using(SqlConnection connection = _connection.GetConnectionString())
      {
        await connection.OpenAsync();

        string query = "INSERT INTO respostas (idPergunta, idUsuario, resposta, descricao) VALUES (@idPergunta, @idUsuario, @resposta, @descricao); SELECT SCOPE_IDENTITY();";

        using(SqlCommand command = new SqlCommand (query, connection))
        {
          command.Parameters.AddWithValue("@idPergunta", answer.QuestionId);
          command.Parameters.AddWithValue("@idUsuario", answer.UserId);
          command.Parameters.AddWithValue("@resposta", answer.Grade);
          command.Parameters.AddWithValue("@descricao", answer.Description);

          var id = await command.ExecuteScalarAsync();
          return Convert.ToInt32(id);
        }
      }
    }

    public async Task<List<Answers>?> GetAnswersByClientId(int userId)
    {
      using (SqlConnection connection = _connection.GetConnectionString())
      {
        await connection.OpenAsync();

        string query = "SELECT * FROM  respostas WHERE idUsuario = @idUsuario";

        List<Answers> answersList = new List<Answers>();

        using (SqlCommand command = new SqlCommand(query, connection))
        {
          command.Parameters.AddWithValue("@idUsuario", userId);

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
                Description = reader.GetString("descricao")
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
                Description = reader.GetString("descricao")
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

        string query = "SELECT * FROM respostas WHERE idPergunta = @idPergunta";

        List<Answers> answersList = new List<Answers>();

        using (SqlCommand command = new SqlCommand(query, connection))
        {
          command.Parameters.AddWithValue("@idPergunta", questionId);

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
                Description = reader.GetString("descricao"),
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
