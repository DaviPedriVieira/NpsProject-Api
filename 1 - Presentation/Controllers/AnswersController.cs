using Microsoft.AspNetCore.Http;
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

    [HttpGet]
    public async Task<ActionResult<List<Answers>>> Get()
    {
      List<Answers> answersList = await _answerService.Get();

      return Ok(answersList);
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<List<Answers>>> GetByClientId(int userId)
    {
      List<Answers> answersList = await _answerService.GetAnswerByClientId(userId);

      return Ok(answersList);
    }

    [HttpPost]
    public async Task<ActionResult<Answers>> SubmitAnswer(Answers answer)
    {
      Answers newAnswer = await _answerService.Submit(answer);

      return Ok(newAnswer);
    }
  }
}
