using NpsApi._3___Domain.Models.Enums;
using NpsApi.Data;
using NpsApi.Models;
using System.Data;
using System.Data.SqlClient;

namespace NpsApi.Repositories
{
  public class UsersRepository
  {
    private readonly DatabaseConnection _databaseConnection;

    public UsersRepository(DatabaseConnection sqlConnection)
    {
      _databaseConnection = sqlConnection;
    }

    public async Task<User> CreateUser(User user)
    {
      using (SqlConnection sqlConnection = _databaseConnection.GetConnectionString())
      {
        await sqlConnection.OpenAsync();

        string query = $"INSERT INTO usuarios VALUES ('{user.Name}', '{user.Password}', '{user.Type}'); SELECT SCOPE_IDENTITY();";

        SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

        user.Id = Convert.ToInt32(await sqlCommand.ExecuteScalarAsync());
        return user;
      }
    }

    public async Task<List<User>> GetUsers()
    {
      using (SqlConnection sqlConnection = _databaseConnection.GetConnectionString())
      {
        await sqlConnection.OpenAsync();

        string query = "SELECT * FROM usuarios";

        List<User> usersList = new List<User>();

        SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

        SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

        while (await sqlDataReader.ReadAsync())
        {
          User user = new User
          {
            Id = sqlDataReader.GetInt32("id"),
            Name = sqlDataReader.GetString("nome"),
            Password = sqlDataReader.GetString("senha"),
            Type = Enum.Parse<UserType>(sqlDataReader.GetString("tipo")),
          };

          usersList.Add(user);
        }

        return usersList;
      }
    }

    public async Task<User?> GetUserById(int id)
    {
      using (SqlConnection sqlConnection = _databaseConnection.GetConnectionString())
      {
        await sqlConnection.OpenAsync();

        string query = $"SELECT * FROM usuarios WHERE id = {id}";

        User? user = null;

        SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

        SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

        if (await sqlDataReader.ReadAsync())
        {
          user = new User
          {
            Id = sqlDataReader.GetInt32("id"),
            Name = sqlDataReader.GetString("nome"),
            Password = sqlDataReader.GetString("senha"),
            Type = Enum.Parse<UserType>(sqlDataReader.GetString("tipo")),
          };
        }

        return user;
      }
    }
  }
}
