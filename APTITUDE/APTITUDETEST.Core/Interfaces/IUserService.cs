using AptitudeTest.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Core.Interfaces
{
    public interface IUserService
    {
        Task<JsonResult> GetUserById(int id);
        Task<JsonResult> GetAllUsers(string? searchQuery, int? currentPageIndex, int? pageSize);
        Task<JsonResult> Create(UserVM user);
        Task<JsonResult> Update(UserVM user);
        Task<JsonResult> InActiveUsers(List<int> userIds);
        Task<JsonResult> DeleteUsers(List<int> userIds);

    }
}
