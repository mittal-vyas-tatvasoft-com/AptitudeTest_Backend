using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Application.Services
{
    public class AdminAuthenticationService : IAdminAuthenticationService
    {
        #region Properties
        private readonly IAdminAuthenticationRepository _adminAuthenticationRepository;
        #endregion

        #region Constructor
        public AdminAuthenticationService(IAdminAuthenticationRepository adminAuthenticationRepository)
        {
            _adminAuthenticationRepository = adminAuthenticationRepository;
        }
        #endregion

        #region Methods

        #region Login
        public async Task<JsonResult> Login(LoginVm loginVm)
        {
            return await _adminAuthenticationRepository.Login(loginVm);
        }
        #endregion

        #region ForgetPassword
        public async Task<JsonResult> ForgetPassword(string email)
        {
            return await _adminAuthenticationRepository.ForgetPassword(email);
        }
        #endregion

        #region ResetPassword
        public async Task<JsonResult> ResetPassword(ResetPasswordVm resetPassword)
        {
            return await _adminAuthenticationRepository.ResetPassword(resetPassword);
        }
        #endregion

        #region ChangePassword
        public async Task<JsonResult> ChangePassword(ChangePasswordVM changePassword)
        {
            return await _adminAuthenticationRepository.ChangePassword(changePassword);
        }
        #endregion

        #region RefreshToken

        public async Task<JsonResult> RefreshToken(TokenVm tokens)
        {
            return await _adminAuthenticationRepository.RefreshToken(tokens);
        }
        #endregion



        #endregion
    }
}
