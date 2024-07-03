using Microsoft.AspNetCore.Mvc;
using NpsApi.Application.Services;
using NpsApi.Models;

namespace NpsApi.Presentation.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class QuestionsController : ControllerBase
  {
    private readonly QuestionsService _questionsService;

    public QuestionsController(QuestionsService questionsService)
    {
      _questionsService = questionsService;
    }

    [HttpGet]
    public async Task<ActionResult<Questions>> Get()
    {
      List<Questions> group = await _questionsService.Get();

      return Ok(group);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Questions>> GetById(int id)
    {
      Questions question = await _questionsService.GetById(id);

      return Ok(question);
    }

    [HttpPost]
    public async Task<ActionResult<Questions>> Create(Questions question)
    {
      Questions createdQuestion = await _questionsService.Create(question);

      return Ok(createdQuestion);
    }

    [HttpDelete]
    public async Task<ActionResult<Questions>> Delete(int id)
    {
      string message = await _questionsService.Delete(id);

      return Ok(message);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Questions>> Update(int id, Questions question)
    {
      var message = await _questionsService.Update(id, question);

      return Ok(message);
    }
  }
}
