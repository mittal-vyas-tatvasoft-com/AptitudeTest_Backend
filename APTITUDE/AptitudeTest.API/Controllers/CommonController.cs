using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using AptitudeTest.Data.Common;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly ICollegeService _collegeService;
        private readonly IProfileService _profileService;
        private readonly IUserService _userService;
        private readonly IStreamService _streamService;
        private readonly IDegreeService _degreeService;
        private readonly ITestService _testService;

        public CommonController(ICollegeService collegeService, IProfileService profileService, IUserService userService, IStreamService streamService, IDegreeService degreeService, ITestService testService)
        {
            _collegeService = collegeService;
            _profileService = profileService;
            _userService = userService;
            _streamService = streamService;
            _degreeService = degreeService;
            _testService = testService;
        }

        #region GetActiveColleges
        /// <summary>
        /// Get all active colleges for register.
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<JsonResult> GetActiveColleges()
        {
            return await _collegeService.GetActiveColleges();
        }
        #endregion

        #region GetActiveProfiles
        /// <summary>
        /// Get all active profiles for register.
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<JsonResult> GetActiveProfiles()
        {
            return await _profileService.GetActiveProfiles();
        }
        #endregion

        #region GetAllState
        /// <summary>
        /// GetAllState
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<JsonResult> GetAllState()
        {
            return await _userService.GetAllState();
        }
        #endregion

        #region GetAllActiveStreams
        /// <summary>
        /// Get All Active Streams
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<JsonResult> GetAllActiveStreams()
        {
            return await _streamService.GetAllActiveStreams();
        }
        #endregion

        #region GetActiveDegrees
        /// <summary>
        /// Get Active Degrees 
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<JsonResult> GetActiveDegrees()
        {
            return await _degreeService.GetActiveDegrees();
        }
        #endregion

        #region Update
        /// <summary>
        /// This method Registers User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<JsonResult> Register(UserVM user)
        {
            if (ModelState.IsValid)
            {
                return await _userService.RegisterUser(user);
            }

            return new JsonResult(new ApiResponse<string>() { Message = ResponseMessages.BadRequest, Result = false, StatusCode = ResponseStatusCode.BadRequest });
        }
        #endregion

        #region GetActiveColleges
        /// <summary>
        /// Get all tests for dropdown.
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<JsonResult> GetTestForDropdown()
        {
            return await _testService.GetTestsForDropdown();
        }
        #endregion




    }
}
