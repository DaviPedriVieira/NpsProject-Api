using NpsApi.Data;
using NpsApi.Models;
using System.Data;
using System.Data.SqlClient;

namespace NpsApi.Repositories
{
  public class FormsRepository
  {
    private readonly DatabaseConnection _databaseConnection;

    public FormsRepository(DatabaseConnection sqlConnection)
    {
      _databaseConnection = sqlConnection;
    }

    public async Task<Form> CreateForm(Form form)
    {
      using (SqlConnection sqlConnection = _databaseConnection.GetConnectionString())
      {
        await sqlConnection.OpenAsync();

        string query = $"INSERT INTO formularios VALUES ({form.GroupId}, '{form.Name}'); SELECT SCOPE_IDENTITY();";

        SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

        form.Id  = Convert.ToInt32(await sqlCommand.ExecuteScalarAsync());
        return form;
      }
    }

    public async Task<Form?> GetFormById(int id)
    {
      using (SqlConnection sqlConnection = _databaseConnection.GetConnectionString())
      {
        await sqlConnection.OpenAsync();

        string query = $"SELECT * FROM formularios WHERE id = {id}";

        Form? form = null;

        SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

        SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

        if (await sqlDataReader.ReadAsync())
        {
          form = new Form
          {
            Id = sqlDataReader.GetInt32("id"),
            GroupId = sqlDataReader.GetInt32("idGrupo"),
            Name = sqlDataReader.GetString("nome"),
            Questions = new List<Question>(),
          };
        }

        return form;
      }
    }

    public async Task<List<Form>> GetForms()
    {
      using (SqlConnection sqlConnection = _databaseConnection.GetConnectionString())
      {
        await sqlConnection.OpenAsync();

        string query = "SELECT * FROM formularios";

        List<Form> formsList = new List<Form>();

        SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

        SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

        while (await sqlDataReader.ReadAsync())
        {
          Form form = new Form
          {
            Id = sqlDataReader.GetInt32("id"),
            GroupId = sqlDataReader.GetInt32("idGrupo"),
            Name = sqlDataReader.GetString("nome"),
            Questions = new List<Question>(),
          };

          formsList.Add(form);
        }

        return formsList;
      }
    }

    public async Task<bool> DeleteForm(int id)
    {
      using (SqlConnection sqlConnection = _databaseConnection.GetConnectionString())
      {
        await sqlConnection.OpenAsync();

        string query = $"DELETE FROM formularios WHERE id = {id}";

        SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

        return await sqlCommand.ExecuteNonQueryAsync() > 0;
      }
    }

    public async Task<bool> DeleteForms(List<int> ids)
    {
      using (SqlConnection sqlConnection = _databaseConnection.GetConnectionString())
      {
        await sqlConnection.OpenAsync();

        string idsString = string.Join(", ", ids);

        string query = $"DELETE FROM formularios WHERE id in ({idsString})";

        SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

        return await sqlCommand.ExecuteNonQueryAsync() > 0;
      }
    }

    public async Task<bool> UpdateForm(int id, string newName)
    {
      using (SqlConnection sqlConnection = _databaseConnection.GetConnectionString())
      {
        await sqlConnection.OpenAsync();

        string query = $"UPDATE formularios SET nome = '{newName}' WHERE id = {id}";

        SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

        return await sqlCommand.ExecuteNonQueryAsync() > 0;
      }
    }

    public async Task<List<Form>> GetFormsByGroupId(int groupId)
    {
      using (SqlConnection sqlConnection = _databaseConnection.GetConnectionString())
      {
        await sqlConnection.OpenAsync();

        string query = $"SELECT * FROM formularios WHERE idGrupo = {groupId}";

        List<Form> formsList = new List<Form>();

        SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

        SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

        while (await sqlDataReader.ReadAsync())
        {
          Form form = new Form
          {
            Id = sqlDataReader.GetInt32("id"),
            GroupId = sqlDataReader.GetInt32("idGrupo"),
            Name = sqlDataReader.GetString("nome"),
            Questions = new List<Question>(),
          };

          formsList.Add(form);
        }

        return formsList;
      }
    }

    public async Task<List<int>> GetFormsByGroupIds(int groupId)
    {
      using (SqlConnection sqlConnection = _databaseConnection.GetConnectionString())
      {
        await sqlConnection.OpenAsync();

        string query = $"SELECT * FROM formularios WHERE idGrupo = {groupId}";

        List<int> formsIdList = new List<int>();

        SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

        SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

        while (await sqlDataReader.ReadAsync())
        {
          int Id = sqlDataReader.GetInt32("id");

          formsIdList.Add(Id);
        }

        return formsIdList;
      }
    }
  }
}
