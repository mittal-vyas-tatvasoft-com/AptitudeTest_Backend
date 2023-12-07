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

        public async Task<JsonResult> GetColleges(int? currentPageIndex, int? pageSize, string? sortField, string? sortOrder)
        {
            return await _repository.GetColleges(currentPageIndex, pageSize, sortField, sortOrder);
        }

        public async Task<JsonResult> GetActiveColleges()
        {
            return await _repository.GetActiveColleges();
        }

        public async Task<JsonResult> Get(int id)
        {
            return await _repository.Get(id);

        }
        public async Task<JsonResult> Create(CollegeVM collegeToUpsert)
        {
            return await _repository.Create(collegeToUpsert);
        }

        public async Task<JsonResult> Update(CollegeVM collegeToUpsert)
        {
            return await _repository.Update(collegeToUpsert);
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
