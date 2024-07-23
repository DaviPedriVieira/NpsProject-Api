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

    public FormsGroupsCommandHandler(FormsGroupsRepository formsGroupsRepository, FormsRepository formsRepository, QuestionsRepository questionsRepository, AnswersRepository answersRepository)
    {
      _formsGroupsRepository = formsGroupsRepository;
      _formsRepository = formsRepository;
      _questionsRepository = questionsRepository;
      _answersRepository = answersRepository;
    }

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

      if (group is null)
      {
        throw new KeyNotFoundException($"Não existe nenhum grupo com o id = {id}!");
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

    public async Task<string> DeleteGroup(int id)
    {
      FormsGroup? group = await _formsGroupsRepository.GetGroupById(id);

      if (group is null)
      {
        throw new KeyNotFoundException($"Não existe nenhum grupo com o id = {id}!");
      }

      List<Form> formsInsideGroup = await _formsRepository.GetFormsByGroupId(id);

      foreach (Form form in formsInsideGroup)
      {
        List<Question> questionsList = await _questionsRepository.GetQuestionsByFormId(form.Id);

        foreach (Question question in questionsList)
        {
          if(question.Answers.Count > 0)
          {
            await _answersRepository.DeleteAnswersByQuestionId(question.Id);
          }

          await _questionsRepository.DeleteQuestion(question.Id);
        }

        await _formsRepository.DeleteForm(form.Id);
      }

      bool deleted = await _formsGroupsRepository.DeleteGroup(id);

      return deleted ? "Grupo excluído!" : "Não foi possível excluir o grupo!";
    }

    public async Task<string> UpdateGroup(int id, FormsGroup group)
    {
      FormsGroup? toUpdateGroup = await _formsGroupsRepository.GetGroupById(id);

      if (toUpdateGroup is null)
      {
        throw new KeyNotFoundException($"Não existe nenhum grupo com o id = {id}!");
      }

      if (string.IsNullOrWhiteSpace(group.Name))
      {
        throw new ArgumentNullException("group.Name", "O nome não pode ser vazio!");
      }

      bool updated = await _formsGroupsRepository.UpdateGroup(id, group);

      return updated ? "Grupo editado!" : "Não foi possível editar o grupo!";
    }
  }
}
