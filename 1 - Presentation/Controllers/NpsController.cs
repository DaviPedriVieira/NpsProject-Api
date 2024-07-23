using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NpsApi._2___Application.Services;

namespace NpsApi._1___Presentation.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class NpsController : ControllerBase
  {
    private readonly NpsService _npsService;

    public NpsController(NpsService npsService)
    {
      _npsService = npsService;
    }

    [Authorize(Policy = "AdmininistradorPolicy")]
    [HttpGet("NpsScore")]
    public async Task<ActionResult<int>> GetNpsScore()
    {
      return Ok(await _npsService.GetNpsScore());
    }
  }
}
