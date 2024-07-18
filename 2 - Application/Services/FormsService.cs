using NpsApi._3___Domain.CommandHandlers;
using NpsApi.Models;

namespace NpsApi.Application.Services
{
  public class FormsService(FormsCommandHandler formsCommandHandler)
  {
    private readonly FormsCommandHandler _formsCommandHandler = formsCommandHandler;

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

    public async Task<bool> UpdateForm(int id, Form form)
    {
      return await _formsCommandHandler.UpdateForm(id, form);
    }
  }
}
