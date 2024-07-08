using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NpsApi.Application.Services;
using NpsApi.Models;

namespace NpsApi.Presentation.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class FormsController : ControllerBase
  {
    private readonly FormsService _formsService;

    public FormsController(FormsService formsService)
    {
      _formsService = formsService;
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<Forms>> Get()
    {
      List<Forms> group = await _formsService.GetForms();

      return Ok(group);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<Forms>> GetById(int id)
    {
      Forms form = await _formsService.GetFormById(id);

      return Ok(form);
    }

    [Authorize(Policy = "AdmininistradorPolicy")]
    [HttpPost]
    public async Task<ActionResult<Forms>> Create(Forms form)
    {
      Forms createdForm = await _formsService.CreateForm(form);

      return Ok(createdForm);
    }

    [Authorize(Policy = "AdmininistradorPolicy")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<Forms>> Delete(int id)
    {
      string message = await _formsService.DeleteForm(id);

      return Ok(message);
    }

    [Authorize(Policy = "AdmininistradorPolicy")]
    [HttpPut("{id}")]
    public async Task<ActionResult<FormsGroups>> Update(int id, Forms form)
    {
      var message = await _formsService.UpdateForm(id, form);

      return Ok(message);
    }
  }
}
