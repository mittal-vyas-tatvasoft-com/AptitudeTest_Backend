using AptitudeTest.Core.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Core.Interfaces
{
    public interface IUserService
    {
        Task<JsonResult> GetUserById(int id);
        Task<JsonResult> GetAllUsers(string? searchQuery, int? CollegeId, int? GroupId, bool? Status, int? currentPageIndex, int? pageSize);
        Task<JsonResult> Create(CreateUserVM user);
        Task<JsonResult> Update(UserVM user);
        Task<JsonResult> ActiveInActiveUsers(UserStatusVM userStatusVM);
        Task<JsonResult> DeleteUsers(List<int> userIds);
        Task<JsonResult> ImportUsers(IFormFile file);
    }
}
