using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using AptitudeTest.Data.Common;
using AptitudeTest.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(SessionIdCheckFilterAttribute))]
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

        /// <summary>
        /// starts the test for user and random questions will be stored in DB
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
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
        public async Task<JsonResult> CreateUserTestResult(CreateUserTestResultVM userTestResult)
        {
            if (ModelState.IsValid)
            {
                return await _candidateService.CreateUserTestResult(userTestResult);
            }

            return new JsonResult(new ApiResponse<string>() { Message = ResponseMessages.BadRequest, Result = false, StatusCode = ResponseStatusCode.BadRequest });
        }

        [HttpGet("[action]/{questionId}/{userId}")]
        public async Task<JsonResult> GetCandidateTestQuestion(int questionId, int userId)
        {
            return await _candidateService.GetCandidateTestQuestion(questionId, userId);
        }

        [HttpGet("[action]/{userId}")]
        public async Task<JsonResult> GetQuestionsStatus(int userId)
        {
            return await _candidateService.GetQuestionsStatus(userId);
        }

        /// <summary>
        /// saves the answer of test question given by user 
        /// </summary>
        /// <param name="userTestQuestionAnswer"></param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public async Task<JsonResult> SaveTestQuestionAnswer(UpdateTestQuestionAnswerVM userTestQuestionAnswer)
        {
            if (ModelState.IsValid)
            {
                return await _candidateService.SaveTestQuestionAnswer(userTestQuestionAnswer);
            }

            return new JsonResult(new ApiResponse<string>() { Message = ResponseMessages.BadRequest, Result = false, StatusCode = ResponseStatusCode.BadRequest });
        }

        /// <summary>
        /// GetInstructions For theTest for user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("[action]/{userId}/{testStatus}")]
        public async Task<JsonResult> GetInstructionsOfTheTestForUser(int userId, string testStatus)
        {
            return await _candidateService.GetInstructionsOfTheTestForUser(userId, testStatus);
        }
        /// <summary>
        /// end the test of candidate
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPut("[action]/{userId:int}")]
        public async Task<JsonResult> EndTest(int userId)
        {
            return await _candidateService.EndTest(userId);
        }

    }
}
