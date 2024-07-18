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
      return Ok(await _questionsService.GetQuestions());
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<Question>> GetById(int id)
    {
      return Ok(await _questionsService.GetQuestionById(id));
    }

    [Authorize(Policy = "AdmininistradorPolicy")]
    [HttpPost]
    public async Task<ActionResult<Question>> Create(Question question)
    {
      return Ok(await _questionsService.Create(question));
    }

    [Authorize(Policy = "AdmininistradorPolicy")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<Question>> Delete(int id)
    {
      return Ok(await _questionsService.DeleteQuestion(id));
    }

    [Authorize(Policy = "AdmininistradorPolicy")]
    [HttpPut("{id}")]
    public async Task<ActionResult<Question>> Update(int id, Question question)
    {
      return Ok(await _questionsService.UpdateQuestion(id, question));
    }
  }
}
