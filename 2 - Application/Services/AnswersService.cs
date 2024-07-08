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

    public async Task<Answers> SubmitAnswer(Answers answer)
    {
      if (answer.Grade < 0 || answer.Grade > 10)
      {
        throw new ArgumentException("Nota inválida!");
      }

      Answers newAnswer = await _answersRepository.SubmitAnswer(answer);

      return newAnswer;
    }

    public async Task<List<Answers>> GetAnswersByClientId(int userId)
    {
      if (userId <= 0)
      {
        throw new ArgumentException("O id do usuário não pode ser menor ou igual a zero!");
      }

      List<Answers> answers = await _answersRepository.GetAnswersByClientId(userId);

      if (!answers.Any())
      {
        throw new KeyNotFoundException($"Não foi encontrada nenhuma pergunta do usuário com o Id = {userId}!");
      }

      return answers;
    }

    public async Task<List<Answers>> GetAnswers()
    {
      List<Answers> answersList = await _answersRepository.GetAnswers();

      if (!answersList.Any())
      {
        throw new ArgumentException("Não há respostas cadastradas!");
      }

      return answersList;
    }

    public async Task<string> DeleteAnswer(int id)
    {
      if (id <= 0)
      {
        throw new ArgumentException("O id não pode ser menor ou igual a zero!");
      }

      bool deleted = await _answersRepository.DeleteAnswer(id);

      if (!deleted)
      {
        return "Não foi possível excluir a resposta!";
      }

      return "Resposta excluída!";
    }
  }
}
