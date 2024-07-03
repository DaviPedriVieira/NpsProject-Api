using NpsApi.Data;
using NpsApi.Models;
using System.Data;
using System.Data.SqlClient;

namespace NpsApi.Repositories
{
  public class UsersRepository
  {
    private readonly DataBaseConnection _databaseConnection;

    public UsersRepository(DataBaseConnection connection)
    {
      _databaseConnection = connection;
    }

    public async Task<int> Create(Users user)
    {
      using (SqlConnection connection = _databaseConnection.GetConnectionString())
      {
        await connection.OpenAsync();

        string query = "INSERT INTO usuarios (nome, senha, tipo) VALUES (@nome, @senha, @tipo); SELECT SCOPE_IDENTITY();";

        using (SqlCommand command = new SqlCommand(query, connection))
        {
          command.Parameters.AddWithValue("@nome", user.Name);
          command.Parameters.AddWithValue("@senha", user.Password);
          command.Parameters.AddWithValue("@tipo", user.Type.ToString());

          var id = await command.ExecuteScalarAsync();
          return Convert.ToInt32(id);
        }
      }
    }

    public async Task<List<Users>> Get()
    {
      using (SqlConnection connection = _databaseConnection.GetConnectionString())
      {
        await connection.OpenAsync();

        string query = "SELECT * FROM usuarios";

        List<Users> allUsers = new List<Users>();

        using (SqlCommand command = new SqlCommand(query, connection))
        {
          using (SqlDataReader reader = await command.ExecuteReaderAsync())
          {
            while (await reader.ReadAsync())
            {
              Users user = new Users
              {
                Id = reader.GetInt32("id"),
                Name = reader.GetString("nome"),
                Password = reader.GetString("senha"),
                Type = Enum.Parse<Users.UserType>(reader.GetString("tipo")),
              };

              allUsers.Add(user);
            }
          }
        }
        return allUsers;
      }
    }
  }
}
