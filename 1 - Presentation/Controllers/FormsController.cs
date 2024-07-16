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
    public async Task<ActionResult<Form>> Get()
    {
      List<Form> group = await _formsService.GetForms();

      return Ok(group);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<Form>> GetById(int id)
    {
      Form form = await _formsService.GetFormById(id);

      return Ok(form);
    }

    [Authorize(Policy = "AdmininistradorPolicy")]
    [HttpPost]
    public async Task<ActionResult<Form>> Create(Form form)
    {
      Form createdForm = await _formsService.CreateForm(form);

      return Ok(createdForm);
    }

    [Authorize(Policy = "AdmininistradorPolicy")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<Form>> Delete(int id)
    {
      bool deleted = await _formsService.DeleteForm(id);

      return Ok(deleted);
    }

    [Authorize(Policy = "AdmininistradorPolicy")]
    [HttpPut("{id}")]
    public async Task<ActionResult<FormsGroup>> Update(int id, Form form)
    {
      bool updated = await _formsService.UpdateForm(id, form);

      return Ok(updated);
    }
  }
}
