using NpsApi.Models;
using NpsApi.Repositories;

namespace NpsApi.Application.Services
{
  public class FormsGroupsService
  {
    private readonly FormsGroupsRepository _formsGroupsRepository;
    private readonly FormsRepository _formsRepository;
    private readonly QuestionsRepository _questionsRepository;

    public FormsGroupsService(FormsGroupsRepository repository, FormsRepository formsRepository, QuestionsRepository questionsRepository)
    {
      _formsGroupsRepository = repository;
      _formsRepository = formsRepository;
      _questionsRepository = questionsRepository;
    }

    public async Task<FormsGroups> Create(string name)
    {
      if (string.IsNullOrWhiteSpace(name))
      {
        throw new ArgumentException("O nome não pode ser vazio!");
      }

      FormsGroups group = new FormsGroups
      {
        Name = name,
      };

      group.Id = await _formsGroupsRepository.Create(group);

      return group;
    }

    public async Task<FormsGroups> GetById(int id)
    {
      if (id <= 0)
      {
        throw new ArgumentException("O id não pode ser menor ou igual a zero!");
      }

      FormsGroups? group = await _formsGroupsRepository.GetById(id);

      if (group == null)
      {
        throw new KeyNotFoundException($"Não foi encontrado nenhum grupo com o Id = {id}!");
      }

      group.Forms = await _formsRepository.GetByGroupId(group.Id);

      foreach (Forms form in group.Forms)
      {
        form.Questions = await _questionsRepository.GetByFormId(form.Id);
      }

      return group;
    }

    public async Task<List<FormsGroups>> Get()
    {
      List<FormsGroups> groupsList = await _formsGroupsRepository.Get();

      if (!groupsList.Any())
      {
        throw new ArgumentException("Não há grupos cadastrados!");
      }

      return groupsList;
    }

    public async Task<string> Delete(int id)
    {
      if (id <= 0)
      {
        throw new ArgumentException("O id não pode ser menor ou igual a zero!");
      }

      bool deleted = await _formsGroupsRepository.Delete(id);

      if (!deleted)
      {
        return "Não foi possível excluir o grupo!";
      }

      return "Grupo excluído com sucesso";
    }

    public async Task<string> Update(int id, FormsGroups group)
    {
      if (string.IsNullOrWhiteSpace(group.Name))
      {
        throw new ArgumentException("O nome não pode ser vazio!");
      }

      if (id <= 0)
      {
        throw new ArgumentException("O id não pode ser menor ou igual a zero!");
      }

      bool edited = await _formsGroupsRepository.Delete(id);

      if (!edited)
      {
        return "Não foi possível editar!";
      }

      return "Editado com sucesso!";
    }

  }
}
