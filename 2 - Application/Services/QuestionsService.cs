using NpsApi.Models;
using NpsApi.Repositories;

namespace NpsApi.Application.Services
{
  public class QuestionsService
  {
    private readonly QuestionsRepository _questionsRepository;
    private readonly AnswersRepository _answersRepository;

    public QuestionsService(QuestionsRepository repository, AnswersRepository answersRepository)
    {
      _questionsRepository = repository;
      _answersRepository = answersRepository;
    }

    public async Task<Question> Create(Question question)
    {
      if (string.IsNullOrWhiteSpace(question.Content))
      {
        throw new ArgumentException("A pergunta não pode ser vazia!");
      }

      Question newQuestion = await _questionsRepository.CreateQuestion(question);

      return newQuestion;
    }

    public async Task<Question> GetQuestionById(int id)
    {
      Question? question = await _questionsRepository.GetQuestionById(id);

      if (question == null)
      {
        throw new KeyNotFoundException($"Não foi encontrado nenhuma pergunta com o Id = {id}!");
      }

      return question;
    }

    public async Task<List<Question>> GetQuestions()
    {
      List<Question> questionsList = await _questionsRepository.GetQuestions();

      if (!questionsList.Any())
      {
        throw new ArgumentException("Não há perguntas cadastradas!");
      }

      return questionsList;
    }

    public async Task<string> DeleteQuestion(int id)
    {
      await _answersRepository.DeleteAnswersByQuestionId(id);

      bool deleted = await _questionsRepository.DeleteQuestion(id);

      if (!deleted)
      {
        return "Não foi possível excluir a pergunta!";
      }

      return "Pergunta excluída!";
    }

    public async Task<string> UpdateQuestion(int id, Question question)
    {
      if (string.IsNullOrWhiteSpace(question.Content))
      {
        throw new ArgumentException("O nome não pode ser vazio!");
      }

      if (question.FormId <= 0)
      {
        throw new ArgumentException("Id do formulário inválido!");
      }

      bool edited = await _questionsRepository.DeleteQuestion(id);

      if (!edited)
      {
        return "Não foi possível editar a pergunta!";
      }

      return "Pergunta editada!";
    }
  }
}
