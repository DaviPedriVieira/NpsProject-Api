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
                throw new Exception("O nome do grupo não pode ser vazio!");
            }

            foreach (Form form in group.Forms)
            {
                if (string.IsNullOrWhiteSpace(form.Name) || form.Questions.Where(question => question.Content.Trim() == "").Any())
                {
                    throw new Exception("Nenhum formulário ou pergunta pode ser vazio!");
                }
            }

            FormsGroup newGroup = await _formsGroupsRepository.CreateGroup(group);

            foreach (Form form in newGroup.Forms)
            {
                form.GroupId = newGroup.Id;
                await _formsRepository.CreateForm(form);

                foreach (Question question in form.Questions)
                {
                    question.FormId = form.Id;
                }

                await _questionsRepository.CreateQuestion(form.Questions.ToList());
            }

            return newGroup;
        }

        public async Task<FormsGroup> GetGroupById(int id)
        {
            FormsGroup? group = await _formsGroupsRepository.GetGroupById(id);

            if (group is null)
            {
                throw new Exception($"Não existe nenhum grupo com o id = {id}!");
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
            return await _formsGroupsRepository.GetGroups();
        }

        public async Task<bool> DeleteGroup(int id)
        {
            FormsGroup? group = await _formsGroupsRepository.GetGroupById(id);

            if (group is null)
            {
                throw new Exception($"Não existe nenhum grupo com o id = {id}!");
            }

            List<int> formIdsList = await _formsRepository.GetFormsByGroupIds(id);

            if (formIdsList.Any())
            {
                List<int> questionIdsList = await _questionsRepository.GetQuestionsIdByFormIds(formIdsList);

                if (questionIdsList.Any())
                {
                    await _answersRepository.DeleteAnswersByQuestionId(questionIdsList);

                    await _questionsRepository.DeleteQuestions(questionIdsList);
                }
                await _formsRepository.DeleteForms(formIdsList);
            }

            return await _formsGroupsRepository.DeleteGroup(id);
        }

        public async Task<bool> UpdateGroup(int id, string newName)
        {
            FormsGroup? toUpdateGroup = await _formsGroupsRepository.GetGroupById(id);

            if (toUpdateGroup is null)
            {
                throw new Exception($"Não existe nenhum grupo com o id = {id}!");
            }

            if (string.IsNullOrWhiteSpace(newName))
            {
                throw new Exception("O nome não pode ser vazio!");
            }

            return await _formsGroupsRepository.UpdateGroup(id, newName);
        }
    }
}
