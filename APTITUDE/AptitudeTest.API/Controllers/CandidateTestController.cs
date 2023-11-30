using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using AptitudeTest.Data.Common;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidateTestController : ControllerBase
    {
        #region Properties
        private readonly ICandidateTestService _candidateTestService;
        #endregion

        #region Constructor
        public CandidateTestController(ICandidateTestService candidateService)
        {
            _candidateTestService = candidateService;
        }
        #endregion

        [HttpGet("[action]/{questionId}/{userId}/{testId}")]
        public async Task<JsonResult> GetCandidateTestQuestion(int questionId, int userId, int testId)
        {
            if (questionId != 0)
            {
                return await _candidateTestService.GetCandidateTestQuestion(questionId, userId, testId);
            }

            return new JsonResult(new ApiResponse<string>() { Message = ResponseMessages.BadRequest, Result = false, StatusCode = ResponseStatusCode.BadRequest });
        }
    }
}
