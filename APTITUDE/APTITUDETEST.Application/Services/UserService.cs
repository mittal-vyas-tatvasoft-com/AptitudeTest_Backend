using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Application.Services
{
    public class UserService : IUserService
    {
        #region Properties
        private readonly IUsersRepository _userRepository;
        #endregion

        #region Constructor
        public UserService(IUsersRepository userrepo)
        {
            _userRepository = userrepo;
        }
        #endregion

        #region Methods
        public async Task<JsonResult> GetAllUsers(string? searchQuery, int? CollegeId, int? GroupId, bool? Status, int? Year, int? currentPageIndex, int? pageSize)
        {
            return await _userRepository.GetAllUsers(searchQuery, CollegeId, GroupId, Status, Year, currentPageIndex, pageSize);
        }

        public async Task<JsonResult> GetUserById(int id)
        {
            return await _userRepository.GetUserById(id);
        }

        public async Task<JsonResult> GetAllState()
        {
            return await _userRepository.GetAllState();
        }

        public async Task<JsonResult> Create(CreateUserVM user)
        {
            return await _userRepository.Create(user);
        }

        public async Task<JsonResult> Update(UserVM user)
        {
            return await _userRepository.Update(user);
        }

        public async Task<JsonResult> ActiveInActiveUsers(UserStatusVM userStatusVM)
        {
            return await _userRepository.ActiveInActiveUsers(userStatusVM);
        }

        public async Task<JsonResult> DeleteUsers(List<int> userIds)
        {
            return await _userRepository.DeleteUsers(userIds);
        }

        public async Task<JsonResult> ImportUsers(ImportUserVM importUsers)
        {
            return await _userRepository.ImportUsers(importUsers);
        }
        #endregion
    }
}
