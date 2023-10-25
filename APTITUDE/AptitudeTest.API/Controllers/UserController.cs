using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using AptitudeTest.Data.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.API.Controllers
{
    [Authorize]
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

        #region GetAllUsers
        /// <summary>
        /// This method fetches all the users with pagination data according to search string
        /// </summary>
        /// <param name="searchQuery"></param>
        /// <param name="currentPageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("{currentPageIndex:int}/{pageSize:int}")]
        public async Task<JsonResult> GetAllUsers(string? searchQuery, int? currentPageIndex = 0, int? pageSize = 10)
        {
            return await _userService.GetAllUsers(searchQuery, currentPageIndex, pageSize);
        }
        #endregion


        #region GetUserById
        /// <summary>
        /// This method fetches single user data using user's Id
        /// </summary>
        /// <param name="id">user will be fetched according to this 'id'</param>
        /// <returns> user </returns> 
        [HttpGet("[action]/{id:int}")]
        public async Task<JsonResult> Get(int id)
        {
            return await _userService.GetUserById(id);
        }
        #endregion

        #region Create
        [HttpPost("[action]")]
        public async Task<JsonResult> Create(UserVM user)
        {
            if (ModelState.IsValid)
            {
                return await _userService.Create(user);
            }

            return new JsonResult(new ApiResponse<string>() { Message = ResponseMessages.BadRequest, Result = false, StatusCode = ResponseStatusCode.BadRequest });
        }
        #endregion
        #endregion
    }
}

