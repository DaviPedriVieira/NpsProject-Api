using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using NpsApi.Models;
using NpsApi.Repositories;
using System.Security.Claims;

namespace NpsApi._3___Domain.CommandHandlers
{
  public class UsersCommandHandler
  {
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UsersRepository _userRepository;
    private readonly AnswersRepository _answersRepository;

    public UsersCommandHandler(UsersRepository userRepository, IHttpContextAccessor httpContextAccessor, AnswersRepository answersRepository)
    {
      _httpContextAccessor = httpContextAccessor;
      _userRepository = userRepository;
      _answersRepository = answersRepository;
    }

    public async Task<User> CreateUser(User user)
    {
      if (string.IsNullOrWhiteSpace(user.Name) || string.IsNullOrWhiteSpace(user.Password))
      {
        throw new ArgumentNullException(user.Name, "O nome/senha não podem ser vazios!");
      }

      List<User> users = await GetUsers();
      User? repeatedNameUser = users.Find(u => u.Name == user.Name);

      if (repeatedNameUser != null)
      {
        throw new ArgumentException("Nome de usuário já existente!", user.Name);
      }

      User createdUser = await _userRepository.CreateUser(user);

      await Login(createdUser.Name, createdUser.Password);

      return createdUser;
    }

    public async Task<List<User>> GetUsers()
    {
      List<User> usersList = await _userRepository.GetUsers();

      if (usersList.Count == 0)
      {
        throw new Exception("Não há usuários cadastrados!");
      }

      return usersList;
    }

    public async Task<User> GetUserById(int id)
    {
      User? user = await _userRepository.GetUserById(id);

      if (user is null)
      {
        throw new KeyNotFoundException($"Não foi encontrado nenhum usuário com o Id = {id}!");
      }

      return user;
    }

    public async Task<string> Login(string username, string password)
    {
      if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
      {
        return "Usuário já logado!";
      }

      List<User> usersList = await _userRepository.GetUsers();
      User? user = usersList.Find(user => user.Name == username && user.Password == password);

      if (user is null)
      {
        throw new ArgumentException("Não há usuários com este nome e senha!");
      }

      List<Claim> claims = new List<Claim> {
         new Claim(ClaimTypes.Name, user.Name),
         new Claim(ClaimTypes.Role, user.Type.ToString()),
         new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
      };

      ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

      await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

      return "Login realizado!";
    }

    public async Task<string> Logout()
    {
      await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

      return "Logout realizado!";
    }

    public async Task<List<User>> GetUsersAccordingAnswersGradeRange(int minValue, int maxValue)
    {
      List<User> users = new List<User>();

      List<Answer> answers = await _answersRepository.GetAnswers();

      List<Answer> answersFilteredAccordingGrade = answers.Where(answer => answer.Grade >= minValue && answer.Grade <= maxValue).ToList();

      foreach (Answer answer in answersFilteredAccordingGrade)
      {
        User? user = await _userRepository.GetUserById(answer.UserId);

        if (user != null && users.Find(x => x.Name == user.Name) is null)
        {
          users.Add(user);
        }
      }

      return users;
    }
  }
}
