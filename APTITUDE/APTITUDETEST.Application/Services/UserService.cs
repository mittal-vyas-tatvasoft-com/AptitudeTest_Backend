using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Application.Services
{
    public class UserService : IUserService
    {
        #region Properties
        private readonly IUsersRepository _userRepository;
        #endregion

        #region Constructor
        public UserService(IUsersRepository userrepo)
        {
            _userRepository = userrepo;
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

        public async Task<JsonResult> Create(UserVM user)
        {
            return await _userRepository.Create(user);
        }

        public async Task<JsonResult> Update(UserVM user)
        {
            return await _userRepository.Update(user);
        }
        #endregion
    }
}
