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
    public async Task<ActionResult<FormsGroup>> Get()
    {
      List<FormsGroup> group = await _groupFormsService.GetGroups();

      return Ok(group);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<FormsGroup>> GetById(int id)
    {
      FormsGroup group = await _groupFormsService.GetGroupById(id);

      return Ok(group);
    }

    [Authorize(Policy = "AdmininistradorPolicy")]
    [HttpPost]
    public async Task<ActionResult<FormsGroup>> Create(FormsGroup group)
    {
      FormsGroup createdGroup = await _groupFormsService.CreateGroup(group);

      return Ok(createdGroup);
    }

    [Authorize(Policy = "AdmininistradorPolicy")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<string>> Delete(int id)
    {
      bool deleted = await _groupFormsService.DeleteGroup(id);

      return Ok(deleted);
    }

    [Authorize(Policy = "AdmininistradorPolicy")]
    [HttpPut("{id}")]
    public async Task<ActionResult<bool>> Update(int id, FormsGroup group)
    {
      bool updated = await _groupFormsService.UpdateGroup(id, group);

      return Ok(updated);
    }

  }
}
