using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NpsApi.Application.Services;
using NpsApi.Models;
using System.Data.SqlClient;

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

    [HttpGet]
    public async Task<ActionResult<FormsGroups>> Get()
    {
      List<FormsGroups> group = await _groupFormsService.Get();

      return Ok(group);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<FormsGroups>> GetById(int id)
    {
      FormsGroups group = await _groupFormsService.GetById(id);

      return Ok(group);
    }

    [HttpPost]
    public async Task<ActionResult<FormsGroups>> Create(string name)
    {
      FormsGroups createdGroup = await _groupFormsService.Create(name);

      return Ok(createdGroup);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<string>> Delete(int id)
    {
      string message = await _groupFormsService.Delete(id);

      return Ok(message);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<string>> Update(int id, FormsGroups group)
    {
      string message = await _groupFormsService.Update(id, group);

      return Ok(message);
    }


  }
}
