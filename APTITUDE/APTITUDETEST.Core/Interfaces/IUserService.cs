using AptitudeTest.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Core.Interfaces
{
    public interface IUserService
    {
        Task<JsonResult> GetUserById(int id);
        Task<JsonResult> GetAllUsers(string? searchQuery, int? currentPageIndex, int? pageSize);
        Task<JsonResult> Create(CreateUserVM user);
        Task<JsonResult> Update(UserVM user);
        Task<JsonResult> ActiveInActiveUsers(UserStatusVM userStatusVM);
        Task<JsonResult> DeleteUsers(List<int> userIds);

    }
}
