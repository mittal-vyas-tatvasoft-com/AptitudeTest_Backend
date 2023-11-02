using AptitudeTest.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TestsController : ControllerBase
    {
        #region Properties
        private readonly ITestService _service;
        #endregion

        #region Constructor
        public TestsController(ITestService service)
        {
            _service = service;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get All Tests
        /// </summary>
        /// <param name="searchQuery"></param>
        /// <param name="GroupId"></param>
        /// <param name="Status"></param>
        /// <param name="Date"></param>
        /// <param name="currentPageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>

        [HttpGet("{currentPageIndex:int}/{pageSize:int}")]
        public async Task<JsonResult> GetTests(string? searchQuery, int? GroupId, int? Status, DateTime? Date, int? currentPageIndex = 0, int? pageSize = 10)
        {
            return await _service.GetTests(searchQuery, GroupId, Status, Date, currentPageIndex, pageSize);
        }

        #endregion
    }
}
