using Microsoft.AspNetCore.Authorization;
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

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<Question>> Get()
    {
      List<Question> group = await _questionsService.GetQuestions();

      return Ok(group);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<Question>> GetById(int id)
    {
      Question question = await _questionsService.GetQuestionById(id);

      return Ok(question);
    }

    [Authorize(Policy = "AdmininistradorPolicy")]
    [HttpPost]
    public async Task<ActionResult<Question>> Create(Question question)
    {
      Question createdQuestion = await _questionsService.Create(question);

      return Ok(createdQuestion);
    }

    [Authorize(Policy = "AdmininistradorPolicy")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<Question>> Delete(int id)
    {
      string message = await _questionsService.DeleteQuestion(id);

      return Ok(message);
    }

    [Authorize(Policy = "AdmininistradorPolicy")]
    [HttpPut("{id}")]
    public async Task<ActionResult<Question>> Update(int id, Question question)
    {
      string message = await _questionsService.UpdateQuestion(id, question);

      return Ok(message);
    }
  }
}
