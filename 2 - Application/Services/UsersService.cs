using NpsApi._3___Domain.Enums;
using NpsApi.Models;
using NpsApi.Repositories;
using System.Reflection.PortableExecutable;

namespace NpsApi.Application.Services
{
  public class UsersService
  {
    private readonly UsersRepository _userRepository;

    public UsersService(UsersRepository userRepository)
    {
      _userRepository = userRepository;
    }

    public async Task<Users> CreateUser(Users user)
    {
      if (string.IsNullOrWhiteSpace(user.Name) || string.IsNullOrWhiteSpace(user.Password))
      {
        throw new ArgumentException("O nome/senha não podem ser vazios!");
      }

      if (Enum.TryParse<UserType>($"{user.Type}", true, out _)) 
      {
        throw new ArgumentException("Tipo inexistente!");
      }

      List<Users> users = await GetUsers();
      Users? repeatedNameUser = users.FirstOrDefault(User => User.Name == user.Name);

      if(repeatedNameUser != null)
      {
        throw new ArgumentException("Nome de usuário já existente!");
      }

      Users newUser = await _userRepository.CreateUser(user);

      return newUser;
    }

    public async Task<List<Users>> GetUsers()
    {
      List<Users> usersList = await _userRepository.GetUsers();

      if (!usersList.Any())
      {
        throw new ArgumentException("Não há perguntas cadastradas!");
      }

      return usersList;
    }
  }
}
