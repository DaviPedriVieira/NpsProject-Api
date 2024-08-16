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
    public async Task<ActionResult<User>> CreateUser(User user)
    {
      try
      {
        return Ok(await _usersService.CreateUser(user));
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    [Authorize(Policy = "AdmininistradorPolicy")]
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUserById(int id)
    {
      try
      {
        return Ok(await _usersService.GetUserById(id));
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    [HttpPost("Login")]
    public async Task<ActionResult> Login(string name, string password)
    {
      try
      {
        return Ok(await _usersService.Login(name, password));
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    [Authorize]
    [HttpPost("Logout")]
    public async Task<ActionResult> Logout()
    {
      try
      {
        return Ok(await _usersService.Logout());
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    [Authorize(Policy = "AdmininistradorPolicy")]
    [HttpGet("Promoters")]
    public async Task<ActionResult<List<User>>> GetPromoters()
    {
      try
      {
        return Ok(await _usersService.GetPromoters());
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    [Authorize(Policy = "AdmininistradorPolicy")]
    [HttpGet("Passives")]
    public async Task<ActionResult<List<User>>> GetPassives()
    {
      try
      {
        return Ok(await _usersService.GetPassives());
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    [Authorize(Policy = "AdmininistradorPolicy")]
    [HttpGet("Detractors")]
    public async Task<ActionResult<List<User>>> GetDetractors()
    {
      try
      {
        return Ok(await _usersService.GetDetractors());
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

        [Authorize]
        [HttpGet("Type")]
        public async Task<ActionResult<bool>> UserIsAdmin(string username)
        {
            try
            {
                return Ok(await _usersService.UserIsAdmin(username));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

