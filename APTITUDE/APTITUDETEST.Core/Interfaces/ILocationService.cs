using AptitudeTest.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Core.Interfaces
{
    public interface ILocationService
    {
        #region Location

        public Task<JsonResult> GetLocations(string? searchQuery, int? filter, List<int>? collegeList, int? currentPageIndex, int? pageSize);

        public Task<JsonResult> Upsert(LocationVM location);

        public Task<JsonResult> CheckUncheckAll(bool check);

        public Task<JsonResult> Delete(int id);
        #endregion
    }
}
