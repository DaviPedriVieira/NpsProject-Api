using NpsApi.Models;
using NpsApi.Repositories;

namespace NpsApi._3___Domain.CommandHandlers
{
    public class NpsCommandHandler
    {
        private readonly AnswersRepository _answersRepository;

        public NpsCommandHandler(AnswersRepository answersRepository)
        {
            _answersRepository = answersRepository;
        }

        public async Task<int> GetNpsScore()
        {
            List<Answer> answersList = await _answersRepository.GetAnswers();

            if (answersList.Count == 0)
            {
                return 0;
            }

            List<Answer> detractors = answersList.Where(answer => answer.Grade <= 6).ToList();
            List<Answer> promoters = answersList.Where(answer => answer.Grade >= 9).ToList();

            float porcentagemDetractors = (float)detractors.Count / answersList.Count * 100;
            float porcentagemPromoters = (float)promoters.Count / answersList.Count * 100;

            float npsScore = porcentagemPromoters - porcentagemDetractors;

            return Convert.ToInt32(npsScore);
        }
    }
}

