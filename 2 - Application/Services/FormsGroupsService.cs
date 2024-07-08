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

    public async Task<FormsGroups> CreateGroup(FormsGroups group)
    {
      if (string.IsNullOrWhiteSpace(group.Name))
      {
        throw new ArgumentException("O nome não pode ser vazio!");
      }

      FormsGroups newGroup = await _formsGroupsRepository.CreateGroup(group);

      return newGroup;
    }

    public async Task<FormsGroups> GetGroupById(int id)
    {
      if (id <= 0)
      {
        throw new ArgumentException("O id não pode ser menor ou igual a zero!");
      }

      FormsGroups? group = await _formsGroupsRepository.GetGroupById(id);

      if (group == null)
      {
        throw new KeyNotFoundException($"Não foi encontrado nenhum grupo com o Id = {id}!");
      }

      group.Forms = await _formsRepository.GetFormsByGroupId(group.Id);

      foreach (Forms form in group.Forms)
      {
        form.Questions = await _questionsRepository.GetQuestionsByFormId(form.Id);

        foreach (Questions question in form.Questions)
        {
          question.Answers = await _answersRepository.GetAnswersByQuestionId(question.Id);
        }
      }

      return group;
    }

    public async Task<List<FormsGroups>> GetGroups()
    {
      List<FormsGroups> groupsList = await _formsGroupsRepository.GetGroups();

      if (!groupsList.Any())
      {
        throw new ArgumentException("Não há grupos cadastrados!");
      }

      return groupsList;
    }

    public async Task<string> DeleteGroup(int id)
    {
      if (id <= 0)
      {
        throw new ArgumentException("O id não pode ser menor ou igual a zero!");
      }

      List<Forms> formsInsideGroup = await _formsRepository.GetFormsByGroupId(id);

      foreach (Forms form in formsInsideGroup)
      {
        List<Questions> questionsList = await _questionsRepository.GetQuestionsByFormId(form.Id);

        foreach (Questions question in questionsList)
        {
          await _answersRepository.DeleteAnswersByQuestionId(question.Id);
        }

        await _questionsRepository.DeleteQuestionByFormId(form.Id);
        await _formsRepository.DeleteForm(form.Id);
      }

      bool deleted = await _formsGroupsRepository.DeleteGroup(id);

      if (!deleted)
      {
        return "Não foi possível excluir o grupo!";
      }

      return "Grupo excluído!";
    }

    public async Task<string> UpdateGroup(int id, FormsGroups group)
    {
      if (string.IsNullOrWhiteSpace(group.Name))
      {
        throw new ArgumentException("O nome não pode ser vazio!");
      }

      if (id <= 0)
      {
        throw new ArgumentException("O id não pode ser menor ou igual a zero!");
      }

      bool edited = await _formsGroupsRepository.DeleteGroup(id);

      if (!edited)
      {
        return "Não foi possível editar!";
      }

      return "Grupo editado!";
    }

  }
}
