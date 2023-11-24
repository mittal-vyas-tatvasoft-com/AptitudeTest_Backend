using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using AptitudeTest.Data.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Controllers
{
    [Authorize]
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

        [HttpPost("[action]")]
        public async Task<JsonResult> CreateTempUserTest(CreateTempUserTestVM tempUserTest)
        {
            if (ModelState.IsValid)
            {
                return await _candidateService.CreateTempUserTest(tempUserTest);
            }

            return new JsonResult(new ApiResponse<string>() { Message = ResponseMessages.BadRequest, Result = false, StatusCode = ResponseStatusCode.BadRequest });
        }

    }
}
