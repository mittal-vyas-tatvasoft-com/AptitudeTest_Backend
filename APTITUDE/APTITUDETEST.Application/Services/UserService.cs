using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;

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
        public async Task<JsonResult> GetAllUsers(string? searchQuery, int? CollegeId, int? GroupId, bool? Status, int? Year, string? sortField, string? sortOrder, int? currentPageIndex, int? pageSize)
        {
            return await _userRepository.GetAllUsers(searchQuery, CollegeId, GroupId, Status, Year, sortField, sortOrder, currentPageIndex, pageSize);
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

        public async Task<JsonResult> RegisterUser(UserVM registerUserVM)
        {
            return await _userRepository.RegisterUser(registerUserVM);
        }

        public async Task<JsonResult> ChangeUserPasswordByAdmin(string? Email, string? Password)
        {
            return await _userRepository.ChangeUserPasswordByAdmin(Email, Password);
        }

        public async Task<JsonResult> GetUsersExportData(string? searchQuery, int? collegeId, int? groupId, bool? status, int? year, string? sortField, string? sortOrder, int? currentPageIndex, int? pageSize)
        {
            return await _userRepository.GetUsersExportData(searchQuery, collegeId, groupId, status, year, sortField, sortOrder, currentPageIndex, pageSize);
        }

        #endregion
    }
}
