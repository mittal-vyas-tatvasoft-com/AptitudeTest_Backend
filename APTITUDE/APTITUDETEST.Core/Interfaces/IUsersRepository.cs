using APTITUDETEST.Core.Entities.Users;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Core.Interfaces
{

    public interface IUsersRepository : IRepositoryBase<User>
    {
        public Task<JsonResult> GetAllUsers(string? searchQuery, int? currentPageIndex, int? pageSize);

        public Task<JsonResult> GetUserById(int id);
    }
}
