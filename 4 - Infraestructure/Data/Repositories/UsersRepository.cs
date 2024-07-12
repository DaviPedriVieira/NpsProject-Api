using NpsApi.Data;
using NpsApi.Models;
using System.Data;
using System.Data.SqlClient;
using NpsApi._3___Domain.Enums;
using Microsoft.OpenApi.Extensions;

namespace NpsApi.Repositories
{
  public class UsersRepository
  {
    private readonly DataBaseConnection _connection;

    public UsersRepository(DataBaseConnection connection)
    {
      _connection = connection;
    }

    public async Task<Users> CreateUser(Users user)
    {
      using (SqlConnection connection = _connection.GetConnectionString())
      {
        await connection.OpenAsync();

        string query = "INSERT INTO usuarios (nome, senha, tipo) VALUES (@Name, @Password, @Type); SELECT SCOPE_IDENTITY();";

        using (SqlCommand command = new SqlCommand(query, connection))
        {
          command.Parameters.AddWithValue("@Name", user.Name);
          command.Parameters.AddWithValue("@Password", user.Password);
          command.Parameters.AddWithValue("@Type", user.Type.ToString());

          var id = await command.ExecuteScalarAsync();
          user.Id = Convert.ToInt32(id);

          return user;
        }
      }
    }

    public async Task<List<Users>> GetUsers()
    {
      using (SqlConnection connection = _connection.GetConnectionString())
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
                Type = Enum.Parse<UserType>(reader.GetString("tipo")),
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
