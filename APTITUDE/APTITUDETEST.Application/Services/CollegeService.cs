using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Application.Services
{
    public class CollegeService : ICollegeService
    {
        #region Properties
        private readonly ICollegeRepository _repository;
        #endregion

        #region Constructor
        public CollegeService(ICollegeRepository repository)
        {
            _repository = repository;
        }
        #endregion

        #region Methods

        public async Task<JsonResult> GetColleges(CollegeQueryVM collegeQuery)
        {
            return await _repository.GetColleges(collegeQuery);
        }

        public async Task<JsonResult> Upsert(CollegeVM college)
        {
            return await _repository.Upsert(college);
        }

        public async Task<JsonResult> CheckUncheckAll(bool check)
        {
            return await _repository.CheckUncheckAll(check);
        }

        public async Task<JsonResult> Delete(int id)
        {
            return await _repository.Delete(id);
        }
        #endregion
    }
}
