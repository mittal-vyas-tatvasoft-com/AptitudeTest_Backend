using AptitudeTest.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Core.Interfaces
{
    public interface IResultRepository
    {
        public Task<JsonResult> Get(int id, int testId, int marks, int pageSize, int pageIndex, bool onlyCorrect);
        public Task<JsonResult> GetResults(string? searchQuery, int? TestId, int? GroupId, int? CollegeId, int? Year, int? currentPageIndex, int? pageSize, string? sortField, string? sortOrder);
        public Task<JsonResult> GetResultStatistics(string? searchQuery, int? TestId, int? GroupId, int? CollegeId, int? Year, int? currentPageIndex, string? sortField, string? sortOrder);
        public Task<JsonResult> GetResultExportData(string? searchQuery, int? TestId, int? GroupId, int? CollegeId, int? Year, int? currentPageIndex, int? pageSize, string? sortField, string? sortOrder);
        public Task<JsonResult> ApproveResumeTest(TestApproveVM testApproveVM);
        public Task<JsonResult> GetApproveTestData(int userId, int testId);
        public Task<JsonResult> GetGroupOfTest(int testId);
        public Task<JsonResult> UpdateTestRemainingTime(List<UserTestVM> userTests, int timeToBeAdded);
    }
}
