using AptitudeTest.Core.Interfaces.Master;
using AptitudeTest.Core.ViewModels.Master;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        #region Properties
        private readonly ILocationService _service;
        #endregion

        #region Constructor
        public LocationController(ILocationService service)
        {
            _service = service;
        }
        #endregion

        #region Methods

        /// <summary>
        /// This gives List of colleges with searching,filtering and pagination
        /// </summary>
        /// <param name="searchQuery">Word that we want to search</param>
        /// <param name="filter">Filter list on status 1 for Active  2 for Inactive </param>
        /// <param name="collegeList">List of college ids whose locations we want </param>
        /// <param name="currentPageIndex">Page index which is page number-1</param>
        /// <param name="pageSize">Length of records in 1 page</param>
        /// <returns>filtered list of locations</returns>
        [HttpPost]

        public async Task<JsonResult> GetLocations(string? searchQuery, int? filter, List<int>? collegeList, int? currentPageIndex, int? pageSize)
        {
            return await _service.GetLocations(searchQuery, filter, collegeList, currentPageIndex, pageSize);
        }

        /// <summary>
        /// This method Inserts And Updates Location
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<JsonResult> Upsert(LocationVM location)
        {
            return await _service.Upsert(location);
        }

        /// <summary>
        /// This method Checks Or unchecks All Locations
        /// </summary>
        /// <param name="check"></param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public async Task<JsonResult> CheckUncheckAll(bool check)
        {
            return await _service.CheckUncheckAll(check);
        }

        /// <summary>
        /// This method soft deletes location
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("[action]")]
        public async Task<JsonResult> Delete(int id)
        {
            return await _service.Delete(id);
        }
        #endregion
    }

}
