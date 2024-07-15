using NpsApi._3___Domain.CommandHandlers;
using NpsApi.Models;

namespace NpsApi.Application.Services
{
  public class FormsGroupsService
  {
    private readonly FormsGroupsCommandHandler _formsGroupsCommandHandler;

    public FormsGroupsService(FormsGroupsCommandHandler formsGroupsCommandHandler)
    {
      _formsGroupsCommandHandler = formsGroupsCommandHandler;
    }

    public async Task<FormsGroup> CreateGroup(FormsGroup group)
    {
      if (string.IsNullOrWhiteSpace(group.Name))
      {
        throw new ArgumentException("O nome não pode ser vazio!");
      }

      FormsGroup newGroup = await _formsGroupsCommandHandler.CreateGroup(group);

      return newGroup;
    }

    public async Task<FormsGroup> GetGroupById(int id)
    {
      FormsGroup group = await _formsGroupsCommandHandler.GetGroupById(id);

      return group;
    }

    public async Task<List<FormsGroup>> GetGroups()
    {
      List<FormsGroup> groupsList = await _formsGroupsCommandHandler.GetGroups();

      return groupsList;
    }

    public async Task<bool> DeleteGroup(int id)
    {
      bool deleted = await _formsGroupsCommandHandler.DeleteGroup(id);

      return deleted;
    }

    public async Task<bool> UpdateGroup(int id, FormsGroup group)
    {
      bool updated = await _formsGroupsCommandHandler.UpdateGroup(id, group);

      return updated;
    }

  }
}
