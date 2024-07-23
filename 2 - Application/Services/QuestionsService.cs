using NpsApi._3___Domain.CommandHandlers;
using NpsApi.Models;

namespace NpsApi.Application.Services
{
  public class QuestionsService
  {
    private readonly QuestionsCommandHandler _questionsCommandHandler;

    public QuestionsService(QuestionsCommandHandler questionsCommandHandler)
    {
      _questionsCommandHandler = questionsCommandHandler;
    }

    public async Task<Question> Create(Question question)
    {
      return await _questionsCommandHandler.CreateQuestion(question);
    }

    public async Task<Question> GetQuestionById(int id)
    {
      return await _questionsCommandHandler.GetQuestionById(id);
    }

    public async Task<List<Question>> GetQuestions()
    {
      return await _questionsCommandHandler.GetQuestions();
    }

    public async Task<string> DeleteQuestion(int id)
    {
      return await _questionsCommandHandler.DeleteQuestion(id);
    }

    public async Task<string> UpdateQuestion(int id, Question question)
    {
      return await _questionsCommandHandler.UpdateQuestion(id, question);
    }
  }
}
