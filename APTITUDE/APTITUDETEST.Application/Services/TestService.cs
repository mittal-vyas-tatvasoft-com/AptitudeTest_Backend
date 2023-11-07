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

        public async Task<JsonResult> GetTests(string? searchQuery, int? GroupId, int? Status, DateTime? Date, int? currentPageIndex, int? pageSize)
        {
            return await _repository.GetTests(searchQuery, GroupId, Status, Date, currentPageIndex, pageSize);
        }
        public Task<JsonResult> CreateTest(CreateTestVM testVM)
        {
            return _repository.CreateTest(testVM);
        }
        public Task<JsonResult> UpdateTestGroup(UpdateTestGroupVM updateTest)
        {
            return _repository.UpdateTestGroup(updateTest);
        }
        #endregion
    }
}
