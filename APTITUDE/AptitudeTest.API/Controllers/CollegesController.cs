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
    public class CollegesController : ControllerBase
    {
        #region Properties
        private readonly ICollegeService _service;
        #endregion

        #region Constructor
        public CollegesController(ICollegeService service)
        {
            _service = service;
        }
        #endregion

        #region Methods

        /// <summary>
        /// gives college by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>college</returns>
        [HttpGet("[action]")]
        public async Task<JsonResult> Get(int id)
        {
            return await _service.Get(id);
        }

        /// <summary>
        /// This gives List of colleges with searching,filtering and pagination
        /// </summary>
        /// <param name="searchQuery">Word that we want to search</param>
        /// <param name="filter">Filter colleges on status 1 for Active  2 for Inactive </param>
        /// <param name="currentPageIndex">Page index which is page number-1</param>
        /// <param name="pageSize">Length of records in 1 page</param>
        /// <param name="searchQuery">word that we want to search</param>
        /// <returns>filtered list of colleges</returns>
        [HttpGet]
        public async Task<JsonResult> GetColleges(int? currentPageIndex, int? pageSize)
        {
            CollegeQueryVM collegeQuery = new CollegeQueryVM()
            {
                CurrentPageIndex = currentPageIndex,
                PageSize = pageSize
            };
            return await _service.GetColleges(collegeQuery);
        }

        /// <summary>
        /// Get Colleges For DropDown
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetActiveColleges")]
        public async Task<JsonResult> GetActiveColleges()
        {
            return await _service.GetActiveColleges();
        }

        /// <summary>
        /// This method Creates College
        /// </summary>
        /// <param name="college"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<JsonResult> Create(CollegeVM college)
        {
            if (ModelState.IsValid)
            {
                return await _service.Create(college);
            }

            return new JsonResult(new ApiResponse<string>() { Message = ResponseMessages.BadRequest, Result = false, StatusCode = ResponseStatusCode.BadRequest });
        }

        /// <summary>
        /// This method Updates College
        /// </summary>
        /// <param name="college"></param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public async Task<JsonResult> Update(CollegeVM college)
        {
            if (ModelState.IsValid)
            {
                return await _service.Update(college);
            }

            return new JsonResult(new ApiResponse<string>() { Message = ResponseMessages.BadRequest, Result = false, StatusCode = ResponseStatusCode.BadRequest });
        }

        /// <summary>
        /// This method  Updates College Status
        /// </summary>
        /// <param name="status"></param>
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
        /// This method Checks Or unchecks All Colleges
        /// </summary>
        /// <param name="check"></param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public async Task<JsonResult> CheckUncheckAll(bool check)
        {
            return await _service.CheckUncheckAll(check);
        }

        /// <summary>
        /// This method soft deletes college
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
