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
        throw new Exception($"Erro na Fk question.FormId, não foi encontrado nenhum formulário com o Id = {question.FormId}!");
      }

      if (string.IsNullOrWhiteSpace(question.Content))
      {
        throw new Exception("A pergunta não pode ser vazia!");
      }

      return await _questionsRepository.CreateQuestion(question);
    }

    public async Task<Question> GetQuestionById(int id)
    {
      Question? question = await _questionsRepository.GetQuestionById(id);

      if (question is null)
      {
        throw new Exception($"Não foi encontrado nenhuma pergunta com o Id = {id}!");
      }

      return question;
    }

    public async Task<List<Question>> GetQuestions()
    {
      return await _questionsRepository.GetQuestions();
    }

    public async Task<bool> DeleteQuestion(int id)
    {
      Question? question = await _questionsRepository.GetQuestionById(id);

      if (question is null)
      {
        throw new Exception($"Não foi encontrada nenhuma pergunta com o Id = {id}!");
      }

      await _answersRepository.DeleteAnswersByQuestionId(id);

      return await _questionsRepository.DeleteQuestion(id);
    }

    public async Task<bool> UpdateQuestion(int id, string newName)
    {
      Question? toUpdateQuestion = await _questionsRepository.GetQuestionById(id);

      if (toUpdateQuestion is null)
      {
        throw new Exception($"Não foi encontrado nenhuma pergunta com o Id = {id}!");
      }

      if (string.IsNullOrWhiteSpace(newName))
      {
        throw new Exception("O nome não pode ser vazio!");
      }

      return await _questionsRepository.UpdateQuestion(id, newName);
    }

        public async Task<List<Question>> GetQuestionByFormId(int formId)
        {
            return await _questionsRepository.GetQuestionsByFormId(formId);
        }
    }
}
