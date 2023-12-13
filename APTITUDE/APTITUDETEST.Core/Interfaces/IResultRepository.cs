using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Core.Interfaces
{
    public interface IResultRepository
    {
        public Task<JsonResult> Get(int id, int marks, int pageSize, int pageIndex);
        public Task<JsonResult> GetResults(string? searchQuery, int? TestId, int? GroupId, int? CollegeId, int? Year, int? currentPageIndex, int? pageSize, string? sortField, string? sortOrder);
        public Task<JsonResult> GetResultStatistics(string? searchQuery, int? TestId, int? GroupId, int? CollegeId, int? Year, int? currentPageIndex, string? sortField, string? sortOrder);
    }
}
