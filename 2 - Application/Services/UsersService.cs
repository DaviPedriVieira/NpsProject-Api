using NpsApi._3___Domain.CommandHandlers;
using NpsApi.Models;

namespace NpsApi.Application.Services
{
  public class UsersService
  {
    private readonly UsersCommandHandler _userCommandHandler;

    public UsersService(UsersCommandHandler usersCommandHandler)
    {
      _userCommandHandler = usersCommandHandler;
    }

    public async Task<User> CreateUser(User user)
    {
      return await _userCommandHandler.CreateUser(user);
    }

    public async Task<List<User>> GetUsers()
    {
      return await _userCommandHandler.GetUsers();
    }

    public async Task<User> GetUserById(int id)
    {
      return await _userCommandHandler.GetUserById(id);
    }
  }
}
