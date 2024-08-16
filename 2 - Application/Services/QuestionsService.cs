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

    public async Task<bool> DeleteQuestion(int id)
    {
      return await _questionsCommandHandler.DeleteQuestion(id);
    }

    public async Task<bool> UpdateQuestion(int id, string newName)
    {
      return await _questionsCommandHandler.UpdateQuestion(id, newName);
    }

        public async Task<List<Question>> GetQuestionByFormId(int formId)
        {
            return await _questionsCommandHandler.GetQuestionByFormId(formId);
        }
    }
}
