using NpsApi.Models;
using NpsApi.Repositories;

namespace NpsApi.Application.Services
{
  public class FormsGroupsService
  {
    private readonly FormsGroupsRepository _formsGroupsRepository;
    private readonly FormsRepository _formsRepository;
    private readonly QuestionsRepository _questionsRepository;
    private readonly AnswersRepository _answersRepository;

    public FormsGroupsService(FormsGroupsRepository repository, FormsRepository formsRepository, QuestionsRepository questionsRepository, AnswersRepository answersRepository)
    {
      _formsGroupsRepository = repository;
      _formsRepository = formsRepository;
      _questionsRepository = questionsRepository;
      _answersRepository = answersRepository;
    }

    public async Task<FormsGroup> CreateGroup(FormsGroup group)
    {
      if (string.IsNullOrWhiteSpace(group.Name))
      {
        throw new ArgumentException("O nome não pode ser vazio!");
      }

      FormsGroup newGroup = await _formsGroupsRepository.CreateGroup(group);

      return newGroup;
    }

    public async Task<FormsGroup> GetGroupById(int id)
    {
      FormsGroup? group = await _formsGroupsRepository.GetGroupById(id);

      if (group == null)
      {
        throw new KeyNotFoundException($"Não foi encontrado nenhum grupo com o Id = {id}!");
      }

      group.Forms = await _formsRepository.GetFormsByGroupId(group.Id);

      foreach (Form form in group.Forms)
      {
        form.Questions = await _questionsRepository.GetQuestionsByFormId(form.Id);
      }

      return group;
    }

    public async Task<List<FormsGroup>> GetGroups()
    {
      List<FormsGroup> groupsList = await _formsGroupsRepository.GetGroups();

      if (!groupsList.Any())
      {
        throw new ArgumentException("Não há grupos cadastrados!");
      }

      return groupsList;
    }

    public async Task<string> DeleteGroup(int id)
    {
      List<Form> formsInsideGroup = await _formsRepository.GetFormsByGroupId(id);

      foreach (Form form in formsInsideGroup)
      {
        List<Question> questionsList = await _questionsRepository.GetQuestionsByFormId(form.Id);

        foreach (Question question in questionsList)
        {
          await _answersRepository.DeleteAnswersByQuestionId(question.Id);

          await _questionsRepository.DeleteQuestion(question.Id);
        }

        await _formsRepository.DeleteForm(form.Id);
      }

      bool deleted = await _formsGroupsRepository.DeleteGroup(id);

      if (!deleted)
      {
        return "Não foi possível excluir o grupo!";
      }

      return "Grupo excluído!";
    }

    public async Task<string> UpdateGroup(int id, FormsGroup group)
    {
      if (string.IsNullOrWhiteSpace(group.Name))
      {
        throw new ArgumentException("O nome não pode ser vazio!");
      }

      bool edited = await _formsGroupsRepository.UpdateGroup(id, group);

      if (!edited)
      {
        return "Não foi possível editar!";
      }

      return "Grupo editado!";
    }

  }
}
