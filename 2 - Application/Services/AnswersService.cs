using NpsApi._3___Domain.CommandHandlers;
using NpsApi.Models;

namespace NpsApi.Application.Services
{
  public class AnswersService
  {
    private readonly AnswersCommandHandler _answersCommandHandler;

    public AnswersService(AnswersCommandHandler answersCommandHandler)
    {
      _answersCommandHandler = answersCommandHandler;
    }

    public async Task<Answer> SubmitAnswer(Answer answer)
    {
      return await _answersCommandHandler.SubmitAnswer(answer);
    }

    public async Task<List<Answer>> GetAnswersByUserId(int userId)
    {
      return await _answersCommandHandler.GetAnswersByUserId(userId);
    }

    public async Task<Answer> GetAnswerById(int id)
    {
      return await _answersCommandHandler.GetAnswerById(id);
    }

    public async Task<List<Answer>> GetAnswers()
    {
      return await _answersCommandHandler.GetAnswers();
    }
  }
}
