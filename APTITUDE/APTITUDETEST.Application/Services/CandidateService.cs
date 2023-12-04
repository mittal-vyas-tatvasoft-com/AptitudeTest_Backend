using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Application.Services
{
    public class CandidateService : ICandidateService
    {
        #region Properties
        private readonly ICandidateRepository _candidateRepository;
        #endregion

        #region Constructor
        public CandidateService(ICandidateRepository candidateRepository)
        {
            _candidateRepository = candidateRepository;
        }
        #endregion

        #region Methods
        public async Task<JsonResult> CreateUserTest(CreateUserTestVM userTest)
        {
            return await _candidateRepository.CreateUserTest(userTest);
        }
        public async Task<JsonResult> CreateTempUserTest(int userId)
        {
            return await _candidateRepository.CreateTempUserTest(userId);
        }
        public async Task<JsonResult> CreateUserTestResult(CreateUserTestResultVM userTestResult)
        {
            return await _candidateRepository.CreateUserTestResult(userTestResult);
        }
        public async Task<JsonResult> GetCandidateTestQuestion(int questionId, int userId)
        {
            return await _candidateRepository.GetCandidateTestQuestion(questionId, userId);
        }

        public async Task<JsonResult> GetQuestionsStatus(int userId)
        {
            return await _candidateRepository.GetQuestionsStatus(userId);
        }

        public async Task<JsonResult> SaveTestQuestionAnswer(UpdateTestQuestionAnswerVM userTestQuestionAnswer)
        {
            return await _candidateRepository.SaveTestQuestionAnswer(userTestQuestionAnswer);
        }
        #endregion
    }
}
