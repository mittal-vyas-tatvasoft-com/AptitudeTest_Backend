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
        public async Task<JsonResult> GetTests(string? searchQuery, int? GroupId, int? Status, DateOnly? Date, string? sortField, string? sortOrder, int? currentPageIndex = 0, int? pageSize = 10)
        {
            return await _service.GetTests(searchQuery, GroupId, Status, Date, currentPageIndex, pageSize, sortField, sortOrder);
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
        public async Task<JsonResult> DeleteTopicWiseTestQuestions(int testId, int topicId)
        {
            return await _service.DeleteTopicWiseTestQuestions(testId, topicId);
        }

        /// <summary>
        /// Delete All Test Questions
        /// </summary>
        /// <param name="testId"></param>
        /// <returns></returns>
        [HttpDelete("[action]/{testId:int}")]
        public async Task<JsonResult> DeleteAllTestQuestions(int testId)
        {
            return await _service.DeleteAllTestQuestions(testId);
        }

        /// <summary>
        /// Delete Test 
        /// </summary>
        /// <param name="testId"></param>
        /// <returns></returns>
        [HttpDelete("[action]/{testId:int}")]
        public async Task<JsonResult> DeleteTest(int testId)
        {
            return await _service.DeleteTest(testId);
        }

        /// <summary>
        /// GetAllTestCandidates
        /// </summary>
        /// <param name="searchQuery"></param>
        /// <param name="GroupId"></param>
        /// <param name="CollegeId"></param>
        /// <param name="SortField"></param>
        /// <param name="SortOrder"></param>
        /// <param name="currentPageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("[action]/{GroupId:int}/{currentPageIndex:int}/{pageSize:int}")]
        public async Task<JsonResult> GetAllTestCandidates(string? searchQuery, int GroupId, int? CollegeId, string? SortField, string? SortOrder, int? currentPageIndex = 0, int? pageSize = 10)
        {
            return await _service.GetAllTestCandidates(searchQuery, GroupId, CollegeId, SortField, SortOrder, currentPageIndex, pageSize);
        }

        /// <summary>
        /// Update Test 
        /// </summary>
        /// <param name="testVM"></param>
        /// <returns></returns>
        [HttpDelete("[action]")]
        public async Task<JsonResult> UpdateTest(CreateTestVM testVM)
        {
            return await _service.UpdateTest(testVM);
        }

        /// <summary>
        /// This method gives count of questions in each topic and its marks
        /// </summary>
        /// <param name="testId"></param>
        /// <returns></returns>
        [HttpGet("[action]/{testId:int}")]
        public async Task<JsonResult> GetQuestinsMarksCount(int testId)
        {
            return await _service.GetQuestinsMarksCount(testId);
        }

        /// <summary>
        /// This method gives count of questions of each topic
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<JsonResult> GetTopicWiseQuestionsCount()
        {
            return await _service.GetTopicWiseQuestionsCount();
        }
        #endregion
    }
}
