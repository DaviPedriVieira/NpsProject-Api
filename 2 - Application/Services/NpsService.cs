using NpsApi._3___Domain.CommandHandlers;

namespace NpsApi._2___Application.Services
{
  public class NpsService
  {

    private readonly NpsCommandHandler _npsCommandHandler;

    public NpsService(NpsCommandHandler npsCommandHandler)
    {
      _npsCommandHandler = npsCommandHandler;
    }
    
    public async Task<int> GetNpsScore()
    {
      return await _npsCommandHandler.GetNpsScore();
    }
  }
}
