using AptitudeTest.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Core.Interfaces
{
    public interface ICollegeService
    {
        #region College
        public Task<JsonResult> GetColleges(int? currentPageIndex, int? pageSize, string? sortField, string? sortOrder);
        public Task<JsonResult> GetActiveColleges();
        public Task<JsonResult> Get(int id);
        public Task<JsonResult> Create(CollegeVM collegeToUpsert);
        public Task<JsonResult> Update(CollegeVM collegeToUpsert);
        public Task<JsonResult> UpdateStatus(StatusVM status);
        public Task<JsonResult> CheckUncheckAll(bool check);

        public Task<JsonResult> Delete(int id);
        #endregion
    }
}
