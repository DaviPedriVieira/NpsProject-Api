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
    public async Task<ActionResult<List<Answer>>> Get()
    {
      return Ok(await _answerService.GetAnswers());
    }

    [Authorize(Policy = "AdmininistradorPolicy")]
    [HttpGet("User/{userId}")]
    public async Task<ActionResult<List<Answer>>> GetByClientId(int userId)
    {
      return Ok(await _answerService.GetAnswersByClientId(userId));
    }

    [Authorize(Policy = "AdmininistradorPolicy")]
    [HttpGet("{id}")]
    public async Task<ActionResult<Answer>> GetById(int id)
    {
      return Ok(await _answerService.GetAnswerById(id));
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Answer>> SubmitAnswer(Answer answer)
    {
      return Ok(await _answerService.SubmitAnswer(answer));
    }
  }
}
