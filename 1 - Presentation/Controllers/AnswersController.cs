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
      try
      {
        return Ok(await _answerService.GetAnswers());
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    [Authorize(Policy = "AdmininistradorPolicy")]
    [HttpGet("User/{userId}")]
    public async Task<ActionResult<List<Answer>>> GetAnswersByUserId(int userId)
    {
      try
      {
        return Ok(await _answerService.GetAnswersByUserId(userId));
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    [Authorize(Policy = "AdmininistradorPolicy")]
    [HttpGet("{id}")]
    public async Task<ActionResult<Answer>> GetAnswerById(int id)
    {
      try
      {
        return Ok(await _answerService.GetAnswerById(id));
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Answer>> SubmitAnswers(List<Answer> answers)
    {
      try
      {
        return Ok(await _answerService.SubmitAnswers(answers));
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }
  }
}
