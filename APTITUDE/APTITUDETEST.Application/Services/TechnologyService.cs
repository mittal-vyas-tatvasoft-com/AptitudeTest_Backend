using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Application.Services
{
    public class TechnologyService : ITechnologyService
    {
        #region Properties
        private readonly ITechnologyRepository _repository;
        #endregion

        #region Constructor
        public TechnologyService(ITechnologyRepository repository)
        {
            _repository = repository;
        }
        #endregion

        #region Methods

        public async Task<JsonResult> GetTechnologies(string? searchQuery, int? filter, int? currentPageIndex, int? pageSize)
        {
            return await _repository.GetTechnologies(searchQuery, filter, currentPageIndex, pageSize);
        }

        public async Task<JsonResult> Create(TechnologyVM technology)
        {
            return await _repository.Create(technology);
        }

        public async Task<JsonResult> Update(TechnologyVM technology)
        {
            return await _repository.Update(technology);
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
