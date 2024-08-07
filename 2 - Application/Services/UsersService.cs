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

    public async Task<User> GetUserById(int id)
    {
      return await _userCommandHandler.GetUserById(id);
    }

    public async Task<bool> Login(string username, string password)
    {
      return await _userCommandHandler.Login(username, password);
    }

    public async Task<bool> Logout()
    {
      return await _userCommandHandler.Logout();
    }

    public async Task<List<User>> GetPromoters()
    {
      return await _userCommandHandler.GetUsersAccordingAnswersGradeRange(9, 10);
    }

    public async Task<List<User>> GetPassives()
    {
      return await _userCommandHandler.GetUsersAccordingAnswersGradeRange(7, 8);
    }

    public async Task<List<User>> GetDetractors()
    {
      return await _userCommandHandler.GetUsersAccordingAnswersGradeRange(0, 6);
    }
  }
}
