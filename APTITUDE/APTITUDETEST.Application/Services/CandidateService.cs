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
        public async Task<JsonResult> CreateTempUserTest(CreateTempUserTestVM tempUserTest)
        {
            return await _candidateRepository.CreateTempUserTest(tempUserTest);
        }
        public async Task<JsonResult> CreateUserTestResult(CreateUserTestResultVM userTestResult)
        {
            return await _candidateRepository.CreateUserTestResult(userTestResult);
        }
        public async Task<JsonResult> CreateTempUserTestResult(CreateUserTestResultVM tempUserTestResult)
        {
            return await _candidateRepository.CreateTempUserTestResult(tempUserTestResult);
        }
        #endregion
    }
}
