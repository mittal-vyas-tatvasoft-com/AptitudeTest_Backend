using AptitudeTest.Core.Interfaces.UserAuthentication;
using AptitudeTest.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Application.Services.UserAuthentication
{
    public class UserAuthenticationService : IUserAuthenticationService
    {

        #region Properties
        private readonly IUserAuthenticationRepository _userAuthenticationRepository;
        #endregion

        #region Constructor
        public UserAuthenticationService(IUserAuthenticationRepository userAuthenticationRepository)
        {
            _userAuthenticationRepository = userAuthenticationRepository;
        }
        #endregion

        #region Methods

        #region Login
        public async Task<JsonResult> Login(LoginVm loginVm)
        {
            return await _userAuthenticationRepository.Login(loginVm);
        }
        #endregion

        #region ForgetPassword
        public async Task<JsonResult> ForgetPassword(string email)
        {
            return await _userAuthenticationRepository.ForgetPassword(email);
        }
        #endregion

        #region RefreshToken

        public async Task<JsonResult> RefreshToken(TokenVm tokens)
        {
            return await _userAuthenticationRepository.RefreshToken(tokens);
        }
        #endregion

        #region test

        public async Task<JsonResult> GetAllUsers()
        {

            return await _userAuthenticationRepository.GetAllUsers();

        }

        #endregion

        #endregion

    }
}
