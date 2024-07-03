using NpsApi.Models;
using NpsApi.Repositories;

namespace NpsApi.Application.Services
{
  public class AnswersService
  {
    private readonly AnswersRepository _answersRepository;

    public AnswersService(AnswersRepository answersRepository)
    {
      _answersRepository = answersRepository;
    }

    public async Task<Answers> Submit(Answers answer)
    {
      if (answer.Grade < 0 || answer.Grade > 10)
      {
        throw new ArgumentException("Nota inválida!");
      }

      Answers newAnswer = new Answers
      {
        QuestionId = answer.QuestionId,
        UserId = answer.UserId,
        Grade = answer.Grade,
        Description = answer.Description,
      };

      newAnswer.Id = await _answersRepository.SubmitAnswer(newAnswer);

      return newAnswer;
    }

    public async Task<List<Answers>> GetAnswerByClientId(int userId)
    {
      if (userId <= 0)
      {
        throw new ArgumentException("O id do usuário não pode ser menor ou igual a zero!");
      }

      List<Answers>? answers = await _answersRepository.GetAnswersByClientId(userId);

      if (answers == null)
      {
        throw new KeyNotFoundException($"Não foi encontrado nenhum usuário com o Id = {userId}!");
      }

      return answers;
    }

    public async Task<List<Answers>> Get()
    {
      List<Answers> answersList = await _answersRepository.GetAnswers();

      if (!answersList.Any())
      {
        throw new ArgumentException("Não há respostas cadastradas!");
      }

      return answersList;
    }
  }
}
