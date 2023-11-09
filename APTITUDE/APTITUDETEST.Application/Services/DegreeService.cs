using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Application.Services
{
    public class DegreeService : IDegreeService
    {
        #region Properties
        private readonly IDegreeRepository _repository;
        #endregion

        #region Constructor
        public DegreeService(IDegreeRepository repository)
        {
            _repository = repository;
        }
        #endregion

        #region Methods

        public async Task<JsonResult> GetDegrees(string? sortField, string? sortOrder)
        {
            return await _repository.GetDegrees(sortField,sortOrder);
        }
        public async Task<JsonResult> Get(int id)
        {
            return await _repository.Get(id);
        }

        public async Task<JsonResult> Create(DegreeVM degree)
        {
            return await _repository.Create(degree);
        }

        public async Task<JsonResult> Update(DegreeVM degree)
        {
            return await _repository.Update(degree);
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
