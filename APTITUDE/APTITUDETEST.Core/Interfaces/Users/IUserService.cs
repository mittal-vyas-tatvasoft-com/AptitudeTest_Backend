using AptitudeTest.Core.ViewModels.User;
using APTITUDETEST.Core.Entities.Users;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Core.Interfaces.Users
{
    public interface IUserService
    {
        #region Methods
        Task<User> GetUserById(int id);
        Task<JsonResult> Create(UserVm userVm);
        Task<JsonResult> Update(UserVm userVm);
        Task<JsonResult> Delete(int id);
        #endregion

    }
}
