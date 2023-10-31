using AptitudeTest.Core.Interfaces;
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
        #endregion
    }
}
