using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using AptitudeTest.Data.Common;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CandidatesController : ControllerBase
    {
        #region Properties
        private readonly ICandidateService _candidateService;
        #endregion

        #region Constructor
        public CandidatesController(ICandidateService candidateService)
        {
            _candidateService = candidateService;
        }
        #endregion

        [HttpPost("[action]")]
        public async Task<JsonResult> CreateUserTest(CreateUserTestVM userTest)
        {
            if (ModelState.IsValid)
            {
                return await _candidateService.CreateUserTest(userTest);
            }

            return new JsonResult(new ApiResponse<string>() { Message = ResponseMessages.BadRequest, Result = false, StatusCode = ResponseStatusCode.BadRequest });
        }

        [HttpPost("[action]/{userId:int}")]
        public async Task<JsonResult> StartUserTest(int userId)
        {
            if (ModelState.IsValid)
            {
                return await _candidateService.CreateTempUserTest(userId);
            }

            return new JsonResult(new ApiResponse<string>() { Message = ResponseMessages.BadRequest, Result = false, StatusCode = ResponseStatusCode.BadRequest });
        }

        [HttpPost("[action]")]
        public async Task<JsonResult> CreateTempUserTestResult(CreateUserTestResultVM tempUserTestResult)
        {
            if (ModelState.IsValid)
            {
                return await _candidateService.CreateTempUserTestResult(tempUserTestResult);
            }

            return new JsonResult(new ApiResponse<string>() { Message = ResponseMessages.BadRequest, Result = false, StatusCode = ResponseStatusCode.BadRequest });
        }

        [HttpPost("[action]")]
        public async Task<JsonResult> CreateUserTestResult(CreateUserTestResultVM userTestResult)
        {
            if (ModelState.IsValid)
            {
                return await _candidateService.CreateUserTestResult(userTestResult);
            }

            return new JsonResult(new ApiResponse<string>() { Message = ResponseMessages.BadRequest, Result = false, StatusCode = ResponseStatusCode.BadRequest });
        }

        [HttpGet("[action]/{questionId}/{userId}/{testId}")]
        public async Task<JsonResult> GetCandidateTestQuestion(int questionId, int userId, int testId)
        {
            return await _candidateService.GetCandidateTestQuestion(questionId, userId, testId);
        }

    }
}
