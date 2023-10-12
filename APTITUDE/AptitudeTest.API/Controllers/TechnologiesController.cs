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
    public class TechnologiesController : ControllerBase
    {
        #region Properties
        private readonly ITechnologyService _service;
        #endregion

        #region Constructor
        public TechnologiesController(ITechnologyService service)
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
        /// <returns>filtered list of technologies</returns>
        [HttpGet]

        public async Task<JsonResult> GetTechnologies(string? searchQuery, int? filter, int? currentPageIndex, int? pageSize)
        {
            return await _service.GetTechnologies(searchQuery, filter, currentPageIndex, pageSize);
        }

        /// <summary>
        /// This method Create technology
        /// </summary>
        /// <param name="technology"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<JsonResult> Create(TechnologyVM technology)
        {
            if (ModelState.IsValid)
            {
                return await _service.Create(technology);

            }

            return new JsonResult(new ApiResponse<string>() { Message = ResponseMessages.BadRequest, Result = false, StatusCode = ResponseStatusCode.BadRequest });
        }

        /// <summary>
        /// This method  Updates technology
        /// </summary>
        /// <param name="technology"></param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public async Task<JsonResult> Update(TechnologyVM technology)
        {
            if (ModelState.IsValid)
            {
                return await _service.Update(technology);

            }

            return new JsonResult(new ApiResponse<string>() { Message = ResponseMessages.BadRequest, Result = false, StatusCode = ResponseStatusCode.BadRequest });
        }

        /// <summary>
        /// This method Checks Or unchecks All technology
        /// </summary>
        /// <param name="check"></param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public async Task<JsonResult> CheckUncheckAll(bool check)
        {
            return await _service.CheckUncheckAll(check);
        }

        /// <summary>
        /// This method soft deletes technology
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
