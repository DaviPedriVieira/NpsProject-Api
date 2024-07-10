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

    public async Task<Questions> Create(Questions question)
    {
      if (string.IsNullOrWhiteSpace(question.Content))
      {
        throw new ArgumentException("A pergunta não pode ser vazia!");
      }

      Questions newQuestion = await _questionsRepository.CreateQuestion(question);

      return newQuestion;
    }

    public async Task<Questions> GetQuestionById(int id)
    {
      if (id <= 0)
      {
        throw new ArgumentException("O id não pode ser menor ou igual a zero!");
      }

      Questions? question = await _questionsRepository.GetQuestionById(id);

      if (question == null)
      {
        throw new KeyNotFoundException($"Não foi encontrado nenhuma pergunta com o Id = {id}!");
      }

      question.Answers = await _answersRepository.GetAnswersByQuestionId(question.Id);

      return question;
    }

    public async Task<List<Questions>> GetQuestions()
    {
      List<Questions> questionsList = await _questionsRepository.GetQuestions();

      if (!questionsList.Any())
      {
        throw new ArgumentException("Não há perguntas cadastradas!");
      }

      return questionsList;
    }

    public async Task<string> DeleteQuestion(int id)
    {
      List<Answers> answersList = await _answersRepository.GetAnswersByQuestionId(id);
      answersList.ForEach(async answer => await _answersRepository.DeleteAnswer(answer.Id));

      bool deleted = await _questionsRepository.DeleteQuestion(id);

      if (!deleted)
      {
        return "Não foi possível excluir a pergunta!";
      }

      return "Pergunta excluída!";
    }

    public async Task<string> UpdateQuestion(int id, Questions question)
    {
      if (string.IsNullOrWhiteSpace(question.Content))
      {
        throw new ArgumentException("O nome não pode ser vazio!");
      }

      if (id <= 0 || question.FormId <= 0)
      {
        throw new ArgumentException("O Id e Id do formulário não podem ser menores ou iguais a zero!");
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
