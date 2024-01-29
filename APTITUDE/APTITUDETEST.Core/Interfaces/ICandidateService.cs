using AptitudeTest.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Core.Interfaces
{
    public interface ICandidateService
    {
        Task<JsonResult> CreateUserTest(CreateUserTestVM userTest);
        Task<JsonResult> CreateTempUserTest(int userId);
        Task<JsonResult> CreateUserTestResult(CreateUserTestResultVM userTestResult);
        Task<JsonResult> GetCandidateTestQuestion(int questionId, int userId);
        Task<JsonResult> GetQuestionsStatus(int userId);
        Task<JsonResult> SaveTestQuestionAnswer(UpdateTestQuestionAnswerVM userTestQuestionAnswer);
        Task<JsonResult> GetInstructionsOfTheTestForUser(int userId, string testStatus);
        Task<JsonResult> EndTest(int userId);
        Task<JsonResult> UpdateRemainingTime(UpdateTestTimeVM updateTestTimeVM);
    }
}
