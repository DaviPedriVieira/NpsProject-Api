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
                throw new Exception("O nome/senha não podem ser vazios!");
            }

            User? foundUser = await _userRepository.GetUserByLogin(user.Name);

            if (foundUser != null)
            {
                throw new Exception("Nome de usuário já existente!");
            }

            User createdUser = await _userRepository.CreateUser(user);

            return createdUser;
        }

        public async Task<User> GetUserById(int id)
        {
            User? user = await _userRepository.GetUserById(id);

            if (user is null)
            {
                throw new Exception($"Não foi encontrado nenhum usuário com o Id = {id}!");
            }

            return user;
        }

        public async Task<User> Login(string username, string password)
        {
            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                throw new Exception("Usuário já logado!");
            }

            User? user = await _userRepository.GetUserByLogin(username);

            if (user is null || user.Password != password)
            {
                throw new Exception("Não há usuários com este nome e senha!");
            }

            List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Type.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            return user;
        }

        public async Task<bool> Logout()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return true;
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

        public async Task<bool> UserIsAdmin(string username)
        {
            if (username.Trim() == "")
            {
                throw new Exception("O nome de usuário não pode ser vazio");
            }

            return await _userRepository.UserIsAdmin(username);
        }

        public async Task<List<User>> GetUsers()
        {
            return await _userRepository.GetUsers();
        }

    }
}
