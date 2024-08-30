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
      try
      {
        return Ok(await _questionsService.GetQuestions());
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<Question>> GetQuestionById(int id)
    {
      try
      {
        return Ok(await _questionsService.GetQuestionById(id));
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

        [Authorize]
        [HttpGet("Form/{formId}")]
        public async Task<ActionResult<Question>> GetQuestionByFormId(int formId)
        {
            try
            {
                return Ok(await _questionsService.GetQuestionByFormId(formId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("ids/Form/{formId}")]
        public async Task<ActionResult<List<int>>> GetQuestionIdsByFormId(int formId)
        {
            try
            {
                return Ok(await _questionsService.GetQuestionsIdsByFormId(formId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Policy = "AdmininistradorPolicy")]
    [HttpPost]
    public async Task<ActionResult<Question>> CreateQuestion(List<Question> questions)
    {
      try
      {
        return Ok(await _questionsService.Create(questions));
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    [Authorize(Policy = "AdmininistradorPolicy")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteQuestion(int id)
    {
      try
      {
        return Ok(await _questionsService.DeleteQuestion(id));
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    [Authorize(Policy = "AdmininistradorPolicy")]
    [HttpPut("{id}")]
    public async Task<ActionResult<bool>> UpdateQuestion(int id, string newName)
    {
      try
      {
        return Ok(await _questionsService.UpdateQuestion(id, newName));
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }
  }
}
