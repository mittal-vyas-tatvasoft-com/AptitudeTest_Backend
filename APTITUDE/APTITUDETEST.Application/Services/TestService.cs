using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Application.Services
{
    public class TestService : ITestService
    {
        #region Properties
        private readonly ITestRepository _repository;
        #endregion

        #region Constructor
        public TestService(ITestRepository repository)
        {
            _repository = repository;
        }
        #endregion

        #region Methods

        public async Task<JsonResult> GetTests(string? searchQuery, int? GroupId, int? Status, DateTime? Date, int? currentPageIndex, int? pageSize, string? sortField, string? sortOrder)
        {
            return await _repository.GetTests(searchQuery, GroupId, Status, Date, currentPageIndex, pageSize, sortField, sortOrder);
        }
        public Task<JsonResult> CreateTest(CreateTestVM testVM)
        {
            return _repository.CreateTest(testVM);
        }
        public Task<JsonResult> UpdateTestGroup(UpdateTestGroupVM updateTest)
        {
            return _repository.UpdateTestGroup(updateTest);
        }
        public Task<JsonResult> AddTestQuestions(TestQuestionsVM addTestQuestion)
        {
            return _repository.AddTestQuestions(addTestQuestion);
        }
        public Task<JsonResult> UpdateTestQuestions(TestQuestionsVM updateTestQuestion)
        {
            return _repository.UpdateTestQuestions(updateTestQuestion);
        }
        public Task<JsonResult> DeleteTopicWiseTestQuestions(int testId, int topicId)
        {
            return _repository.DeleteTopicWiseTestQuestions(testId, topicId);
        }
        public Task<JsonResult> DeleteAllTestQuestions(int testId)
        {
            return _repository.DeleteAllTestQuestions(testId);
        }
        public Task<JsonResult> DeleteTest(int testId)
        {
            return _repository.DeleteTest(testId);
        }
        public Task<JsonResult> UpdateTest(CreateTestVM testVM)
        {
            return _repository.UpdateTest(testVM);
        }
        public Task<JsonResult> GetQuestinsMarksCount(int testId)
        {
            return _repository.GetQuestinsMarksCount(testId);
        }
        #endregion
    }
}
