
using AptitudeTest.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Core.Interfaces
{
    public interface ICandidateService
    {
        Task<JsonResult> CreateUserTest(CreateUserTestVM userTest);
        Task<JsonResult> CreateTempUserTest(CreateTempUserTestVM tempUserTest);
        Task<JsonResult> CreateUserTestResult(CreateUserTestResultVM userTestResult);
        Task<JsonResult> CreateTempUserTestResult(CreateUserTestResultVM tempUserTestResult);
    }
}
