using NpsApi.Models;
using NpsApi.Repositories;
using System.Security.Claims;

namespace NpsApi._3___Domain.CommandHandlers
{
  public class AnswersCommandHandler
  {
    private readonly AnswersRepository _answersRepository;
    private readonly UsersRepository _usersRepository;
    private readonly QuestionsRepository _questionsRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AnswersCommandHandler(AnswersRepository answersRepository, UsersRepository usersRepository, QuestionsRepository questionsRepository, IHttpContextAccessor httpContextAccessor)
    {
      _answersRepository = answersRepository;
      _usersRepository = usersRepository;
      _questionsRepository = questionsRepository;
      _httpContextAccessor = httpContextAccessor;
    }

    public async Task<List<Answer>> SubmitAnswers(List<Answer> answers)
    {
      string userIdString = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

      int activeUserId = Convert.ToInt32(userIdString);

      foreach (Answer answer in answers)
      {
        if (answer.Grade < 0 || answer.Grade > 10)
        {
          throw new Exception("Nota inválida!");
        }

        Question? question = await _questionsRepository.GetQuestionById(answer.QuestionId);

        if (question is null)
        {
          throw new Exception($"Erro na FK answer.QuestionId, não foi encontrada nenhuma pergunta com o Id = {answer.QuestionId}!");
        }

        answer.UserId = activeUserId;
      }

      return await _answersRepository.SubmitAnswers(answers);
    }

    public async Task<List<Answer>> GetAnswersByUserId(int userId)
    {
      User? user = await _usersRepository.GetUserById(userId);

      if (user is null)
      {
        throw new Exception($"Erro na FK answer.UserId, não foi encontrado nenhum usuário com o Id = {userId}!");
      }

      List<Answer> answers = await _answersRepository.GetAnswersByUserId(userId);

      if (answers.Count == 0)
      {
        throw new Exception($"Não foi encontrada nenhuma resposta do usuário com o Id = {userId}!");
      }

      return answers;
    }

    public async Task<Answer> GetAnswerById(int id)
    {
      Answer? answer = await _answersRepository.GetAnswerById(id);

      if (answer is null)
      {
        throw new Exception($"Não foi encontrada nenhuma resposta com o Id = {id}!");
      }

      return answer;
    }

    public async Task<List<Answer>> GetAnswers()
    {
      List<Answer> answersList = await _answersRepository.GetAnswers();

      if (answersList.Count == 0)
      {
        throw new Exception("Não há respostas cadastradas!");
      }

      return answersList;
    }
  }
}
