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
    public class GroupsController : ControllerBase
    {
        #region Properties
        private readonly IGroupService _service;
        #endregion

        #region Constructor
        public GroupsController(IGroupService service)
        {
            _service = service;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get Groups For DropDown
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetGroupsForDropDown")]
        public async Task<JsonResult> GetActiveGroups()
        {
            return await _service.GetActiveGroups();
        }

        /// <summary>
        /// This method return all the groups along with the colleges under it with searching as well
        /// </summary>
        /// <param name="searchGroup">key for searching in group names</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> GetAllGroups(string? searchGroup, int? searchedCollegeId)
        {
            return await _service.GetGroups(searchGroup, searchedCollegeId);
        }

        /// <summary>
        /// This method creates a new group
        /// </summary>
        /// <param name="group">group to be added</param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<JsonResult> Create(GroupsQueryVM group)
        {
            if (ModelState.IsValid)
            {
                return await _service.Create(group);
            }
            return new JsonResult(new ApiResponse<string>() { Message = ResponseMessages.BadRequest, Result = false, StatusCode = ResponseStatusCode.BadRequest });
        }

        /// <summary>
        /// This method renames the group
        /// </summary>
        /// <param name="group">group to be renamed</param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public async Task<JsonResult> Update(GroupsQueryVM group)
        {
            if (ModelState.IsValid)
            {
                return await _service.Update(group);
            }
            return new JsonResult(new ApiResponse<string>() { Message = ResponseMessages.BadRequest, Result = false, StatusCode = ResponseStatusCode.BadRequest });
        }

        /// <summary>
        /// This method deletes the group with given id
        /// </summary>
        /// <param name="id">Id of group to be deleted</param>
        /// <returns></returns>
        [HttpDelete("[action]")]
        public async Task<JsonResult> Delete(int id)
        {
            return await _service.Delete(id);
        }
        #endregion
    }
}
