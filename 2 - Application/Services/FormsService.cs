using NpsApi._3___Domain.CommandHandlers;
using NpsApi.Models;

namespace NpsApi.Application.Services
{
  public class FormsService
  {
    private readonly FormsCommandHandler _formsCommandHandler;

    public FormsService(FormsCommandHandler formsCommandHandler)
    {
      _formsCommandHandler = formsCommandHandler;
    }

    public async Task<Form> CreateForm(Form form)
    {
      return await _formsCommandHandler.CreateForm(form);
    }

    public async Task<Form> GetFormById(int id)
    {
      return await _formsCommandHandler.GetFormById(id);
    }

    public async Task<List<Form>> GetForms()
    {
      return await _formsCommandHandler.GetForms();
    }

    public async Task<bool> DeleteForm(int id)
    {
      return await _formsCommandHandler.DeleteForm(id);
    }

    public async Task<bool> UpdateForm(int id, string newName)
    {
      return await _formsCommandHandler.UpdateForm(id, newName);
    }

        public async Task<List<Form>> GetFormsByGroupId(int groupId)
        {
            return await _formsCommandHandler.GetFormsByGroupId(groupId);
        }
    }
}
