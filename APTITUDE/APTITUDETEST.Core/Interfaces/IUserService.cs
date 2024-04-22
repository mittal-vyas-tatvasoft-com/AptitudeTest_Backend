using AptitudeTest.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Core.Interfaces
{
    public interface IUserService
    {
        Task<JsonResult> GetUserById(int id);

        Task<JsonResult> GetAllState();
        Task<JsonResult> GetAllUsers(string? searchQuery, int? CollegeId, int? GroupId, bool? Status, int? Year, string? sortField, string? sortOrder, int? currentPageIndex, int? pageSize);
        Task<JsonResult> Create(CreateUserVM user);
        Task<JsonResult> Update(UserVM user);
        Task<JsonResult> ActiveInActiveUsers(UserStatusVM userStatusVM);
        Task<JsonResult> DeleteUsers(List<int> userIds);
        Task<JsonResult> ImportUsers(ImportUserVM importUsers);
        Task<JsonResult> RegisterUser(UserVM registerUserVM);
        Task<JsonResult> ChangeUserPasswordByAdmin(string? Email, string? Password);
        Task<JsonResult> GetUsersExportData(string? searchQuery, int? CollegeId, int? GroupId, bool? Status, int? Year, string? sortField, string? sortOrder, int? currentPageIndex, int? pageSize);

    }
}
