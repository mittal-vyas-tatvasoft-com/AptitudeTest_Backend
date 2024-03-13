using AptitudeTest.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Core.Interfaces
{
    public interface ICandidateService
    {
        Task<JsonResult> GetTempUserTest(int userId);
        Task<JsonResult> GetCandidateTestQuestion(int questionId, int userId);
        Task<JsonResult> GetQuestionsStatus(int userId, bool isRefresh);
        Task<JsonResult> SaveTestQuestionAnswer(UpdateTestQuestionAnswerVM userTestQuestionAnswer);
        Task<JsonResult> GetInstructionsOfTheTestForUser(int userId, string testStatus);
        Task<JsonResult> EndTest(int userId);
        Task<JsonResult> UpdateRemainingTime(UpdateTestTimeVM updateTestTimeVM);
        Task<JsonResult> UpdateQuestionTimer(QuestionTimerVM questionTimerDetails);
        Task<JsonResult> UpdateUserTestStatus(UpdateUserTestStatusVM updateUserTestStatusVM);
    }
}
