using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using AptitudeTest.Data.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ResultsController : ControllerBase
    {
        #region Properties
        private readonly IResultService _service;
        #endregion

        #region Constructor
        public ResultsController(IResultService service)
        {
            _service = service;
        }
        #endregion

        #region Methods
        /// <summary>
        /// It gives User Result
        /// </summary>
        /// <param name="id"></param>
        /// <param name="marks"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        [HttpGet("[action]/{id:int}/{testId:int}/{marks:int}/{pageSize:int}/{pageIndex:int}/{onlyCorrect:bool}")]
        public async Task<JsonResult> Get(int id, int testId, int marks, int pageSize, int pageIndex, bool onlyCorrect)
        {
            return await _service.Get(id, testId, marks, pageSize, pageIndex, onlyCorrect);

        }

        /// <summary>
        /// Get All TestResults
        /// </summary>
        /// <param name="searchQuery"></param>
        /// <param name="testId"></param>
        /// <param name="groupId"></param>
        /// <param name="collegeId"></param>
        /// <param name="year"></param>
        /// <param name="currentPageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<JsonResult> GetResults(string? searchQuery, int? testId, int? groupId, int? collegeId, int? year, string? sortField, string? sortOrder, int? currentPageIndex = 0, int? pageSize = 10)
        {
            return await _service.GetResults(searchQuery, testId, groupId, collegeId, year, currentPageIndex, pageSize, sortField, sortOrder);
        }

        /// <summary>
        /// Get All TestResults Statistics
        /// </summary>
        /// <param name="searchQuery"></param>
        /// <param name="testId"></param>
        /// <param name="groupId"></param>
        /// <param name="collegeId"></param>
        /// <param name="year"></param>
        /// <param name="currentPageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<JsonResult> GetResultStatistics(string? searchQuery, int? testId, int? groupId, int? collegeId, int? year, string? sortField, string? sortOrder, int? currentPageIndex = 0)
        {
            return await _service.GetResultStatistics(searchQuery, testId, groupId, collegeId, year, currentPageIndex, sortField, sortOrder);
        }

        /// <summary>
        /// This methods adds the given minutes to the remaining time of user's test
        /// </summary>
        /// <param name="userTests"></param>
        /// <param name="timeToBeAdded"></param>
        /// <returns></returns>
        [HttpPut("[action]/{timeToBeAdded}")]
        public async Task<JsonResult> UpdateTestRemainingTime(List<UserTestVM> userTestIds, int timeToBeAdded)
        {
            if (ModelState.IsValid)
            {
                return await _service.UpdateTestRemainingTime(userTestIds, timeToBeAdded);
            }
            return new JsonResult(new ApiResponse<string>() { Message = ResponseMessages.BadRequest, Result = false, StatusCode = ResponseStatusCode.BadRequest });
        }

        /// <summary>
        /// Get Test Results Data For Export
        /// </summary>
        /// <param name="searchQuery"></param>
        /// <param name="testId"></param>
        /// <param name="groupId"></param>
        /// <param name="collegeId"></param>
        /// <param name="year"></param>
        /// <param name="currentPageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<JsonResult> GetResultExportData(string? searchQuery, int? testId, int? groupId, int? collegeId, int? year, string? sortField, string? sortOrder, int? currentPageIndex = 0, int? pageSize = 10)
        {
            return await _service.GetResultExportData(searchQuery, testId, groupId, collegeId, year, currentPageIndex, pageSize, sortField, sortOrder);
        }

        /// <summary>
        /// This method allows admin to approve resume test
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="testId"></param>
        /// <param name="isApprove"></param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public async Task<JsonResult> ApproveResumeTest(TestApproveVM testApproveVM)
        {
            return await _service.ApproveResumeTest(testApproveVM);
        }

        /// <summary>
        /// This methods returns data related to test
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="testId"></param>
        /// <returns></returns>
        [HttpGet("[action]/{userId:int}/{testId:int}")]
        public async Task<JsonResult> GetApproveTestData(int userId, int testId)
        {
            return await _service.GetApproveTestData(userId, testId);
        }

        [HttpGet("[action]/{testId:int}")]
        public async Task<JsonResult> GetGroupOfTest(int testId)
        {
            return await _service.GetGroupOfTest(testId);
        }
        #endregion
    }
}
