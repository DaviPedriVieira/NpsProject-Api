using Microsoft.AspNetCore.Http.HttpResults;
using NpsApi.Models;
using NpsApi.Repositories;

namespace NpsApi._3___Domain.CommandHandlers
{
  public class AnswersCommandHandler
  {
    private readonly AnswersRepository _answersRepository;
    private readonly UsersRepository _usersRepository;
    private readonly QuestionsRepository _questionsRepository;

    public AnswersCommandHandler(AnswersRepository answersRepository, UsersRepository usersRepository, QuestionsRepository questionsRepository)
    {
      _answersRepository = answersRepository;
      _usersRepository = usersRepository;
      _questionsRepository = questionsRepository;
    }

    public async Task<Answer> SubmitAnswer(Answer answer)
    {
      if (answer.Grade < 0 || answer.Grade > 10)
      {
        throw new ArgumentOutOfRangeException("answer.Grade", "Nota inválida!");
      }

      Question? question = await _questionsRepository.GetQuestionById(answer.QuestionId);

      if (question == null)
      {
        throw new KeyNotFoundException($"Erro na FK, não foi encontrado nenhuma pergunta com o Id = {answer.QuestionId}!");
      }

      User? user = await _usersRepository.GetUserById(answer.UserId);

      if (user == null)
      {
        throw new KeyNotFoundException($"Erro na FK, não foi encontrado nenhum usuário com o Id = {answer.UserId}!");
      }

      return await _answersRepository.SubmitAnswer(answer);
    }

    public async Task<List<Answer>> GetAnswersByClientId(int userId)
    {
      User? user = await _usersRepository.GetUserById(userId);

      if (user == null)
      {
        throw new KeyNotFoundException($"Não foi encontrado nenhum usuário com o Id = {userId}!");
      }

      List<Answer> answers = await _answersRepository.GetAnswersByClientId(userId);

      if (!answers.Any())
      {
        throw new KeyNotFoundException($"Não foi encontrada nenhuma pergunta do usuário com o Id = {userId}!");
      }

      return answers;
    }

    public async Task<Answer> GetAnswerById(int id)
    {
      Answer? answer = await _answersRepository.GetAnswerById(id);

      if (answer == null)
      {
        throw new KeyNotFoundException($"Não foi encontrada nenhuma resposta com o Id = {id}!");
      }

      return answer;
    }

    public async Task<List<Answer>> GetAnswers()
    {
      List<Answer> answersList = await _answersRepository.GetAnswers();

      if (!answersList.Any())
      {
        throw new Exception();
      }

      return answersList;
    }
  }
}
