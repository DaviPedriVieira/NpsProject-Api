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
    public async Task<ActionResult<Form>> GetForms()
    {
      return Ok(await _formsService.GetForms());
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<Form>> GetFormById(int id)
    {
      return Ok(await _formsService.GetFormById(id));
    }

    [Authorize(Policy = "AdmininistradorPolicy")]
    [HttpPost]
    public async Task<ActionResult<Form>> CreateForm(Form form)
    {
      return Ok(await _formsService.CreateForm(form));
    }

    [Authorize(Policy = "AdmininistradorPolicy")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<Form>> DeleteForm(int id)
    {
      return Ok(await _formsService.DeleteForm(id));
    }

    [Authorize(Policy = "AdmininistradorPolicy")]
    [HttpPut("{id}")]
    public async Task<ActionResult<FormsGroup>> UpdateForm(int id, Form form)
    {
      return Ok(await _formsService.UpdateForm(id, form));
    }
  }
}
