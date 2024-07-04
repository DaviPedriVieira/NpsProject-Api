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
    public async Task<ActionResult<Users>> Create(Users user)
    {
      Users createdUser = await _usersService.CreateUser(user);

      return Ok(createdUser);
    }

    [HttpGet]
    public async Task<ActionResult<Users>> Get()
    {
      List<Users> users = await _usersService.GetUsers();

      return Ok(users);
    }
  }
}
