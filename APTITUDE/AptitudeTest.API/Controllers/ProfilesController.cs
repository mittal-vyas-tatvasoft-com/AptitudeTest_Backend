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
    public class ProfilesController : ControllerBase
    {
        #region Properties
        private readonly IProfileService _service;
        #endregion

        #region Constructor
        public ProfilesController(IProfileService service)
        {
            _service = service;
        }
        #endregion

        #region Methods

        /// <summary>
        /// This gives List of profiles with searching,filtering and pagination
        /// </summary>
        /// <param name="searchQuery">Word that we want to search</param>
        /// <param name="filter">Filter list on status 1 for Active  2 for Inactive </param>
        /// <param name="currentPageIndex">Page index which is page number-1</param>
        /// <param name="pageSize">Length of records in 1 page</param>
        /// <returns>filtered list of profiles</returns>
        [HttpGet]

        public async Task<JsonResult> GetProfiles(string? searchQuery, int? filter, int? currentPageIndex, int? pageSize)
        {
            return await _service.GetProfiles(searchQuery, filter, currentPageIndex, pageSize);
        }

        /// <summary>
        /// This method Create profile
        /// </summary>
        /// <param name="profile"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<JsonResult> Create(ProfileVM profile)
        {
            if (ModelState.IsValid)
            {
                return await _service.Create(profile);

            }

            return new JsonResult(new ApiResponse<string>() { Message = ResponseMessages.BadRequest, Result = false, StatusCode = ResponseStatusCode.BadRequest });
        }

        /// <summary>
        /// This method  Updates profile
        /// </summary>
        /// <param name="profile"></param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public async Task<JsonResult> Update(ProfileVM profile)
        {
            if (ModelState.IsValid)
            {
                return await _service.Update(profile);

            }

            return new JsonResult(new ApiResponse<string>() { Message = ResponseMessages.BadRequest, Result = false, StatusCode = ResponseStatusCode.BadRequest });
        }

        /// <summary>
        /// This method Checks Or unchecks All profile
        /// </summary>
        /// <param name="check"></param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public async Task<JsonResult> CheckUncheckAll(bool check)
        {
            return await _service.CheckUncheckAll(check);
        }

        /// <summary>
        /// This method soft deletes profile
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("[action]")]
        public async Task<JsonResult> Delete(int id)
        {
            return await _service.Delete(id);
        }

        /// <summary>
        /// this method get the profile data by it's id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("[action]/{id:int}")]

        public async Task<JsonResult> Get(int? id)
        {
            return await _service.GetProfileById(id);
        }

        /// <summary>
        /// this api update the status of single profile
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
        #endregion


    }
}
