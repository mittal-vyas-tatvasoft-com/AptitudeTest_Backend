using AptitudeTest.Core.Interfaces.Master;
using AptitudeTest.Core.ViewModels.Master;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Application.Services.Master
{

    public class LocationService : ILocationService
    {
        #region Properties
        ILocationRepository _repository;
        #endregion

        #region Constructor
        public LocationService(ILocationRepository repository)
        {
            _repository = repository;
        }
        #endregion

        #region Methods
        public async Task<JsonResult> GetLocations(string? searchQuery, int? filter, List<int>? collegeList, int? currentPageIndex, int? pageSize)
        {
            return await _repository.GetLocations(searchQuery, filter, collegeList, currentPageIndex, pageSize);
        }

        public async Task<JsonResult> Upsert(LocationVM location)
        {
            return await _repository.Upsert(location);
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
