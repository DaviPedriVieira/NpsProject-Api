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
    public async Task<ActionResult<FormsGroups>> Get()
    {
      List<FormsGroups> group = await _groupFormsService.GetGroups();

      return Ok(group);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<FormsGroups>> GetById(int id)
    {
      FormsGroups group = await _groupFormsService.GetGroupById(id);

      return Ok(group);
    }

    [Authorize(Policy = "AdmininistradorPolicy")]
    [HttpPost]
    public async Task<ActionResult<FormsGroups>> Create(FormsGroups group)
    {
      FormsGroups createdGroup = await _groupFormsService.CreateGroup(group);

      return Ok(createdGroup);
    }

    [Authorize(Policy = "AdmininistradorPolicy")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<string>> Delete(int id)
    {
      string message = await _groupFormsService.DeleteGroup(id);

      return Ok(message);
    }

    [Authorize(Policy = "AdmininistradorPolicy")]
    [HttpPut("{id}")]
    public async Task<ActionResult<string>> Update(int id, FormsGroups group)
    {
      string message = await _groupFormsService.UpdateGroup(id, group);

      return Ok(message);
    }

  }
}
