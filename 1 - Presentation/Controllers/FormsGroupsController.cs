using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NpsApi.Application.Services;
using NpsApi.Models;

namespace NpsApi.Presentation.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class FormsGroupsController : ControllerBase
  {
    private readonly FormsGroupsService _groupFormsService;

    public FormsGroupsController(FormsGroupsService groupFormsService)
    {
      _groupFormsService = groupFormsService;
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<FormsGroup>> GetGroups()
    {
      try
      {
        return Ok(await _groupFormsService.GetGroups());
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<FormsGroup>> GetGroupById(int id)
    {
      try
      {
        return Ok(await _groupFormsService.GetGroupById(id));
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    [Authorize(Policy = "AdmininistradorPolicy")]
    [HttpPost]
    public async Task<ActionResult<FormsGroup>> CreateGroup(FormsGroup group)
    {
      try
      {
        return Ok(await _groupFormsService.CreateGroup(group));
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    [Authorize(Policy = "AdmininistradorPolicy")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteGroup(int id)
    {
      //try
      //{
        return Ok(await _groupFormsService.DeleteGroup(id));
      //}
      //catch (Exception ex)
      //{
      //  return BadRequest(ex.Message);
      //}
    }

    [Authorize(Policy = "AdmininistradorPolicy")]
    [HttpPut("{id}")]
    public async Task<ActionResult<bool>> UpdateGroup(int id, string newName)
    {
      try
      {
        return Ok(await _groupFormsService.UpdateGroup(id, newName));
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }
  }
}
