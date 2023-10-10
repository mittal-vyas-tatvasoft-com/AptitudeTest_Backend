using AptitudeTest.Core.Interfaces.UserAuthentication;
using AptitudeTest.Core.ViewModels;
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
            return new JsonResult(new ApiResponseVm<string> { Data = null, Message = ResponseMessages.InternalServerError, StatusCode = ResponseStatusCodes.InternalServerError, Result = false });

        }
        #endregion



        #region test
        [HttpGet("Test")]
        [Authorize]
        public async Task<JsonResult> GetAllUsers()
        {

            return await _userAuthenticationService.GetAllUsers();

        }

        #endregion
    }
}
