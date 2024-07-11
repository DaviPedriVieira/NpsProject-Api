using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NpsApi.Application.Services;
using NpsApi.Models;
using System.Security.Claims;

namespace NpsApi.Presentation.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class UsersController : ControllerBase
  {
    private readonly UsersService _usersService;

    public UsersController(UsersService usersService)
    {
      _usersService = usersService;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Users>> Create(Users user)
    {
      Users createdUser = await _usersService.CreateUser(user);

      return Ok(createdUser);
    }

    [Authorize(Policy = "AdmininistradorPolicy")]
    [HttpGet]
    public async Task<ActionResult<Users>> Get()
    {
      List<Users> users = await _usersService.GetUsers();

      return Ok(users);
    }

    [HttpPost("/login")]
    public async Task<ActionResult> Login(string name, string password)
    {
      List<Users> usersList = await _usersService.GetUsers();
      Users? user = usersList.Find(user => user.Name == name && user.Password == password);

      if (user == null)
      {
        return BadRequest("Não há usuários com este nome e senha!");
      }

      List<Claim> claims = new List<Claim> {
         new Claim(ClaimTypes.Name, user.Name),
         new Claim(ClaimTypes.Role, user.Type.ToString())
      };

      ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
      ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

      await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

      return Ok($"Login realizado!");
    }

    [HttpPost("/logout")]
    public async Task<ActionResult> Logout()
    {
      await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
      return Ok("Logout realizado!");
    }
  }
}
