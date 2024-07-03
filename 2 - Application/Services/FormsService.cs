using NpsApi.Data;
using NpsApi.Models;
using NpsApi.Repositories;
using System.Data.SqlClient;

namespace NpsApi.Application.Services
{
  public class FormsService
  {
    private readonly FormsRepository _formsRepository;
    private readonly QuestionsRepository _questionsRepository;


    public FormsService(FormsRepository repository, QuestionsRepository questionsRepository)
    {
      _formsRepository = repository;
      _questionsRepository = questionsRepository;
    }

    public async Task<Forms> Create(Forms form)
    {
      if (string.IsNullOrWhiteSpace(form.Name))
      {
        throw new ArgumentException("O nome não pode ser vazio!");
      }

      Forms newForm = new Forms
      {
        Name = form.Name,
        GroupId = form.GroupId,
      };

      newForm.Id = await _formsRepository.Create(newForm);

      return newForm;
    }

    public async Task<Forms> GetById(int id)
    {
      if (id <= 0)
      {
        throw new ArgumentException("O id não pode ser menor ou igual a zero!");
      }

      Forms? form = await _formsRepository.GetById(id);

      if (form == null)
      {
        throw new KeyNotFoundException($"Não foi encontrado nenhum forulário com o Id = {id}!");
      }

      form.Questions = await _questionsRepository.GetByFormId(form.Id);

      return form;
    }

    public async Task<List<Forms>> Get()
    {
      List<Forms> groupsList = await _formsRepository.Get();

      if (!groupsList.Any())
      {
        throw new ArgumentException("Não há formulários cadastrados!");
      }

      return groupsList;
    }

    public async Task<string> Delete(int id)
    {
      bool deleted = await _formsRepository.Delete(id);

      if (!deleted)
      {
        return "Não foi possível excluir o formulário!";
      }

      return "Formulário excluído com sucesso";
    }

    public async Task<string> Update(int id, Forms form)
    {
      if (string.IsNullOrWhiteSpace(form.Name))
      {
        throw new ArgumentException("O nome não pode ser vazio!");
      }

      if (id <= 0 || form.GroupId <= 0)
      {
        throw new ArgumentException("O Id/Id do grupo não podem ser menores ou iguais a zero!");
      }

      bool edited = await _formsRepository.Delete(id);

      if (!edited)
      {
        return "Não foi possível editar o formulário!";
      }

      return "Formulário editado com sucesso!";
    }
  }
}
