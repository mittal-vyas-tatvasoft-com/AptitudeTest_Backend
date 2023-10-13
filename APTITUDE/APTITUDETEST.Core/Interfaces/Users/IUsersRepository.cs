using AptitudeTest.Core.ViewModels.User;
using APTITUDETEST.Core.Entities.Users;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Core.Interfaces.Users
{

    public interface IUsersRepository : IRepositoryBase<User>
    {
        #region Methods
        Task<JsonResult> Create(UserVm userVm);
        Task<JsonResult> Update(UserVm userVm);
        Task<JsonResult> Delete(int id);
        #endregion
    }
}
