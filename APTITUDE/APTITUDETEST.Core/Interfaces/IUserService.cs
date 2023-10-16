using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Core.Interfaces
{
    public interface IUserService
    {
        Task<JsonResult> GetUserById(int id);
        Task<JsonResult> GetAllUsers(string? searchQuery, int? currentPageIndex, int? pageSize);

    }
}
