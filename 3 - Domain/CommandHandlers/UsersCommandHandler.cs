using NpsApi._3___Domain.Enums;
using NpsApi.Models;
using NpsApi.Repositories;

namespace NpsApi._3___Domain.CommandHandlers
{
  public class UsersCommandHandler
  {
    private readonly UsersRepository _userRepository;

    public UsersCommandHandler(UsersRepository userRepository)
    {
      _userRepository = userRepository;
    }

    public async Task<User> CreateUser(User user)
    {
      if (string.IsNullOrWhiteSpace(user.Name) || string.IsNullOrWhiteSpace(user.Password))
      {
        throw new ArgumentNullException("O nome/senha não podem ser vazios!");
      }

      List<User> users = await GetUsers();
      User? repeatedNameUser = users.Find(User => User.Name == user.Name);

      if (repeatedNameUser != null)
      {
        throw new Exception("Nome de usuário já existente!");
      }

      return await _userRepository.CreateUser(user);
    }

    public async Task<List<User>> GetUsers()
    {
      List<User> usersList = await _userRepository.GetUsers();

      if (!usersList.Any())
      {
        throw new Exception("Não há perguntas cadastradas!");
      }

      return usersList;
    }

    public async Task<User> GetUserById(int id)
    {
      User? user = await _userRepository.GetUserById(id);

      if(user == null)
      {
        throw new KeyNotFoundException($"Não foi encontrado nenhum usuário com o Id = {id}!");
      }

      return user;
    }
  }
}
