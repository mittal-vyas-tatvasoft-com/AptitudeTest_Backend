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
    public class DegreeController : ControllerBase
    {
        #region Properties
        private readonly IDegreeService _service;
        #endregion

        #region Constructor
        public DegreeController(IDegreeService service)
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
        /// <param name="currentPageIndex">Page index which is page number-1</param>
        /// <param name="pageSize">Length of records in 1 page</param>
        /// <returns>filtered list of degrees</returns>
        [HttpGet]

        public async Task<JsonResult> GetDegrees(string? searchQuery, int? filter, int? currentPageIndex, int? pageSize)
        {
            return await _service.GetDegrees(searchQuery, filter, currentPageIndex, pageSize);
        }

        /// <summary>
        /// This method Inserts And Updates Location
        /// </summary>
        /// <param name="degree"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<JsonResult> Upsert(DegreeVM degree)
        {
            if (ModelState.IsValid)
            {
                return await _service.Upsert(degree);
            }
            return new JsonResult(new ApiResponse<string>() { Message = ResponseMessages.BadRequest, Result = false, StatusCode = ResponseStatusCode.BadRequest });

        }

        /// <summary>
        /// This method Checks Or unchecks All Degrees
        /// </summary>
        /// <param name="check"></param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public async Task<JsonResult> CheckUncheckAll(bool check)
        {
            return await _service.CheckUncheckAll(check);
        }

        /// <summary>
        /// This method soft deletes Degree
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
