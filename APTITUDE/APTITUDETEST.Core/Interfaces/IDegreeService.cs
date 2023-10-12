using AptitudeTest.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Core.Interfaces
{
    public interface IDegreeService
    {
        public Task<JsonResult> GetDegrees(string? searchQuery, int? filter, int? currentPageIndex, int? pageSize);

        public Task<JsonResult> Create(DegreeVM degree);
        public Task<JsonResult> Update(DegreeVM degree);

        public Task<JsonResult> CheckUncheckAll(bool check);

        public Task<JsonResult> Delete(int id);
    }
}
