using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
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

        /// <summary>
        /// Create Test
        /// </summary>
        /// <param name="testVM"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<JsonResult> CreateTest(CreateTestVM testVM)
        {
            return await _service.CreateTest(testVM);
        }

        /// <summary>
        /// This method Updates Test Group
        /// </summary>
        /// <param name="updateTest"></param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public async Task<JsonResult> UpdateTestGroup(UpdateTestGroupVM updateTest)
        {
            return await _service.UpdateTestGroup(updateTest);
        }

        /// <summary>
        /// Add Test Questions
        /// </summary>
        /// <param name="testQuestionVM"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<JsonResult> AddTestQuestions(TestQuestionsVM testQuestionVM)
        {
            return await _service.AddTestQuestions(testQuestionVM);
        }

        /// <summary>
        /// Update Test Questions
        /// </summary>
        /// <param name="testQuestionVM"></param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public async Task<JsonResult> UpdateTestQuestions(TestQuestionsVM testQuestionVM)
        {
            return await _service.UpdateTestQuestions(testQuestionVM);
        }

        /// <summary>
        /// Delete Test Questions
        /// </summary>
        /// <param name="testId"></param>
        /// <param name="topicId"></param>
        /// <returns></returns>
        [HttpDelete("[action]/{testId:int}/{topicId:int}")]
        public async Task<JsonResult> DeleteTestQuestions(int testId, int topicId)
        {
            return await _service.DeleteTestQuestions(testId, topicId);
        }
        #endregion
    }
}
