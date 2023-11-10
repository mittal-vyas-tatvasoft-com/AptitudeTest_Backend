using AptitudeTest.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Core.Interfaces
{

    public interface IUsersRepository
    {
        public Task<JsonResult> GetAllUsers(string? searchQuery, int? CollegeId, int? GroupId, bool? Status, int? Year, string? sortField, string? sortOrder, int? currentPageIndex, int? pageSize);
        public Task<JsonResult> GetUserById(int id);
        public Task<JsonResult> GetAllState();
        public Task<JsonResult> Create(CreateUserVM user);
        public Task<JsonResult> Update(UserVM user);
        public Task<JsonResult> ActiveInActiveUsers(UserStatusVM userStatusVM);
        public Task<JsonResult> DeleteUsers(List<int> userIds);
        public Task<JsonResult> ImportUsers(ImportUserVM importUsers);
    }
}
