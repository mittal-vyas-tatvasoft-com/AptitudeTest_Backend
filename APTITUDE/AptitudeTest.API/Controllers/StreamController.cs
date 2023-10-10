using AptitudeTest.Core.Interfaces.Master;
using AptitudeTest.Core.ViewModels.Master;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StreamController : ControllerBase
    {
        #region Properties
        private readonly IStreamService _service;
        #endregion

        #region Constructor
        public StreamController(IStreamService service)
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
        /// <param name="degreelist">List of degree ids whose stream we want </param>
        /// <param name="currentPageIndex">Page index which is page number-1</param>
        /// <param name="pageSize">Length of records in 1 page</param>
        /// <returns>filtered list of streams</returns>
        [HttpPost]

        public async Task<JsonResult> Getstreams(string? searchQuery, int? filter, List<int>? degreelist, int? currentPageIndex, int? pageSize)
        {
            return await _service.Getstreams(searchQuery, filter, degreelist, currentPageIndex, pageSize);
        }

        /// <summary>
        /// This method Inserts And Updates stream
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<JsonResult> Upsert(StreamVM stream)
        {
            return await _service.Upsert(stream);
        }

        /// <summary>
        /// This method Checks Or unchecks all streams
        /// </summary>
        /// <param name="check"></param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public async Task<JsonResult> CheckUncheckAll(bool check)
        {
            return await _service.CheckUncheckAll(check);
        }

        /// <summary>
        /// This method soft deletes stream
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
