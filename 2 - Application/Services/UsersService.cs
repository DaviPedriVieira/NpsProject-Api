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

    public async Task<Users> CreateUser(Users user)
    {
      if (string.IsNullOrWhiteSpace(user.Name) || string.IsNullOrWhiteSpace(user.Password))
      {
        throw new ArgumentException("O nome/senha não podem ser vazios!");
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
