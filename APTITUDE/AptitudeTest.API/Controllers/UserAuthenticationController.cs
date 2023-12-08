using AptitudeTest.Core.Interfaces.UserAuthentication;
using AptitudeTest.Core.ViewModels;
using AptitudeTest.Data.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAuthenticationController : ControllerBase
    {
        #region Properties
        private readonly IUserAuthenticationService _userAuthenticationService;
        #endregion

        #region Constructor
        public UserAuthenticationController(IUserAuthenticationService userAuthenticationService)
        {
            _userAuthenticationService = userAuthenticationService;
        }
        #endregion

        #region UserAuthentication
        /// <summary>
        /// Login User
        /// </summary>
        /// <param name="loginVm"></param>
        /// <returns>Tokens for authentication</returns>
        [HttpPost("Login")]
        public async Task<JsonResult> Login(LoginVm loginVm)
        {
            return await _userAuthenticationService.Login(loginVm);
        }

        #endregion

        #region Logout
        /// <summary>
        /// Logout request
        /// </summary>
        /// <param name="email"></param>
        /// <returns>no data return onl mail sent</returns>
        [HttpPost("[action]")]

        public async Task<JsonResult> Logout(string email)
        {
            return await _userAuthenticationService.Logout(email);
        }

        #endregion

        #region ForgetPassword
        /// <summary>
        /// Forget password request
        /// </summary>
        /// <param name="email"></param>
        /// <returns>no data return onl mail sent</returns>
        [HttpPost("ForgetPassword")]

        public async Task<JsonResult> ForgetPassword(string email)
        {
            return await _userAuthenticationService.ForgetPassword(email);
        }

        #endregion

        #region ResetPassword
        /// <summary>
        /// method for reset the password of user
        /// </summary>
        /// <param name="resetPassword">takes encrypted email and new password</param>
        /// <returns>change the password</returns>
        [HttpPost("ResetPassword")]
        public async Task<JsonResult> ResetPassword(ResetPasswordVm resetPassword)
        {
            return await _userAuthenticationService.ResetPassword(resetPassword);
        }

        #endregion

        #region ChangePassword
        /// <summary>
        /// method for change the password of user
        /// </summary>
        /// <param name="changePassword">takes encrypted email and new password</param>
        /// <returns>change the password</returns>
        [Authorize]
        [HttpPost("ChangePassword")]
        public async Task<JsonResult> ChangePassword(ChangePasswordVM changePassword)
        {
            if (ModelState.IsValid)
            {
                return await _userAuthenticationService.ChangePassword(changePassword);
            }
            else
            {
                return new JsonResult(new ApiResponse<string> { Message = ResponseMessages.BadRequest, StatusCode = ResponseStatusCode.BadRequest, Result = false });
            }
        }

        #endregion

        #region RefreshToken
        /// <summary>
        /// generate refresh token
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns>return new access and refresh token</returns>
        [HttpPost("RefreshToken")]

        public async Task<JsonResult> RefreshToken(TokenVm tokens)
        {
            if (tokens != null)
            {
                return await _userAuthenticationService.RefreshToken(tokens);
            }
            return new JsonResult(new ApiResponseVm<string> { Data = null, Message = ResponseMessages.InternalError, StatusCode = ResponseStatusCode.InternalServerError, Result = false });

        }
        #endregion

    }
}
