using AptitudeTest.Core.Interfaces.Master;
using AptitudeTest.Core.ViewModels.Common;
using AptitudeTest.Core.ViewModels.Master;
using AptitudeTest.Data.Common;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollegeController : ControllerBase
    {
        #region Properties
        private readonly ICollegeService _service;
        #endregion

        #region Constructor
        public CollegeController(ICollegeService service)
        {
            _service = service;
        }
        #endregion

        #region Methods

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
        public async Task<JsonResult> GetColleges(string? searchQuery, int? filter, int? currentPageIndex, int? pageSize)
        {
            CollegeQueryVM collegeQuery = new CollegeQueryVM()
            {
                CurrentPageIndex = currentPageIndex,
                Filter = filter,
                SearchQuery = searchQuery,
                PageSize = pageSize
            };
            return await _service.GetColleges(collegeQuery);
        }

        /// <summary>
        /// This method Inserts And Updates College
        /// </summary>
        /// <param name="college"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<JsonResult> Upsert(CollegeVM college)
        {
            if (ModelState.IsValid)
            {
                return await _service.Upsert(college);
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
