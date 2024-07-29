using NpsApi.Models;
using NpsApi.Repositories;

namespace NpsApi._3___Domain.CommandHandlers
{
  public class QuestionsCommandHandler
  {
    private readonly FormsRepository _formsRepository;
    private readonly QuestionsRepository _questionsRepository;
    private readonly AnswersRepository _answersRepository;

    public QuestionsCommandHandler(QuestionsRepository questionsRepository, AnswersRepository answersRepository, FormsRepository formsRepository)
    {
      _formsRepository = formsRepository;
      _questionsRepository = questionsRepository;
      _answersRepository = answersRepository;
    }

    public async Task<Question> CreateQuestion(Question question)
    {
      Form? form = await _formsRepository.GetFormById(question.FormId);

      if (form is null)
      {
        throw new KeyNotFoundException($"Erro na Fk, não foi encontrado nenhum formulário com o Id = {question.FormId}!");
      }

      if (string.IsNullOrWhiteSpace(question.Content))
      {
        throw new ArgumentNullException("question.Content", "A pergunta não pode ser vazia!");
      }

      return await _questionsRepository.CreateQuestion(question);
    }

    public async Task<Question> GetQuestionById(int id)
    {
      Question? question = await _questionsRepository.GetQuestionById(id);

      if (question is null)
      {
        throw new KeyNotFoundException($"Não foi encontrado nenhuma pergunta com o Id = {id}!");
      }

      return question;
    }

    public async Task<List<Question>> GetQuestions()
    {
      List<Question> questionsList = await _questionsRepository.GetQuestions();

      if (questionsList.Count == 0)
      {
        throw new Exception("Não há perguntas cadastradas!");
      }

      return questionsList;
    }

    public async Task<string> DeleteQuestion(int id)
    {
      Question? question = await _questionsRepository.GetQuestionById(id);

      if (question is null)
      {
        throw new KeyNotFoundException($"Não foi encontrada nenhuma pergunta com o Id = {id}!");
      }

      await _answersRepository.DeleteAnswersByQuestionId(id);

      bool deleted = await _questionsRepository.DeleteQuestion(id);

      return deleted ? "Pergunta excluída!" : "Não foi possível excluir a pergunta!";
    }

    public async Task<string> UpdateQuestion(int id, Question question)
    {
      Question? toUpdateQuestion = await _questionsRepository.GetQuestionById(id);

      if (toUpdateQuestion is null)
      {
        throw new KeyNotFoundException($"Não foi encontrado nenhuma pergunta com o Id = {id}!");
      }

      if (string.IsNullOrWhiteSpace(question.Content))
      {
        throw new ArgumentNullException("question.Content", "O nome não pode ser vazio!");
      }

      bool updated = await _questionsRepository.UpdateQuestion(id, question);

      return updated ? "Pergunta editada!" : "Não foi possível editar a pergunta!";
    }
  }
}
