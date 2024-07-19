using Microsoft.OpenApi.Extensions;
using NpsApi._3___Domain.Models.Enums;
using NpsApi.Data;
using NpsApi.Models;
using System.Data;
using System.Data.SqlClient;

namespace NpsApi.Repositories
{
  public class UsersRepository
  {
    private readonly DataBaseConnection _connection;

    public UsersRepository(DataBaseConnection connection)
    {
      _connection = connection;
    }

    public async Task<User> CreateUser(User user)
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

          user.Id = Convert.ToInt32(await command.ExecuteScalarAsync());
          return user;
        }
      }
    }

    public async Task<List<User>> GetUsers()
    {
      using (SqlConnection connection = _connection.GetConnectionString())
      {
        await connection.OpenAsync();

        string query = "SELECT * FROM usuarios";

        List<User> usersList = new List<User>();

        using (SqlCommand command = new SqlCommand(query, connection))
        {
          using (SqlDataReader reader = await command.ExecuteReaderAsync())
          {
            while (await reader.ReadAsync())
            {
              User user = new User
              {
                Id = reader.GetInt32("id"),
                Name = reader.GetString("nome"),
                Password = reader.GetString("senha"),
                Type = Enum.Parse<UserType>(reader.GetString("tipo")),
              };

              usersList.Add(user);
            }
          }
        }

        return usersList;
      }
    }

    public async Task<User?> GetUserById(int id)
    {
      using (SqlConnection connection = _connection.GetConnectionString())
      {
        await connection.OpenAsync();

        string query = "SELECT * FROM usuarios WHERE id = @Id";

        User? user = null;

        using (SqlCommand command = new SqlCommand(query, connection))
        {
          command.Parameters.AddWithValue("@Id", id);

          using (SqlDataReader reader = await command.ExecuteReaderAsync())
          {
            if (await reader.ReadAsync())
            {
              user = new User
              {
                Id = reader.GetInt32("id"),
                Name = reader.GetString("nome"),
                Password = reader.GetString("senha"),
                Type = Enum.Parse<UserType>(reader.GetString("tipo")),
              };
            }

            return user;
          }
        }
      }
    }

  }
}
