using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NpsApi.Application.Services;
using NpsApi.Models;

namespace NpsApi.Presentation.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AnswersController : ControllerBase
  {
    private readonly AnswersService _answerService;

    public AnswersController(AnswersService answersService)
    {
      _answerService = answersService;
    }

    [Authorize(Policy = "AdmininistradorPolicy")]
    [HttpGet]
    public async Task<ActionResult<List<Answers>>> Get()
    {
      List<Answers> answersList = await _answerService.GetAnswers();

      return Ok(answersList);
    }

    [Authorize(Policy = "AdmininistradorPolicy")]
    [HttpGet("{userId}")]
    public async Task<ActionResult<List<Answers>>> GetByClientId(int userId)
    {
      List<Answers> answersList = await _answerService.GetAnswersByClientId(userId);

      return Ok(answersList);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Answers>> SubmitAnswer(Answers answer)
    {
      Answers newAnswer = await _answerService.SubmitAnswer(answer);

      return Ok(newAnswer);
    }

    [Authorize(Policy = "AdmininistradorPolicy")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<Forms>> Delete(int id)
    {
      string message = await _answerService.DeleteAnswer(id);

      return Ok(message);
    }
  }
}
