using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NpsApi.Application.Services;
using NpsApi.Models;

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

    [HttpPost]
    public async Task<ActionResult<User>> Create(User user)
    {
      return Ok(await _usersService.CreateUser(user));

    }

    [Authorize(Policy = "AdmininistradorPolicy")]
    [HttpGet]
    public async Task<ActionResult<User>> Get()
    {
      Console.WriteLine(DateTime.Now);
      return Ok(await _usersService.GetUsers());
    }

    [Authorize(Policy = "AdmininistradorPolicy")]
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> Get(int id)
    {
      return Ok(await _usersService.GetUserById(id));
    }

    [HttpPost("/login")]
    public async Task<ActionResult> Login(string name, string password)
    {
      return Ok(await _usersService.Login(name, password));
    }

    [Authorize]
    [HttpPost("/logout")]
    public async Task<ActionResult> Logout()
    {
      return Ok(await _usersService.Logout());
    }

    [Authorize(Policy = "AdmininistradorPolicy")]
    [HttpGet("Promoters")]
    public async Task<ActionResult<List<User>>> GetPromoters()
    {
      return Ok(await _usersService.GetPromoters());
    }

    [Authorize(Policy = "AdmininistradorPolicy")]
    [HttpGet("Passives")]
    public async Task<ActionResult<List<User>>> GetPassives()
    {
      return Ok(await _usersService.GetPassives());
    }

    [Authorize(Policy = "AdmininistradorPolicy")]
    [HttpGet("Detractors")]
    public async Task<ActionResult<List<User>>> GetDetractors()
    {
      return Ok(await _usersService.GetDetractors());
    }
  }
}

