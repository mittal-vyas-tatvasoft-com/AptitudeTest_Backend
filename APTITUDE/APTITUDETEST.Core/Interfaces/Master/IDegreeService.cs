using AptitudeTest.Core.ViewModels.Master;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Core.Interfaces.Master
{
    public interface IDegreeService
    {
        public Task<JsonResult> GetDegrees(string? searchQuery, int? filter, int? currentPageIndex, int? pageSize);

        public Task<JsonResult> Upsert(DegreeVM degree);

        public Task<JsonResult> CheckUncheckAll(bool check);

        public Task<JsonResult> Delete(int id);
    }
}
