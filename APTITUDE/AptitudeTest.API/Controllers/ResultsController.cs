using AptitudeTest.Core.Interfaces;
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
        [HttpGet("[action]/{id:int}/{testId:int}/{marks:int}/{pageSize:int}/{pageIndex:int}")]
        public async Task<JsonResult> Get(int id,int testId,int marks, int pageSize, int pageIndex)
        {
            return await _service.Get(id, testId,marks, pageSize, pageIndex);

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
        [HttpGet("{currentPageIndex:int}/{pageSize:int}")]
        public async Task<JsonResult> GetResults(string? searchQuery, int? testId, int? groupId, int? collegeId, int? year, int? currentPageIndex, int? pageSize, string? sortField, string? sortOrder)
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
        [HttpGet("{currentPageIndex}")]
        public async Task<JsonResult> GetResultStatistics(string? searchQuery, int? testId, int? groupId, int? collegeId, int? year, int? currentPageIndex, string? sortField, string? sortOrder)
        {
            return await _service.GetResultStatistics(searchQuery, testId, groupId, collegeId, year, currentPageIndex, sortField, sortOrder);
        }
        #endregion
    }
}
