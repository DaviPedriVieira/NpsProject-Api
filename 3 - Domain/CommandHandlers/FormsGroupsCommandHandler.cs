using NpsApi.Models;
using NpsApi.Repositories;

namespace NpsApi._3___Domain.CommandHandlers
{
  public class FormsGroupsCommandHandler(FormsGroupsRepository repository, FormsRepository formsRepository, QuestionsRepository questionsRepository, AnswersRepository answersRepository)
  {
    private readonly FormsGroupsRepository _formsGroupsRepository = repository;
    private readonly FormsRepository _formsRepository = formsRepository;
    private readonly QuestionsRepository _questionsRepository = questionsRepository;
    private readonly AnswersRepository _answersRepository = answersRepository;

    public async Task<FormsGroup> CreateGroup(FormsGroup group)
    {
      if (string.IsNullOrWhiteSpace(group.Name))
      {
        throw new ArgumentNullException("group.Name", "O nome não pode ser vazio!");
      }

      return await _formsGroupsRepository.CreateGroup(group);
    }

    public async Task<FormsGroup> GetGroupById(int id)
    {
      FormsGroup? group = await _formsGroupsRepository.GetGroupById(id);

      if (group == null)
      {
        throw new KeyNotFoundException($"Não existe nenhum grupo com o id = {id}");
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

      if (groupsList.Count == 0)
      {
        throw new Exception("Não há grupos cadastrados!");
      }

      return groupsList;
    }

    public async Task<bool> DeleteGroup(int id)
    {
      FormsGroup? group = await _formsGroupsRepository.GetGroupById(id);

      if (group == null)
      {
        throw new KeyNotFoundException($"Não existe nenhum grupo com o id = {id}");
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

      return await _formsGroupsRepository.DeleteGroup(id);
    }

    public async Task<bool> UpdateGroup(int id, FormsGroup group)
    {
      FormsGroup? toUpdateGroup = await _formsGroupsRepository.GetGroupById(id);

      if (toUpdateGroup == null)
      {
        throw new KeyNotFoundException($"Não existe nenhum grupo com o id = {id}");
      }

      if (string.IsNullOrWhiteSpace(group.Name))
      {
        throw new ArgumentNullException("group.Name", "O nome não pode ser vazio!");
      }

      return await _formsGroupsRepository.UpdateGroup(id, group);
    }
  }
}
