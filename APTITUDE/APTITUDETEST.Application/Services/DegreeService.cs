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

        public async Task<JsonResult> GetDegrees(string? searchQuery, int? filter, int? currentPageIndex, int? pageSize)
        {
            return await _repository.GetDegrees(searchQuery, filter, currentPageIndex, pageSize);
        }

        public async Task<JsonResult> Upsert(DegreeVM degree)
        {
            return await _repository.Upsert(degree);
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
