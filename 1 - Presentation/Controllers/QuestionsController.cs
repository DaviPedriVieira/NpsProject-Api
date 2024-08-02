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
    public async Task<ActionResult<Question>> GetQuestions()
    {
      return Ok(await _questionsService.GetQuestions());
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<Question>> GetQuestionById(int id)
    {
      return Ok(await _questionsService.GetQuestionById(id));
    }

    [Authorize(Policy = "AdmininistradorPolicy")]
    [HttpPost]
    public async Task<ActionResult<Question>> CreateQuestion(Question question)
    {
      return Ok(await _questionsService.Create(question));
    }

    [Authorize(Policy = "AdmininistradorPolicy")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<Question>> DeleteQuestion(int id)
    {
      return Ok(await _questionsService.DeleteQuestion(id));
    }

    [Authorize(Policy = "AdmininistradorPolicy")]
    [HttpPut("{id}")]
    public async Task<ActionResult<Question>> UpdateQuestion(int id, Question question)
    {
      return Ok(await _questionsService.UpdateQuestion(id, question));
    }
  }
}
