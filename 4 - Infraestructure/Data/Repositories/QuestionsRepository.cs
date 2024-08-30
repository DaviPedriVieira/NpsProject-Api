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

        public async Task<List<Question>> CreateQuestion(List<Question> questions)
        {
            using (SqlConnection sqlConnection = _databaseConnection.GetConnectionString())
            {
                await sqlConnection.OpenAsync();

                DataTable questionsTable = new DataTable();
                questionsTable.TableName = "perguntas";
                questionsTable.Columns.Add("formId", typeof(int));
                questionsTable.Columns.Add("content", typeof(string));

                foreach (Question question in questions)
                {
                    questionsTable.Rows.Add(question.FormId, question.Content);
                }

                SqlBulkCopy sqlBulk = new SqlBulkCopy(sqlConnection);
                sqlBulk.DestinationTableName = questionsTable.TableName;
                sqlBulk.ColumnMappings.Add("formId", "idFormulario");
                sqlBulk.ColumnMappings.Add("content", "conteudo");


                sqlBulk.WriteToServer(questionsTable);
                return questions;
            }
        }


        public async Task<Question?> GetQuestionById(int id)
        {
            using (SqlConnection sqlConnection = _databaseConnection.GetConnectionString())
            {
                await sqlConnection.OpenAsync();

                string query = $"SELECT * FROM perguntas WHERE id = {id}";

                Question? question = null;

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

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

        public async Task<List<Question>> GetQuestions()
        {
            using (SqlConnection sqlConnection = _databaseConnection.GetConnectionString())
            {
                await sqlConnection.OpenAsync();

                string query = "SELECT * FROM perguntas";

                List<Question> questionsList = new List<Question>();

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

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

                return questionsList;
            }
        }

        public async Task<bool> DeleteQuestion(int id)
        {
            using (SqlConnection sqlConnection = _databaseConnection.GetConnectionString())
            {
                await sqlConnection.OpenAsync();

                string query = $"DELETE FROM perguntas WHERE id = {id}";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                return await sqlCommand.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<bool> UpdateQuestion(int id, string newName)
        {
            using (SqlConnection sqlConnection = _databaseConnection.GetConnectionString())
            {
                await sqlConnection.OpenAsync();

                string query = $"UPDATE perguntas SET conteudo = '{newName}' WHERE id = {id}";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                return await sqlCommand.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<List<Question>> GetQuestionsByFormId(int formId)
        {
            using (SqlConnection sqlConnection = _databaseConnection.GetConnectionString())
            {
                await sqlConnection.OpenAsync();

                string query = $"SELECT * FROM perguntas WHERE idFormulario = {formId}";

                List<Question> questionsList = new List<Question>();

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

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

                return questionsList;
            }
        }

        public async Task<List<int>> GetQuestionsIdByFormIds(List<int> formIds)
        {
            using (SqlConnection sqlConnection = _databaseConnection.GetConnectionString())
            {
                await sqlConnection.OpenAsync();

                string idsString = string.Join(", ", formIds);

                string query = $"SELECT * FROM perguntas WHERE idFormulario in ({idsString})";

                List<int> questionIdsList = new List<int>();

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

                while (await sqlDataReader.ReadAsync())
                {
                    int Id = sqlDataReader.GetInt32("id");

                    questionIdsList.Add(Id);
                }

                return questionIdsList;
            }
        }

        public async Task<List<int>> GetQuestionsIdByFormIds(int formId)
        {
            using (SqlConnection sqlConnection = _databaseConnection.GetConnectionString())
            {
                await sqlConnection.OpenAsync();

                string query = $"SELECT * FROM perguntas WHERE idFormulario = {formId}";

                List<int> questionIdsList = new List<int>();

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

                while (await sqlDataReader.ReadAsync())
                {
                    questionIdsList.Add(sqlDataReader.GetInt32("id"));
                }

                return questionIdsList;
            }
        }

        public async Task<bool> DeleteQuestions(List<int> ids)
        {
            using (SqlConnection sqlConnection = _databaseConnection.GetConnectionString())
            {
                await sqlConnection.OpenAsync();

                string idsString = string.Join(", ", ids);

                string query = $"DELETE FROM perguntas WHERE id in ({idsString})";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                return await sqlCommand.ExecuteNonQueryAsync() > 0;
            }
        }
    }
}
