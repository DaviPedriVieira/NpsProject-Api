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
      User? repeatedNameUser = users.Find(User => User.Name == user.Name);

      if (repeatedNameUser != null)
      {
        throw new ArgumentException(user.Name, "Nome de usuário já existente!");
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
        throw new Exception("Não há perguntas cadastradas!");
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

    public async Task<string> Login(string name, string password)
    {
      List<User> usersList = await _userRepository.GetUsers();
      User? user = usersList.Find(user => user.Name == name && user.Password == password);

      if (user is null)
      {
        throw new ArgumentException("name, password", "Não há usuários com este nome e senha!");
      }

      List<Claim> claims = new List<Claim> {
         new Claim(ClaimTypes.Name, user.Name),
         new Claim(ClaimTypes.Role, user.Type.ToString())
      };

      ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

      await _httpContextAccessor.HttpContext.SignInAsync(
        CookieAuthenticationDefaults.AuthenticationScheme,
        new ClaimsPrincipal(claimsIdentity),
        new AuthenticationProperties
        {
          AllowRefresh = true,
          IsPersistent = true,
        });

      return "Login realizado!";
    }

    public async Task<string> Logout()
    {
      await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

      return "Logout realizado!";
    }

    public async Task<List<User>> GetPromoters()
    {
      List<Answer> answersList = await _answersRepository.GetAnswers();

      List<Answer> promoterAnswers = answersList.Where(answer => answer.Grade >= 9).ToList();

      List<User> promoters = new List<User>();

      foreach (Answer answer in promoterAnswers)
      {
        User? user = await _userRepository.GetUserById(answer.UserId);

        if (user != null)
        {
          if (promoters.Find(x => x.Name == user.Name) == null)
          {
            promoters.Add(user);
          }
        }
      }

      return promoters;
    }

    public async Task<List<User>> GetPassives()
    {
      List<Answer> answersList = await _answersRepository.GetAnswers();

      List<Answer> passiveAnswers = answersList.Where(answer => answer.Grade > 6 && answer.Grade < 9).ToList();

      List<User> passives = new List<User>();

      foreach (Answer answer in passiveAnswers)
      {
        User? user = await _userRepository.GetUserById(answer.UserId);

        if (user != null)
        {
          if (passives.Find(x => x.Name == user.Name) == null)
          {
            passives.Add(user);
          }
        }
      }

      return passives;
    }

    public async Task<List<User>> GetDetractors()
    {
      List<Answer> answersList = await _answersRepository.GetAnswers();

      List<Answer> detractorAnswers = answersList.Where(answer => answer.Grade <= 6).ToList();

      List<User> detractors = new List<User>();

      foreach (Answer answer in detractorAnswers)
      {
        User? user = await _userRepository.GetUserById(answer.UserId);

        if (user != null)
        {
          if (detractors.Find(x => x.Name == user.Name) == null)
          {
            detractors.Add(user);
          }
        }
      }

      return detractors;
    }
  }
}
