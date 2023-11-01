﻿using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using AptitudeTest.Data.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.API.Controllers
{
    //[Authorize]
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
        public async Task<JsonResult> GetAllUsers(string? searchQuery, int? CollegeId, int? GroupId, bool? Status, int? Year, int? currentPageIndex = 0, int? pageSize = 10)
        {
            return await _userService.GetAllUsers(searchQuery, CollegeId, GroupId, Status, Year, currentPageIndex, pageSize);
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
        /// <summary>
        /// This method Creates User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<JsonResult> Create(CreateUserVM user)
        {
            if (ModelState.IsValid)
            {
                return await _userService.Create(user);
            }

            return new JsonResult(new ApiResponse<string>() { Message = ResponseMessages.BadRequest, Result = false, StatusCode = ResponseStatusCode.BadRequest });
        }
        #endregion

        #region Update
        /// <summary>
        /// This method Updates User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public async Task<JsonResult> Update(UserVM user)
        {
            if (ModelState.IsValid)
            {
                return await _userService.Update(user);
            }

            return new JsonResult(new ApiResponse<string>() { Message = ResponseMessages.BadRequest, Result = false, StatusCode = ResponseStatusCode.BadRequest });
        }
        #endregion

        #region ActiveInActive
        /// <summary>
        /// This method ActiveOrInActive Users
        /// </summary>
        /// <param name="check"></param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public async Task<JsonResult> ActiveInActiveUsers(UserStatusVM userStatusVM)
        {
            return await _userService.ActiveInActiveUsers(userStatusVM);
        }
        #endregion

        #region Delete
        /// <summary>
        /// This method Delete Users
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public async Task<JsonResult> DeleteUsers(List<int> userIds)
        {
            return await _userService.DeleteUsers(userIds);
        }
        #endregion

        #region ImportUsers
        /// <summary>
        /// This method Import Users
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>

        [HttpPost("[action]")]
        public async Task<JsonResult> ImportUsers(IFormFile file)
        {
            return await _userService.ImportUsers(file);
        }
        #endregion



        #endregion
    }
}

