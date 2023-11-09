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
    public class DegreesController : ControllerBase
    {
        #region Properties
        private readonly IDegreeService _service;
        #endregion

        #region Constructor
        public DegreesController(IDegreeService service)
        {
            _service = service;
        }
        #endregion

        #region Methods

        /// <summary>
        /// gives degree by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>degree</returns>
        [HttpGet("[action]")]
        public async Task<JsonResult> Get(int id)
        {
            return await _service.Get(id);
        }

        /// <summary>
        /// This gives List of degrees with searching,filtering and pagination
        /// </summary>
        /// <param name="searchQuery">Word that we want to search</param>
        /// <param name="filter">Filter list on status 1 for Active  2 for Inactive </param>
        /// <param name="currentPageIndex">Page index which is page number-1</param>
        /// <param name="pageSize">Length of records in 1 page</param>
        /// <returns>filtered list of degrees</returns>
        [HttpGet]

        public async Task<JsonResult> GetDegrees()
        {
            return await _service.GetDegrees();
        }

        /// <summary>
        /// Get Active Degrees 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetActiveDegrees")]
        public async Task<JsonResult> GetActiveDegrees()
        {
            return await _service.GetActiveDegrees();
        }

        /// <summary>
        /// This method Creates Degree
        /// </summary>
        /// <param name="degree"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<JsonResult> Create(DegreeVM degree)
        {
            if (ModelState.IsValid)
            {
                return await _service.Create(degree);
            }
            return new JsonResult(new ApiResponse<string>() { Message = ResponseMessages.BadRequest, Result = false, StatusCode = ResponseStatusCode.BadRequest });

        }

        /// <summary>
        /// This method  Updates Degree
        /// </summary>
        /// <param name="degree"></param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public async Task<JsonResult> Update(DegreeVM degree)
        {
            if (ModelState.IsValid)
            {
                return await _service.Update(degree);
            }
            return new JsonResult(new ApiResponse<string>() { Message = ResponseMessages.BadRequest, Result = false, StatusCode = ResponseStatusCode.BadRequest });

        }

        /// <summary>
        /// This method  Updates Degree Status
        /// </summary>
        /// <param name="degree"></param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public async Task<JsonResult> UpdateStatus(StatusVM status)
        {
            if (ModelState.IsValid)
            {
                return await _service.UpdateStatus(status);
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
