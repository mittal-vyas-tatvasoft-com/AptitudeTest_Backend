using AptitudeTest.Core.Entities.Master;
using AptitudeTest.Core.ViewModels.Master;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Core.Interfaces.Master
{
    public interface ICollegeRepository : IRepositoryBase<MasterCollege>
    {
        #region College

        public Task<JsonResult> GetColleges(CollegeQueryVM collegeQuery);

        public Task<JsonResult> Upsert(CollegeVM college);

        public Task<JsonResult> CheckUncheckAll(bool check);

        public Task<JsonResult> Delete(int id);
        #endregion
    }
}
