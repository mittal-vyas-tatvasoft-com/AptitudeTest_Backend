using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Application.Services
{
    public class ResultService : IResultService
    {
        #region Properties
        private readonly IResultRepository _repository;
        #endregion

        #region Constructor
        public ResultService(IResultRepository repository)
        {
            _repository = repository;
        }
        #endregion

        #region Methods
        public async Task<JsonResult> Get(int id, int testId, int marks, int pageSize, int pageIndex, bool onlyCorrect)
        {
            return await _repository.Get(id, testId, marks, pageSize, pageIndex, onlyCorrect);
        }

        public Task<JsonResult> GetResults(string? searchQuery, int? TestId, int? GroupId, int? CollegeId, int? Year, int? currentPageIndex, int? pageSize, string? sortField, string? sortOrder)
        {
            return _repository.GetResults(searchQuery, TestId, GroupId, CollegeId, Year, currentPageIndex, pageSize, sortField, sortOrder);
        }

        public Task<JsonResult> GetResultStatistics(string? searchQuery, int? TestId, int? GroupId, int? CollegeId, int? Year, int? currentPageIndex, string? sortField, string? sortOrder)
        {
            return _repository.GetResultStatistics(searchQuery, TestId, GroupId, CollegeId, Year, currentPageIndex, sortField, sortOrder);
        }
        public Task<JsonResult> GetResultExportData(string? searchQuery, int? TestId, int? GroupId, int? CollegeId, int? Year, int? currentPageIndex, int? pageSize, string? sortField, string? sortOrder)
        {
            return _repository.GetResultExportData(searchQuery, TestId, GroupId, CollegeId, Year, currentPageIndex, pageSize, sortField, sortOrder);
        }
        public Task<JsonResult> ApproveResumeTest(TestApproveVM testApproveVM)
        {
            return _repository.ApproveResumeTest(testApproveVM);
        }
        public Task<JsonResult> GetApproveTestData(int userId, int testId)
        {
            return _repository.GetApproveTestData(userId, testId);
        }
        public async Task<JsonResult> GetGroupOfTest(int testId)
        {
            return await _repository.GetGroupOfTest(testId);
        }

        public async Task<JsonResult> UpdateTestRemainingTime(List<UserTestVM> userTests, int timeToBeAdded)
        {
            return await _repository.UpdateTestRemainingTime(userTests, timeToBeAdded);
        }

        public async Task<JsonResult> ReverseLockedTests(ReverseLockedTestVM reverseLockedTestVM)
        {
            return await _repository.ReverseLockedTests(reverseLockedTestVM);
        }
        #endregion
    }
}
