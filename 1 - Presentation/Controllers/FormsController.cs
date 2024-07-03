using Microsoft.AspNetCore.Http;
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

    [HttpGet]
    public async Task<ActionResult<Forms>> Get()
    {
      List<Forms> group = await _formsService.Get();

      return Ok(group);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Forms>> GetById(int id)
    {
      Forms form = await _formsService.GetById(id);

      return Ok(form);
    }

    [HttpPost]
    public async Task<ActionResult<Forms>> Create(Forms form)
    {
      Forms createdForm = await _formsService.Create(form);

      return Ok(createdForm);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Forms>> Delete(int id)
    {
      string message = await _formsService.Delete(id);

      return Ok(message);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<FormsGroups>> Update(int id, Forms form)
    {
      var message = await _formsService.Update(id, form);

      return Ok(message);
    }
  }
}
