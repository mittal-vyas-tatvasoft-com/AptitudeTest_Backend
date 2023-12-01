
using AptitudeTest.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Core.Interfaces
{
    public interface ICandidateRepository
    {
        Task<JsonResult> CreateUserTest(CreateUserTestVM userTest);
        Task<JsonResult> CreateTempUserTest(int userId);
        Task<JsonResult> CreateUserTestResult(CreateUserTestResultVM userTestResult);
        Task<JsonResult> GetCandidateTestQuestion(int questionId, int userId, int testId);
        Task<JsonResult> SaveTestQuestionAnswer(UpdateTestQuestionAnswerVM userTestQuestionAnswer);
    }
}
