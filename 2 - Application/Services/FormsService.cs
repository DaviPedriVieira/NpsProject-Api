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

    public async Task<Forms> CreateForm(Forms form)
    {
      if (string.IsNullOrWhiteSpace(form.Name))
      {
        throw new ArgumentException("O nome não pode ser vazio!");
      }

      Forms newForm = await _formsRepository.CreateForm(form);

      foreach (Questions question in form.Questions)
      {
        question.FormId = newForm.Id;
        newForm.Questions.Add(await _questionsRepository.CreateQuestion(question));
      }

      return newForm;
    }

    public async Task<Forms> GetFormById(int id)
    {
      if (id <= 0)
      {
        throw new ArgumentException("O id não pode ser menor ou igual a zero!");
      }

      Forms? form = await _formsRepository.GetFormById(id);

      if (form == null)
      {
        throw new KeyNotFoundException($"Não foi encontrado nenhum forulário com o Id = {id}!");
      }

      form.Questions = await _questionsRepository.GetQuestionsByFormId(form.Id);

      return form;
    }

    public async Task<List<Forms>> GetForms()
    {
      List<Forms> groupsList = await _formsRepository.GetForms();

      if (!groupsList.Any())
      {
        throw new ArgumentException("Não há formulários cadastrados!");
      }

      return groupsList;
    }

    public async Task<string> DeleteForm(int id)
    {
      if (id <= 0)
      {
        throw new ArgumentException("O id não pode ser menor ou igual a zero!");
      }

      List<Questions> questionsList = await _questionsRepository.GetQuestionsByFormId(id);

      foreach (Questions question in questionsList)
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

    public async Task<string> UpdateForm(int id, Forms form)
    {
      if (string.IsNullOrWhiteSpace(form.Name))
      {
        throw new ArgumentException("O nome não pode ser vazio!");
      }

      if (id <= 0 || form.GroupId <= 0)
      {
        throw new ArgumentException("O Id/Id do grupo não podem ser menores ou iguais a zero!");
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
