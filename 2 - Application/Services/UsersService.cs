using NpsApi.Data;
using NpsApi.Models;
using NpsApi.Repositories;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace NpsApi.Application.Services
{
  public class UsersService
  {
    private readonly UsersRepository _userRepository;

    public UsersService(UsersRepository userRepository)
    {
      _userRepository = userRepository;
    }

    public async Task<Users> Create(Users user)
    {
      if (string.IsNullOrWhiteSpace(user.Name) || string.IsNullOrWhiteSpace(user.Password))
      {
        throw new ArgumentException("O nome/senha não podem ser vazios!");
      }

      Users newUser = new Users
      {
        Name = user.Name,
        Password = user.Password,
        Type = user.Type,
      };

      newUser.Id = await _userRepository.Create(user);

      return newUser;
    }

    public async Task<List<Users>> Get()
    {
      List<Users> usersList = await _userRepository.Get();

      if (!usersList.Any())
      {
        throw new ArgumentException("Não há perguntas cadastradas!");
      }

      return usersList;
    }
  }
}
