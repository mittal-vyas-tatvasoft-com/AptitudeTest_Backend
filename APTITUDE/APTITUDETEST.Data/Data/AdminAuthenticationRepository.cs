using AptitudeTest.Common.Helpers;
using AptitudeTest.Core.Entities.Admin;
using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using AptitudeTest.Data.Common;
using APTITUDETEST.Common.Data;
using APTITUDETEST.Core.Entities.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Text;

namespace AptitudeTest.Data.Data
{
    public class AdminAuthenticationRepository : IAdminAuthenticationRepository
    {
        #region Properies
        private readonly AppDbContext _context;
        static IConfiguration _appSettingConfiguration;
        public static Dictionary<string, TokenVm> RefreshTokens = new Dictionary<string, TokenVm>();
        #endregion

        #region Constructor
        public AdminAuthenticationRepository(AppDbContext context, IConfiguration appSettingConfiguration)
        {
            _context = context;
            _appSettingConfiguration = appSettingConfiguration;
        }
        #endregion

        #region Methods

        #region Login
        public async Task<JsonResult> Login(LoginVm loginVm)
        {
            try
            {
                if (string.IsNullOrEmpty(loginVm.Password) || string.IsNullOrEmpty(loginVm.Email))
                {
                    return new JsonResult(new ApiResponse<string> { Message = ResponseMessages.BadRequest, StatusCode = ResponseStatusCode.BadRequest, Result = false });
                }

                var jwtHelper = new JWTHelper(_appSettingConfiguration);
                Admin? admin = _context.Admins.Where(u => u.Email == loginVm.Email && u.Password == loginVm.Password)?.FirstOrDefault();
                if (admin == null)
                {
                    return new JsonResult(new ApiResponse<User> { Message = ResponseMessages.InvalidCredentials, StatusCode = ResponseStatusCode.Unauthorized, Result = false });
                }
                string newAccessToken = jwtHelper.GenerateJwtToken(null, admin);
                string newRefreshToken = jwtHelper.CreateRefreshToken(admin.Email, RefreshTokens);

                if (string.IsNullOrEmpty(newAccessToken) || string.IsNullOrEmpty(newRefreshToken))
                {
                    return new JsonResult(new ApiResponse<string> { Message = ResponseMessages.BadRequest, StatusCode = ResponseStatusCode.BadRequest, Result = false });
                }
                TokenVm tokenPayload = new TokenVm()
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken,
                    RefreshTokenExpiryTime = DateTime.Now.AddHours(double.Parse(_appSettingConfiguration["JWT:RefreshTokenValidityInHours"])),
                };
                if (RefreshTokens.ContainsKey(admin.Email))
                {
                    RefreshTokens[admin.Email] = tokenPayload;
                }
                else
                {
                    RefreshTokens.Add(admin.Email, tokenPayload);
                }
                return new JsonResult(new ApiResponse<TokenVm> { Data = tokenPayload, Message = ResponseMessages.LoginSuccess, StatusCode = ResponseStatusCode.OK, Result = true });
            }
            catch
            {
                return new JsonResult(new ApiResponse<string> { Message = ResponseMessages.InternalError, StatusCode = ResponseStatusCode.InternalServerError, Result = false });
            }
        }
        #endregion

        #region ForgetPassword
        public async Task<JsonResult> ForgetPassword(string email)
        {
            try
            {
                Admin? admin = _context.Admins.Where(u => u.Email == email).FirstOrDefault();

                if (admin == null)
                {
                    return new JsonResult(new ApiResponse<string> { Message = string.Format(ResponseMessages.NotFound, ModuleNames.User), StatusCode = ResponseStatusCode.NotFound, Result = false });
                }
                if (admin.IsSuperAdmin == true)
                {
                    return new JsonResult(new ApiResponse<string> { Message = string.Format(ResponseMessages.SuperAdminRequestFail, ModuleNames.Operation), StatusCode = ResponseStatusCode.BadRequest, Result = false });
                }
                var sent = SendMailForResetPassword(admin.FirstName, admin.Email);
                if (sent)
                {
                    return new JsonResult(new ApiResponse<string> { Message = ResponseMessages.MailSentForForgetPassword, StatusCode = ResponseStatusCode.OK, Result = true });
                }
                else
                {
                    return new JsonResult(new ApiResponse<string> { Message = ResponseMessages.InternalError, StatusCode = ResponseStatusCode.InternalServerError, Result = false });
                }
            }
            catch
            {
                return new JsonResult(new ApiResponse<string> { Data = null, Message = ResponseMessages.InternalError, StatusCode = ResponseStatusCode.InternalServerError, Result = false });

            }
        }

