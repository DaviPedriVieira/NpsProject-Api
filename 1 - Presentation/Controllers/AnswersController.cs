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
    public async Task<ActionResult<List<Answer>>> GetAnswers()
    {
      return Ok(await _answerService.GetAnswers());
    }

    [Authorize(Policy = "AdmininistradorPolicy")]
    [HttpGet("User/{userId}")]
    public async Task<ActionResult<List<Answer>>> GetAnswersByUserId(int userId)
    {
      return Ok(await _answerService.GetAnswersByUserId(userId));
    }

    [Authorize(Policy = "AdmininistradorPolicy")]
    [HttpGet("{id}")]
    public async Task<ActionResult<Answer>> GetAnswerById(int id)
    {
      return Ok(await _answerService.GetAnswerById(id));
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Answer>> SubmitAnswers(List<Answer> answers)
    {
      return Ok(await _answerService.SubmitAnswers(answers));
    }
  }
}
