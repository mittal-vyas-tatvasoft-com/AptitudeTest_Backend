﻿using AptitudeTest.Core.ViewModels;
using APTITUDETEST.Core.Entities.Users;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Core.Interfaces.UserAuthentication
{
    public interface IUserAuthenticationRepository 
    { 
        Task<JsonResult> Login(LoginVm loginVm);
        Task<JsonResult> ForgetPassword(string email);
        Task<JsonResult> RefreshToken(TokenVm tokens);
        Task<JsonResult> GetAllUsers();
        Task<JsonResult> ResetPassword(ResetPasswordVm resetPassword);

    }
}
