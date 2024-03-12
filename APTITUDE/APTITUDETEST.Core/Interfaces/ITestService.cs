using AptitudeTest.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Core.Interfaces
{
    public interface ITestService
    {
        public Task<JsonResult> GetTests(string? searchQuery, int? GroupId, int? Status, DateOnly? Date, int? currentPageIndex, int? pageSize, string? sortField, string? sortOrder);
        public Task<JsonResult> CreateTest(CreateTestVM testVM);
        public Task<JsonResult> UpdateTestGroup(UpdateTestGroupVM updateTest);
        public Task<JsonResult> UpdateTestStatus(TestStatusVM statusVM);
        public Task<JsonResult> AddTestQuestions(TestQuestionsVM addTestQuestion);
        public Task<JsonResult> UpdateTestQuestions(TestQuestionsVM updateTestQuestion);
        public Task<JsonResult> DeleteTopicWiseTestQuestions(int testId, int topicId);
        public Task<JsonResult> DeleteAllTestQuestions(int testId);
        public Task<JsonResult> DeleteTest(int testId);
        public Task<JsonResult> DeleteMultipleTests(List<int> testIds);
        public Task<JsonResult> GetAllTestCandidates(string? searchQuery, int GroupId, int? CollegeId, string? SortField, string? SortOrder, int? currentPageIndex = 0, int? pageSize = 10);
        public Task<JsonResult> UpdateTest(UpdateTestVM testVM);
        public Task<JsonResult> GetQuestionsMarksCount(int testId);
        public Task<JsonResult> GetTopicWiseQuestionsCount();
        public Task<JsonResult> GetTestById(int testId);
        public Task<JsonResult> CheckTestName(string testName);
        public Task<JsonResult> GetTestsForDropdown();
        public Task<JsonResult> UpdateBasicPoints(int testId);
        public Task<JsonResult> GenerateTestForCandidates(int testId);
    }
}
