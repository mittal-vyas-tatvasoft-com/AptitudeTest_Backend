using AptitudeTest.Core.ViewModels.Master;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Core.Interfaces.Master
{
    public interface ILocationRepository
    {
        #region Location

        public Task<JsonResult> GetLocations(string? searchQuery, int? filter, List<int>? collegeList, int? currentPageIndex, int? pageSize);

        public Task<JsonResult> Upsert(LocationVM location);

        public Task<JsonResult> CheckUncheckAll(bool check);

        public Task<JsonResult> Delete(int id);
        #endregion
    }
}
