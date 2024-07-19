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

      if (form == null)
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

      if (question == null)
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

    public async Task<bool> DeleteQuestion(int id)
    {
      Question? question = await _questionsRepository.GetQuestionById(id);

      if (question == null)
      {
        throw new KeyNotFoundException($"Não foi encontrado nenhum formulário com o Id = {id}!");
      }

      await _answersRepository.DeleteAnswersByQuestionId(id);

      return await _questionsRepository.DeleteQuestion(id);
    }

    public async Task<bool> UpdateQuestion(int id, Question question)
    {
      Question? toUpdateQuestion = await _questionsRepository.GetQuestionById(id);

      if (toUpdateQuestion == null)
      {
        throw new KeyNotFoundException($"Não foi encontrado nenhuma pergunta com o Id = {id}!");
      }

      Form? form = await _formsRepository.GetFormById(question.FormId);

      if (form == null)
      {
        throw new KeyNotFoundException($"Erro na Fk question.FormId, não foi encontrado nenhum formulário com o Id = {question.FormId}!");
      }

      if (string.IsNullOrWhiteSpace(question.Content))
      {
        throw new ArgumentNullException("question.Content", "O nome não pode ser vazio!");
      }

      return await _questionsRepository.UpdateQuestion(id, question);
    }
  }
}
