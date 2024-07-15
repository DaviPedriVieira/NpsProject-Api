using NpsApi.Models;
using NpsApi.Repositories;

namespace NpsApi.Application.Services
{
  public class FormsService
  {
    private readonly FormsRepository _formsRepository;
    private readonly QuestionsRepository _questionsRepository;
    private readonly AnswersRepository _answersRepository;

    public FormsService(FormsRepository repository, QuestionsRepository questionsRepository, AnswersRepository answersRepository)
    {
      _formsRepository = repository;
      _questionsRepository = questionsRepository;
      _answersRepository = answersRepository;
    }

    public async Task<Form> CreateForm(Form form)
    {
      if (string.IsNullOrWhiteSpace(form.Name))
      {
        throw new ArgumentException("O nome não pode ser vazio!");
      }

      Form newForm = await _formsRepository.CreateForm(form);

      foreach (Question question in form.Questions)
      {
        question.FormId = newForm.Id;
        await _questionsRepository.CreateQuestion(question);
      }

      return newForm;
    }

    public async Task<Form> GetFormById(int id)
    {
      Form? form = await _formsRepository.GetFormById(id);

      if (form == null)
      {
        throw new KeyNotFoundException($"Não foi encontrado nenhum forulário com o Id = {id}!");
      }

      form.Questions = await _questionsRepository.GetQuestionsByFormId(form.Id);

      return form;
    }

    public async Task<List<Form>> GetForms()
    {
      List<Form> groupsList = await _formsRepository.GetForms();

      if (!groupsList.Any())
      {
        throw new ArgumentException("Não há formulários cadastrados!");
      }

      return groupsList;
    }

    public async Task<string> DeleteForm(int id)
    {
      List<Question> questionsList = await _questionsRepository.GetQuestionsByFormId(id);

      foreach (Question question in questionsList)
      {
        await _answersRepository.DeleteAnswersByQuestionId(question.Id);

        await _questionsRepository.DeleteQuestion(question.Id);
      }

      bool deleted = await _formsRepository.DeleteForm(id);

      if (!deleted)
      {
        return "Não foi possível excluir o formulário!";
      }

      return "Formulário excluíd!";
    }

    public async Task<string> UpdateForm(int id, Form form)
    {
      if (string.IsNullOrWhiteSpace(form.Name))
      {
        throw new ArgumentException("O nome não pode ser vazio!");
      }

      if (form.GroupId <= 0)
      {
        throw new ArgumentException("Id do grupo inválido!");
      }

      bool edited = await _formsRepository.UpdateForm(id, form);

      if (!edited)
      {
        return "Não foi possível editar o formulário!";
      }

      return "Formulário editado!";
    }
  }
}