        #endregion

        #region ResetPassword
        public async Task<JsonResult> ResetPassword(ResetPasswordVm resetPassword)
        {
            try
            {
                if (string.IsNullOrEmpty(resetPassword.EncryptedEmail) || string.IsNullOrEmpty(resetPassword.NewPassword))
                {
                    return new JsonResult(new ApiResponse<string> { Message = ResponseMessages.BadRequest, StatusCode = ResponseStatusCode.BadRequest, Result = false });
                }
                byte[] byteForEmail = Convert.FromBase64String(resetPassword.EncryptedEmail);
                string decryptedEmail = Encoding.ASCII.GetString(byteForEmail);

                Admin? admin = _context.Admins.Where(u => u.Email == decryptedEmail).FirstOrDefault();
                if (admin == null)
                {
                    return new JsonResult(new ApiResponse<string> { Message = ResponseMessages.BadRequest, StatusCode = ResponseStatusCode.BadRequest, Result = false });
                }
                if (admin.IsSuperAdmin == true)
                {
                    return new JsonResult(new ApiResponse<string> { Message = string.Format(ResponseMessages.SuperAdminRequestFail, ModuleNames.Operation), StatusCode = ResponseStatusCode.BadRequest, Result = false });

                }
                admin.Password = resetPassword.NewPassword;
                admin.UpdatedBy = admin.Id;
                admin.UpdatedDate = DateTime.Now.ToUniversalTime();
                _context.Update(admin);
                _context.SaveChanges();
                return new JsonResult(new ApiResponse<string> { Message = ResponseMessages.PasswordUpdatedSuccess, StatusCode = ResponseStatusCode.OK, Result = true });

            }
            catch
            {
                return new JsonResult(new ApiResponse<string> { Message = ResponseMessages.InternalError, StatusCode = ResponseStatusCode.InternalServerError, Result = false });
            }
        }
        #endregion

        #region ChangePassword
        public async Task<JsonResult> ChangePassword(ChangePasswordVM changePassword)
        {
            try
            {
                Admin? admin = await Task.FromResult(_context.Admins.Where(x => x.Email.Equals(changePassword.Email) && x.Password.Equals(changePassword.CurrentPassword)).FirstOrDefault());
                if (admin == null)
                {
                    return new JsonResult(new ApiResponse<string> { Message = string.Format(ResponseMessages.NotFound, ModuleNames.User), StatusCode = ResponseStatusCode.NotFound, Result = false });
                }
                if (admin.IsSuperAdmin == true)
                {
                    return new JsonResult(new ApiResponse<string> { Message = string.Format(ResponseMessages.SuperAdminRequestFail, ModuleNames.Operation), StatusCode = ResponseStatusCode.BadRequest, Result = false });
                }
                if (admin.Password.Equals(changePassword.NewPassword))
                {
                    return new JsonResult(new ApiResponse<string> { Message = ResponseMessages.currentAndNewSame, StatusCode = ResponseStatusCode.Forbidden, Result = false });
                }

                if (!changePassword.NewPassword.Equals(changePassword.confirmPassword))
                {
                    return new JsonResult(new ApiResponse<string> { Message = ResponseMessages.passwordNotMatched, StatusCode = ResponseStatusCode.Forbidden, Result = false });
                }
                admin.Password = changePassword.NewPassword;
                admin.UpdatedDate = DateTime.Now.ToUniversalTime();
                _context.Update(admin);
                _context.SaveChanges();
                return new JsonResult(new ApiResponse<string> { Message = string.Format(ResponseMessages.UpdateSuccess, ModuleNames.Password), StatusCode = ResponseStatusCode.OK, Result = true });


            }
            catch
            {
                return new JsonResult(new ApiResponse<string> { Message = ResponseMessages.InternalError, StatusCode = ResponseStatusCode.InternalServerError, Result = false });
            }
        }
        #endregion

