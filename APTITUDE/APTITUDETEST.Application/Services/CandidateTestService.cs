using AptitudeTest.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Application.Services
{
    public class CandidateTestService : ICandidateTestService
    {
        #region Properties
        private readonly ICandidateTestRepository _candidateTestRepository;
        #endregion

        #region Constructor
        public CandidateTestService(ICandidateTestRepository candidateRepository)
        {
            _candidateTestRepository = candidateRepository;
        }
        #endregion

        #region Methods
        public async Task<JsonResult> GetCandidateTestQuestion(int questionId, int userId, int testId)
        {
            return await _candidateTestRepository.GetCandidateTestQuestion(questionId, userId, testId);
        }
        #endregion
    }
}
