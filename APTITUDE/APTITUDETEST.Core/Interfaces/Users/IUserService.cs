using APTITUDETEST.Core.Entities.Users;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Core.Interfaces.Users
{
    public interface IUserService
    {
        Task<JsonResult> GetUserById(int id);
        Task<JsonResult> GetAllUsers(string? searchQuery, int? currentPageIndex, int? pageSize);

    }
}
