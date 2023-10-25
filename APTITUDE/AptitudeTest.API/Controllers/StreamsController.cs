using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;

using AptitudeTest.Data.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StreamsController : ControllerBase
    {
        #region Properties
        private readonly IStreamService _service;
        #endregion

        #region Constructor
        public StreamsController(IStreamService service)
        {
            _service = service;
        }
        #endregion

        #region Methods

        /// <summary>
        /// This gives List of streams with searching,filtering and pagination
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
        /// This method Creates stream
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<JsonResult> Create(StreamVM stream)
        {
            if (ModelState.IsValid)
            {
                return await _service.Create(stream);

            }

            return new JsonResult(new ApiResponse<string>() { Message = ResponseMessages.BadRequest, Result = false, StatusCode = ResponseStatusCode.BadRequest });
        }

        /// <summary>
        /// This method Updates stream
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public async Task<JsonResult> Update(StreamVM stream)
        {
            if (ModelState.IsValid)
            {
                return await _service.Update(stream);

            }

            return new JsonResult(new ApiResponse<string>() { Message = ResponseMessages.BadRequest, Result = false, StatusCode = ResponseStatusCode.BadRequest });
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
