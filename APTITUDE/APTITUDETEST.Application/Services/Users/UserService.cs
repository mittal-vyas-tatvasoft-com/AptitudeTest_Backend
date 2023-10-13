using AptitudeTest.Core.Interfaces.Users;
using AptitudeTest.Core.ViewModels.User;
using APTITUDETEST.Core.Entities.Users;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Application.Services.Users
{
    public class UserService : IUserService
    {
        #region Properties
        private readonly IUsersRepository _userRepository;
        #endregion

        #region Constructor
        public UserService(IUsersRepository userepo)
        {
            _userRepository = userepo;
        }
        #endregion

        #region Methods
        public async Task<User> GetUserById(int id)
        {
            return await _userRepository.GetById(id);

        }

        public async Task<JsonResult> Create(UserVm userVm)
        {
            return await _userRepository.Create(userVm);
        }
        
        public async Task<JsonResult> Update(UserVm userVm)
        {
            return await _userRepository.Update(userVm);
        }
        #endregion
    }
}
