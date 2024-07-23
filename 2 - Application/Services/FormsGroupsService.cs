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
      return await _formsGroupsCommandHandler.CreateGroup(group);
    }

    public async Task<FormsGroup> GetGroupById(int id)
    {
      return await _formsGroupsCommandHandler.GetGroupById(id);
    }

    public async Task<List<FormsGroup>> GetGroups()
    {
      return await _formsGroupsCommandHandler.GetGroups();
    }

    public async Task<string> DeleteGroup(int id)
    {
      return await _formsGroupsCommandHandler.DeleteGroup(id);
    }

    public async Task<string> UpdateGroup(int id, FormsGroup group)
    {
      return await _formsGroupsCommandHandler.UpdateGroup(id, group);
    }

  }
}
