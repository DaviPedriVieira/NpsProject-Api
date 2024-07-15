using NpsApi.Models;
using NpsApi.Repositories;

namespace NpsApi._3___Domain.CommandHandlers
{
  public class FormsGroupsCommandHandler
  {
    private readonly FormsGroupsRepository _formsGroupsRepository;
    private readonly FormsRepository _formsRepository;
    private readonly QuestionsRepository _questionsRepository;
    private readonly AnswersRepository _answersRepository;

    public FormsGroupsCommandHandler(FormsGroupsRepository repository, FormsRepository formsRepository, QuestionsRepository questionsRepository, AnswersRepository answersRepository)
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
      List<FormsGroup> groupsList = await _formsGroupsRepository.GetGroups();

      if (groupsList.Find(group => group.Id == id) == null)
      {
        throw new KeyNotFoundException($"Não foi encontrado nenhum grupo com o Id = {id}!");
      }

      FormsGroup group = await _formsGroupsRepository.GetGroupById(id);

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

    public async Task<bool> DeleteGroup(int id)
    {
      List<FormsGroup> groupsList = await _formsGroupsRepository.GetGroups();

      if (groupsList.Find(group => group.Id == id) == null)
      {
        throw new KeyNotFoundException($"Não foi encontrado nenhum grupo com o Id = {id}!");
      }

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

      return deleted;
    }

    public async Task<bool> UpdateGroup(int id, FormsGroup group)
    {
      List<FormsGroup> groupsList = await _formsGroupsRepository.GetGroups();

      if (groupsList.Find(group => group.Id == id) == null)
      {
        throw new KeyNotFoundException($"Não foi encontrado nenhum grupo com o Id = {id}!");
      }

      if (string.IsNullOrWhiteSpace(group.Name))
      {
        throw new ArgumentException("O nome não pode ser vazio!");
      }

      bool updated = await _formsGroupsRepository.UpdateGroup(id, group);

      return updated;
    }
  }
}
