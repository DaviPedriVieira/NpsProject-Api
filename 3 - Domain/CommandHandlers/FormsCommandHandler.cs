using NpsApi.Models;
using NpsApi.Repositories;

namespace NpsApi._3___Domain.CommandHandlers
{
  public class FormsCommandHandler
  {
    private readonly FormsGroupsRepository _formsGroupsRepository;
    private readonly FormsRepository _formsRepository;
    private readonly QuestionsRepository _questionsRepository;
    private readonly AnswersRepository _answersRepository;

    public FormsCommandHandler(FormsGroupsRepository formsGroupsRepository, FormsRepository formsRepository, QuestionsRepository questionsRepository, AnswersRepository answersRepository)
    {
      _answersRepository = answersRepository;
      _questionsRepository = questionsRepository;
      _formsGroupsRepository = formsGroupsRepository;
      _formsRepository = formsRepository;
    }

    public async Task<Form> CreateForm(Form form)
    {
      FormsGroup? group = await _formsGroupsRepository.GetGroupById(form.GroupId);

      if (group is null)
      {
        throw new KeyNotFoundException($"Erro na FK form.GroupId, não foi encontrado nenhum grupo com o Id = {form.GroupId}!");
      }

      if (string.IsNullOrWhiteSpace(form.Name))
      {
        throw new ArgumentNullException("form.Name", "O nome não pode ser vazio!");
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

      if(form is null)
      {
        throw new KeyNotFoundException($"Não foi encontrado nenhum formulário com o Id = {id}!");
      }

      form.Questions = await _questionsRepository.GetQuestionsByFormId(form.Id);

      return form;
    }

    public async Task<List<Form>> GetForms()
    {
      List<Form> groupsList = await _formsRepository.GetForms();

      if (groupsList.Count == 0)
      {
        throw new Exception("Não há formulários cadastrados!");
      }

      return groupsList;
    }

    public async Task<string> DeleteForm(int id)
    {
      Form? form = await _formsRepository.GetFormById(id);

      if (form is null)
      {
        throw new KeyNotFoundException($"Não foi encontrado nenhum formulário com o Id = {id}!");
      }

      List<Question> questionsList = await _questionsRepository.GetQuestionsByFormId(id);

      foreach (Question question in questionsList)
      {
        if (question.Answers.Count > 0)
        {
          await _answersRepository.DeleteAnswersByQuestionId(question.Id);
        }

        await _questionsRepository.DeleteQuestion(question.Id);
      }

      bool deleted = await _formsRepository.DeleteForm(id);

      return deleted ? "Formulário excluído!" : "Não foi possível excluir o formulário!";
    }

    public async Task<string> UpdateForm(int id, Form form)
    {
      Form? toUpdateForm = await _formsRepository.GetFormById(id);

      if (toUpdateForm is null)
      {
        throw new KeyNotFoundException($"Não foi encontrado nenhum formulário com o Id = {id}!");
      }

      FormsGroup? group = await _formsGroupsRepository.GetGroupById(form.GroupId);

      if (group is null)
      {
        throw new KeyNotFoundException($"Erro na FK form.GroupId, não foi encontrado nenhum grupo com o Id = {form.GroupId}!");
      }

      if (string.IsNullOrWhiteSpace(form.Name))
      {
        throw new ArgumentNullException("form.Name", "O nome não pode ser vazio!");
      }

      bool updated = await _formsRepository.UpdateForm(id, form);

      return updated ? "Formulário editado!" : "Não foi possível editar o formulário!";
    }
  }
}
