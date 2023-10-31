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

        public async Task<JsonResult> GetCollegesForDropDown()
        {
            return await _repository.GetCollegesForDropDown();
        }

        public async Task<JsonResult> Get(int id)
        {
            return await _repository.Get(id);

        }
        public async Task<JsonResult> Create(CollegeVM college)
        {
            return await _repository.Create(college);
        }

        public async Task<JsonResult> Update(CollegeVM college)
        {
            return await _repository.Update(college);
        }

        public async Task<JsonResult> UpdateStatus(StatusVM status)
        {
            return await _repository.UpdateStatus(status);
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
