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
      return Ok(await _groupFormsService.GetGroups());
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<FormsGroup>> GetById(int id)
    {
      return Ok(await _groupFormsService.GetGroupById(id));
    }

    [Authorize(Policy = "AdmininistradorPolicy")]
    [HttpPost]
    public async Task<ActionResult<FormsGroup>> Create(FormsGroup group)
    {
      return Ok(await _groupFormsService.CreateGroup(group));
    }

    [Authorize(Policy = "AdmininistradorPolicy")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<string>> Delete(int id)
    {
      return Ok(await _groupFormsService.DeleteGroup(id));
    }

    [Authorize(Policy = "AdmininistradorPolicy")]
    [HttpPut("{id}")]
    public async Task<ActionResult<bool>> Update(int id, FormsGroup group)
    {
      return Ok(await _groupFormsService.UpdateGroup(id, group));
    }

  }
}
