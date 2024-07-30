using NpsApi.Data;
using NpsApi.Models;
using System.Data;
using System.Data.SqlClient;

namespace NpsApi.Repositories
{
  public class AnswersRepository
  {
    private readonly DatabaseConnection _databaseConnection;

    public AnswersRepository(DatabaseConnection sqlConnection)
    {
      _databaseConnection = sqlConnection;
    }

    public async Task<List<Answer>> SubmitAnswers(List<Answer> answers)
    {
      using (SqlConnection sqlConnection = _databaseConnection.GetConnectionString())
      {
        await sqlConnection.OpenAsync();

        DataTable answersTable = new DataTable();
        answersTable.TableName = "dbo.respostas";
        answersTable.Columns.Add("questionId", typeof(int));
        answersTable.Columns.Add("userId", typeof(int));
        answersTable.Columns.Add("grade", typeof(int));
        answersTable.Columns.Add("description", typeof(string));
        answersTable.Columns.Add("date", typeof(DateTime));

        foreach (Answer answer in answers)
        {
          answer.Date = DateTime.Now;
          answersTable.Rows.Add(answer.QuestionId, answer.UserId, answer.Grade, answer.Description, answer.Date);
        }

        SqlBulkCopy bulk = new SqlBulkCopy(sqlConnection);
        bulk.DestinationTableName = answersTable.TableName;
        bulk.ColumnMappings.Add("questionId", "idPergunta");
        bulk.ColumnMappings.Add("userId", "idUsuario");
        bulk.ColumnMappings.Add("grade", "resposta");
        bulk.ColumnMappings.Add("description", "descricao");
        bulk.ColumnMappings.Add("date", "dataHora");

        await bulk.WriteToServerAsync(answersTable);
        return answers;
      }
    }

    public async Task<List<Answer>> GetAnswersByUserId(int userId)
    {
      using (SqlConnection sqlConnection = _databaseConnection.GetConnectionString())
      {
        await sqlConnection.OpenAsync();

        string query = $"SELECT * FROM respostas WHERE idUsuario = {userId}";

        List<Answer> answersList = new List<Answer>();

        using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
        {
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
      using (SqlConnection sqlConnection = _databaseConnection.GetConnectionString())
      {
        await sqlConnection.OpenAsync();

        string query = "SELECT * FROM respostas";

        List<Answer> answersList = new List<Answer>();

        using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
        {
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

    public async Task<Answer?> GetAnswerById(int id)
    {
      using (SqlConnection sqlConnection = _databaseConnection.GetConnectionString())
      {
        await sqlConnection.OpenAsync();

        string query = $"SELECT * FROM respostas WHERE id = {id}";

        Answer? answer = null;

        using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
        {
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

    public async Task<bool> DeleteAnswersByQuestionId(int questionId)
    {
      using (SqlConnection sqlConnection = _databaseConnection.GetConnectionString())
      {
        await sqlConnection.OpenAsync();

        string query = $"DELETE FROM respostas WHERE idPergunta = {questionId}";

        List<Answer> answersList = new List<Answer>();

        using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
        {
          return await sqlCommand.ExecuteNonQueryAsync() > 0;
        }
      }
    }
  }
}
