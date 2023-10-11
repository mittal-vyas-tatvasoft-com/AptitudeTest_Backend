using AptitudeTest.Core.Interfaces.Users;
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
        public async Task<JsonResult> GetAllUsers(string? searchQuery, int? currentPageIndex, int? pageSize)
        {
            return await _userRepository.GetAllUsers(searchQuery, currentPageIndex, pageSize);
        }

        public async Task<JsonResult> GetUserById(int id)
        {
            return await _userRepository.GetUserById(id);
        }
        #endregion
    }
}
