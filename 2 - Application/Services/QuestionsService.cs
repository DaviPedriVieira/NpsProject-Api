using NpsApi.Data;
using NpsApi.Models;
using NpsApi.Repositories;
using System.Data.SqlClient;

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

    public async Task<Questions> Create(Questions question)
    {
      if (string.IsNullOrWhiteSpace(question.Content))
      {
        throw new ArgumentException("A pergunta não pode ser vazia!");
      }

      Questions newQuestion = new Questions
      {
        Content = question.Content,
        FormId = question.FormId,
      };

      newQuestion.Id = await _questionsRepository.Create(newQuestion);

      return newQuestion;
    }

    public async Task<Questions> GetById(int id)
    {
      if (id <= 0)
      {
        throw new ArgumentException("O id não pode ser menor ou igual a zero!");
      }

      Questions? question = await _questionsRepository.GetById(id);

      if (question == null)
      {
        throw new KeyNotFoundException($"Não foi encontrado nenhuma pergunta com o Id = {id}!");
      }

      question.Answers = await _answersRepository.GetAnswersByQuestionId(question.Id);

      return question;
    }

    public async Task<List<Questions>> Get()
    {
      List<Questions> questionsList = await _questionsRepository.Get();

      if (!questionsList.Any())
      {
        throw new ArgumentException("Não há perguntas cadastradas!");
      }

      return questionsList;
    }

    public async Task<string> Delete(int id)
    {
      bool deleted = await _questionsRepository.Delete(id);

      if (!deleted)
      {
        return "Não foi possível excluir a pergunta!";
      }

      return "Pergunta excluída com sucesso";
    }

    public async Task<string> Update(int id, Questions question)
    {
      if (string.IsNullOrWhiteSpace(question.Content))
      {
        throw new ArgumentException("O nome não pode ser vazio!");
      }

      if (id <= 0 || question.FormId <= 0)
      {
        throw new ArgumentException("O Id/Id do formulário não podem ser menores ou iguais a zero!");
      }

      bool edited = await _questionsRepository.Delete(id);

      if (!edited)
      {
        return "Não foi possível editar a pergunta!";
      }

      return "Pergunta editada com sucesso!";
    }
  }
}
