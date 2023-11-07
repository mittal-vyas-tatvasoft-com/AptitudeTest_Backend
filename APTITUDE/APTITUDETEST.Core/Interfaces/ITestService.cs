using AptitudeTest.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Core.Interfaces
{
    public interface ITestService
    {
        public Task<JsonResult> GetTests(string? searchQuery, int? GroupId, int? Status, DateTime? Date, int? currentPageIndex, int? pageSize);
        public Task<JsonResult> CreateTest(CreateTestVM testVM);
        public Task<JsonResult> UpdateTestGroup(UpdateTestGroupVM updateTest);
    }
}
