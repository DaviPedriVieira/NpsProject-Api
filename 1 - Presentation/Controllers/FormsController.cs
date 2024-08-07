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
      try
      {
        return Ok(await _formsService.GetForms());
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<Form>> GetFormById(int id)
    {
      try
      {
        return Ok(await _formsService.GetFormById(id));
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    [Authorize(Policy = "AdmininistradorPolicy")]
    [HttpPost]
    public async Task<ActionResult<Form>> CreateForm(Form form)
    {
      try
      {
        return Ok(await _formsService.CreateForm(form));
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    [Authorize(Policy = "AdmininistradorPolicy")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteForm(int id)
    {
      try
      {
        return Ok(await _formsService.DeleteForm(id));
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    [Authorize(Policy = "AdmininistradorPolicy")]
    [HttpPut("{id}")]
    public async Task<ActionResult<bool>> UpdateForm(int id, Form form)
    {
      try
      {
        return Ok(await _formsService.UpdateForm(id, form));
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }
  }
}
