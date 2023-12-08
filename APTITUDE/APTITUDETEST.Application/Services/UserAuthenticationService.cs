using AptitudeTest.Core.Interfaces.UserAuthentication;
using AptitudeTest.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Application.Services
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

        #region Logout
        public async Task<JsonResult> Logout(string email)
        {
            return await _userAuthenticationRepository.Logout(email);
        }
        #endregion

        #region ForgetPassword
        public async Task<JsonResult> ForgetPassword(string email)
        {
            return await _userAuthenticationRepository.ForgetPassword(email);
        }
        #endregion

        #region ResetPassword
        public async Task<JsonResult> ResetPassword(ResetPasswordVm resetPassword)
        {
            return await _userAuthenticationRepository.ResetPassword(resetPassword);
        }
        #endregion

        #region ChangePassword
        public async Task<JsonResult> ChangePassword(ChangePasswordVM changePassword)
        {
            return await _userAuthenticationRepository.ChangePassword(changePassword);
        }
        #endregion

        #region RefreshToken

        public async Task<JsonResult> RefreshToken(TokenVm tokens)
        {
            return await _userAuthenticationRepository.RefreshToken(tokens);
        }
        #endregion


        #endregion

    }
}
