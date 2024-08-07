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
        throw new Exception($"Erro na FK form.GroupId, não foi encontrado nenhum grupo com o Id = {form.GroupId}!");
      }

      if (string.IsNullOrWhiteSpace(form.Name))
      {
        throw new Exception("O nome do formulário não pode ser vazio!");
      }

      if(form.Questions.Where(question => question.Content.Trim() == "").Any())
      {
        throw new Exception("Nenhuma pergunta pode ser vazia!");
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

      if (form is null)
      {
        throw new Exception($"Não foi encontrado nenhum formulário com o Id = {id}!");
      }

      form.Questions = await _questionsRepository.GetQuestionsByFormId(form.Id);

      return form;
    }

    public async Task<List<Form>> GetForms()
    {
      return await _formsRepository.GetForms();
    }

    public async Task<bool> DeleteForm(int id)
    {
      Form? form = await _formsRepository.GetFormById(id);

      if (form is null)
      {
        throw new Exception($"Não foi encontrado nenhum formulário com o Id = {id}!");
      }

      List<Question> questionsList = await _questionsRepository.GetQuestionsByFormId(id);

      foreach (Question question in questionsList)
      {
        await _answersRepository.DeleteAnswersByQuestionId(question.Id);

        await _questionsRepository.DeleteQuestion(question.Id);
      }

      return await _formsRepository.DeleteForm(id);
    }

    public async Task<bool> UpdateForm(int id, Form form)
    {
      Form? toUpdateForm = await _formsRepository.GetFormById(id);

      if (toUpdateForm is null)
      {
        throw new Exception($"Não foi encontrado nenhum formulário com o Id = {id}!");
      }

      if (string.IsNullOrWhiteSpace(form.Name))
      {
        throw new Exception("O nome do formulário não pode ser vazio!");
      }

      return await _formsRepository.UpdateForm(id, form);
    }
  }
}
