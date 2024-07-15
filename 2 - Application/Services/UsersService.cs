using NpsApi._3___Domain.Enums;
using NpsApi.Models;
using NpsApi.Repositories;

namespace NpsApi.Application.Services
{
  public class UsersService
  {
    private readonly UsersRepository _userRepository;

    public UsersService(UsersRepository userRepository)
    {
      _userRepository = userRepository;
    }

    public async Task<User> CreateUser(User user)
    {
      if (string.IsNullOrWhiteSpace(user.Name) || string.IsNullOrWhiteSpace(user.Password))
      {
        throw new ArgumentException("O nome/senha não podem ser vazios!");
      }

      if (Enum.TryParse<UserType>($"{user.Type}", true, out _)) 
      {
        throw new ArgumentException("Tipo inexistente!");
      }

      List<User> users = await GetUsers();
      User? repeatedNameUser = users.FirstOrDefault(User => User.Name == user.Name);

      if(repeatedNameUser != null)
      {
        throw new ArgumentException("Nome de usuário já existente!");
      }

      User newUser = await _userRepository.CreateUser(user);

      return newUser;
    }

    public async Task<List<User>> GetUsers()
    {
      List<User> usersList = await _userRepository.GetUsers();

      if (!usersList.Any())
      {
        throw new ArgumentException("Não há perguntas cadastradas!");
      }

      return usersList;
    }
  }
}
