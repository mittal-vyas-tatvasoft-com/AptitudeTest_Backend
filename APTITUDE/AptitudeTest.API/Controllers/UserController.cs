using AptitudeTest.Core.Interfaces.Users;
using APTITUDETEST.Core.Entities.Users;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        #region Properties
        private readonly IUserService _userService;
        #endregion

        #region Constructor
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// This method fetches single user data using user's Id
        /// </summary>
        /// <param name="id">user will be fetched according to this 'id'</param>
        /// <returns> user </returns> 

        [HttpGet("[action]")]
        public async Task<JsonResult> GetAllUsers(string? searchQuery, int? currentPageIndex = 1, int? pageSize = 10)
        {
            return await _userService.GetAllUsers(searchQuery, currentPageIndex, pageSize);
        }

        [HttpGet("[action]")]
        public async Task<JsonResult> GetUserById(int id)
        {
            return await _userService.GetUserById(id);
        }
        #endregion
    }
}