        #region RefreshToken
        public async Task<JsonResult> RefreshToken(TokenVm tokens)
        {
            try
            {

                if (tokens is null)
                {
                    return new JsonResult(new ApiResponse<string> { Message = ResponseMessages.BadRequest, StatusCode = ResponseStatusCode.BadRequest, Result = false });
                }
                else
                {
                    var jwtHelper = new JWTHelper(_appSettingConfiguration);
                    string accessToken = tokens.AccessToken;
                    string refreshToken = tokens.RefreshToken;
                    var principal = jwtHelper.GetPrincipleFromExpiredToken(accessToken);
                    var allClaims = principal.Claims.ToList();
                    var email = allClaims[4].Value;
                    var tokenssss = RefreshTokens.GetValueOrDefault(email);
                    var admin = await _context.Admins.FirstOrDefaultAsync(U => U.Email == email);
                    if (admin == null || RefreshTokens[email].RefreshToken != refreshToken || RefreshTokens[email].RefreshTokenExpiryTime <= DateTime.Now)
                    {
                        return new JsonResult(new ApiResponse<string> { Message = ResponseMessages.BadRequest, StatusCode = ResponseStatusCode.BadRequest, Result = false });
                    }

                    var newAccessToken = jwtHelper.GenerateJwtToken(null, admin);
                    var newRefreshToken = jwtHelper.CreateRefreshToken(email, RefreshTokens);
                    if (string.IsNullOrEmpty(newAccessToken) && string.IsNullOrEmpty(newRefreshToken))
                    {
                        return new JsonResult(new ApiResponse<string> { Message = ResponseMessages.BadRequest, StatusCode = ResponseStatusCode.BadRequest, Result = false });
                    }

                    RefreshTokens[email].RefreshToken = newRefreshToken;
                    TokenVm tokenPayload = new TokenVm()
                    {
                        AccessToken = newAccessToken,
                        RefreshToken = newRefreshToken,
                    };
                    return new JsonResult(new ApiResponse<TokenVm> { Data = tokenPayload, Message = ResponseMessages.SessionRefresh, StatusCode = ResponseStatusCode.OK, Result = true });

                }
            }
            catch
            {
                return new JsonResult(new ApiResponse<string> { Data = null, Message = ResponseMessages.InternalError, StatusCode = ResponseStatusCode.InternalServerError, Result = false });
            }
        }
        #endregion

        #region SendEmail
        private bool SendMailForResetPassword(string firstName, string email)
        {
            try
            {
                byte[] byteForEmail = Encoding.ASCII.GetBytes(email);
                string encryptedEmail = Convert.ToBase64String(byteForEmail);
                UriBuilder builder = new();
                builder.Host = Convert.ToString(_appSettingConfiguration["EmailGeneration:FrontEndUrl"]);
                builder.Port = Convert.ToInt16(_appSettingConfiguration["EmailGeneration:FrontEndPort"]);
                builder.Path = "/ResetPassword";
                builder.Query = "&email=" + encryptedEmail;
                var resetLink = builder.ToString();

                var toAddress = new MailAddress(email);
                var subject = "Password reset request";
                var body = $"<h3>Hello {firstName}</h3>,<br />we received password reset request from your side,<br /><br />Please click on the following link to reset your password <br /><br /><a href='{resetLink}'><h3>Click here</h3></a>";

                var emailHelper = new EmailHelper(_appSettingConfiguration);
                var isEmailSent = emailHelper.SendEmail(email, subject, body);
                return isEmailSent;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #endregion

    }
}
