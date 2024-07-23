using NpsApi.Data;
using NpsApi.Models;
using System.Data;
using System.Data.SqlClient;

namespace NpsApi.Repositories
{
  public class AnswersRepository
  {
    private readonly DatabaseConnection _connection;

    public AnswersRepository(DatabaseConnection sqlConnection)
    {
      _connection = sqlConnection;
    }

    public async Task<Answer> SubmitAnswer(Answer answer)
    {
      using(SqlConnection sqlConnection = _connection.GetConnectionString())
      {
        await sqlConnection.OpenAsync();

        answer.Date = DateTime.Now;

        string query = "INSERT INTO respostas (idPergunta, idUsuario, resposta, descricao, dataHora) VALUES (@QuestionId, @UserId, @Grade, @Description, @Date); SELECT SCOPE_IDENTITY();";

        using(SqlCommand sqlCommand = new SqlCommand (query, sqlConnection))
        {
          sqlCommand.Parameters.AddWithValue("@QuestionId", answer.QuestionId);
          sqlCommand.Parameters.AddWithValue("@UserId", answer.UserId);
          sqlCommand.Parameters.AddWithValue("@Grade", answer.Grade);
          sqlCommand.Parameters.AddWithValue("@Description", answer.Description);
          sqlCommand.Parameters.AddWithValue("@Date", answer.Date);

          answer.Id = Convert.ToInt32(await sqlCommand.ExecuteScalarAsync());
          return answer;
        }
      }
    }

    public async Task<List<Answer>> GetAnswersByUserId(int userId)
    {
      using (SqlConnection sqlConnection = _connection.GetConnectionString())
      {
        await sqlConnection.OpenAsync();

        string query = "SELECT * FROM  respostas WHERE idUsuario = @UserId";

        List<Answer> answersList = new List<Answer>();

        using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
        {
          sqlCommand.Parameters.AddWithValue("@UserId", userId);

          using (SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync())
          {
            while (await sqlDataReader.ReadAsync())
            {
              Answer answer = new Answer
              {
                Id = sqlDataReader.GetInt32("id"),
                QuestionId = sqlDataReader.GetInt32("idPergunta"),
                UserId = sqlDataReader.GetInt32("idUsuario"),
                Grade = sqlDataReader.GetInt32("resposta"),
                Description = sqlDataReader.GetString("descricao"),
                Date = sqlDataReader.GetDateTime("dataHora"),
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
      using(SqlConnection sqlConnection = _connection.GetConnectionString())
      {
        await sqlConnection.OpenAsync();

        string query = "SELECT * FROM  respostas";

        List<Answer> answersList = new List<Answer>();

        using(SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
        {
          using(SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync())
          {
            while(await sqlDataReader.ReadAsync())
            {
              Answer answer = new Answer
              {
                Id = sqlDataReader.GetInt32("id"),
                QuestionId = sqlDataReader.GetInt32("idPergunta"),
                UserId = sqlDataReader.GetInt32("idUsuario"),
                Grade = sqlDataReader.GetInt32("resposta"),
                Description = sqlDataReader.GetString("descricao"),
                Date = sqlDataReader.GetDateTime("dataHora"),
              };

              answersList.Add(answer);
            }
          }
        }
        
        return answersList;
      }
    }

    public async Task<Answer?> GetAnswerById(int id)
    {
      using (SqlConnection sqlConnection = _connection.GetConnectionString())
      {
        await sqlConnection.OpenAsync();

        string query = "SELECT * FROM respostas WHERE id = @Id";

        Answer? answer = null;

        using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
        {
          sqlCommand.Parameters.AddWithValue("@Id", id);

          using (SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync())
          {
            if (await sqlDataReader.ReadAsync())
            {
              answer = new Answer
              {
                Id = sqlDataReader.GetInt32("id"),
                QuestionId = sqlDataReader.GetInt32("idPergunta"),
                UserId = sqlDataReader.GetInt32("idUsuario"),
                Grade = sqlDataReader.GetInt32("resposta"),
                Description = sqlDataReader.GetString("descricao"),
                Date = sqlDataReader.GetDateTime("dataHora"),
              };
            }

            return answer;
          }
        }
      }
    }

    public async Task DeleteAnswersByQuestionId(int questionId)
    {
      using (SqlConnection sqlConnection = _connection.GetConnectionString())
      {
        await sqlConnection.OpenAsync();

        string query = "DELETE FROM respostas WHERE idPergunta = @QuestionId";

        List<Answer> answersList = new List<Answer>();

        using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
        {
          sqlCommand.Parameters.AddWithValue("@QuestionId", questionId);
        }
      }
    }
  }
}
