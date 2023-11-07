using AptitudeTest.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Core.Interfaces
{
    public interface IAdminAuthenticationService
    {
        Task<JsonResult> Login(LoginVm loginVm);
        Task<JsonResult> ForgetPassword(string email);
        Task<JsonResult> RefreshToken(TokenVm tokens);
        Task<JsonResult> ResetPassword(ResetPasswordVm resetPassword);
        Task<JsonResult> ChangePassword(ChangePasswordVM changePassword);
    }
}
