using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Core.Interfaces
{
    public interface ICandidateTestService
    {
        Task<JsonResult> GetCandidateTestQuestion(int questionId, int userId, int testId);
    }
}
