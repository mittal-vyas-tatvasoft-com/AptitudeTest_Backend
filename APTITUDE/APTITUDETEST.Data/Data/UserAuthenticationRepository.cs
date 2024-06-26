﻿using AptitudeTest.Common.Helpers;
using AptitudeTest.Core.Entities.Candidate;
using AptitudeTest.Core.Entities.Test;
using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using AptitudeTest.Data.Common;
using APTITUDETEST.Common.Data;
using APTITUDETEST.Core.Entities.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AptitudeTest.Data.Data
{
    public class UserAuthenticationRepository : IUserAuthenticationRepository
    {
        #region Properies
        static AppDbContext _context;
        static IConfiguration? _appSettingConfiguration;
        private Dictionary<string, TokenVm> RefreshTokens = new Dictionary<string, TokenVm>();
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISessionIdHelperInMemoryService _sessionIdHelperInMemoryService;
        private readonly UserActiveTestHelper _userActiveTestHelper;
        private readonly ILoggerManager _logger;
        #endregion

        #region Constructor
        public UserAuthenticationRepository(AppDbContext context, IConfiguration appSettingConfiguration, IHttpContextAccessor httpContextAccessor
            , ISessionIdHelperInMemoryService sessionIdHelperInMemoryService, UserActiveTestHelper userActiveTestHelper, ILoggerManager logger)
        {
            _context = context;
            _appSettingConfiguration = appSettingConfiguration;
            _httpContextAccessor = httpContextAccessor;
            _sessionIdHelperInMemoryService = sessionIdHelperInMemoryService;
            _userActiveTestHelper = userActiveTestHelper;
            _logger = logger;
        }
        #endregion

        #region Methods

        #region Login
        public async Task<JsonResult> Login(LoginVm loginVm)
        {
            try
            {
                if (string.IsNullOrEmpty(loginVm.Email) || string.IsNullOrEmpty(loginVm.Password))
                {
                    return new JsonResult(new ApiResponse<string> { Message = ResponseMessages.BadRequest, StatusCode = ResponseStatusCode.BadRequest, Result = false });
                }
                var jwtHelper = new JwtHelper(_appSettingConfiguration);
                User? user = _context.Users.Where(u => u.Email.Trim().ToLower() == loginVm.Email.Trim().ToLower() && u.Password == loginVm.Password.Trim() && u.IsDeleted == true)?.FirstOrDefault();
                if (user != null)
                {
                    return new JsonResult(new ApiResponse<User> { Message = ResponseMessages.UserDoesNotExist, StatusCode = ResponseStatusCode.Unauthorized, Result = false });
                }
                user = _context.Users.Where(u => u.Email.Trim().ToLower() == loginVm.Email.Trim().ToLower() && u.Password == loginVm.Password.Trim() && u.IsDeleted == false)?.FirstOrDefault();
                if (user == null)
                {
                    return new JsonResult(new ApiResponse<User> { Message = ResponseMessages.InvalidCredentials, StatusCode = ResponseStatusCode.Unauthorized, Result = false });
                }
                if (user.Status == false)
                {
                    return new JsonResult(new ApiResponse<User> { Message = ResponseMessages.InActiveAccount, StatusCode = ResponseStatusCode.Unauthorized, Result = false });
                }
                string newAccessToken = jwtHelper.GenerateJwtToken(user, null);
                string newRefreshToken = jwtHelper.CreateRefreshToken(user.Email, RefreshTokens);

                //Test? test = _userActiveTestHelper.GetTestOfUser(user.Id);
                Test? test = _userActiveTestHelper.GetValidTestOfUser(user.Id);
                UserTest? userTest = new UserTest();
                TempUserTest tempUserTest = null;
                if (test != null)
                {
                    userTest = _context.UserTests.Where(x => x.TestId == test.Id && x.UserId == user.Id).FirstOrDefault();
                    tempUserTest = _context.TempUserTests.Where(x => x.TestId == test.Id && x.UserId == user.Id).FirstOrDefault();
                }

                if (string.IsNullOrEmpty(newAccessToken) && string.IsNullOrEmpty(newRefreshToken))
                {
                    return new JsonResult(new ApiResponse<string> { Message = ResponseMessages.BadRequest, StatusCode = ResponseStatusCode.BadRequest, Result = false });
                }

                string sessionId = GenerateSessionId();
                user.SessionId = sessionId;
                _context.Update(user);
                _context.SaveChanges();
                _sessionIdHelperInMemoryService.AddSessionId(sessionId, user.Email);

                TokenVm tokenPayload = new TokenVm()
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken,
                    RefreshTokenExpiryTime = DateTime.Now.AddHours(double.Parse(_appSettingConfiguration["JWT:RefreshTokenValidityInHours"])),
                };
                if (RefreshTokens.ContainsKey(user.Email))
                {
                    RefreshTokens[user.Email] = tokenPayload;
                }
                else
                {
                    RefreshTokens.Add(user.Email, tokenPayload);
                }

                bool isSubmitted;
                bool isStarted = false;
                if (tempUserTest == null)
                {
                    int? groupId = _context.Users.Where(x => x.Id == user.Id && x.IsDeleted == false).Select(x => x.GroupId).FirstOrDefault();
                    if (groupId != null)
                    {
                        Test? tempTest = _context.Tests.Where(x => x.GroupId == groupId && x.IsDeleted == false && x.Date == DateTime.Today).FirstOrDefault();
                        //Test? tempTest = _userActiveTestHelper.GetValidTestOfUser(user.Id);
                        if (tempTest == null)
                        {
                            isSubmitted = true;
                        }
                        else
                        {
                            if (Convert.ToDateTime(tempTest?.EndTime) <= DateTime.Now)
                            {
                                isSubmitted = true;
                            }
                            else
                            {
                                isSubmitted = false;
                            }

                        }
                    }
                    else
                    {
                        isSubmitted = true;
                    }
                }
                else
                {
                    isStarted = tempUserTest.IsStarted;
                    if (tempUserTest.IsAdminApproved && userTest == null)
                    {
                        isSubmitted = false;
                    }
                    else
                    {
                        isSubmitted = true;
                    }
                }
                TokenWithSidVm tokenWithSidVmPayload = new TokenWithSidVm()
                {
                    Id = user.Id,
                    AccessToken = tokenPayload.AccessToken,
                    RefreshToken = tokenPayload.RefreshToken,
                    RefreshTokenExpiryTime = tokenPayload.RefreshTokenExpiryTime,
                    Sid = sessionId,
                    IsSubmitted = isSubmitted,
                    IsStarted = isStarted,
                    IsProfileEdited = user.IsProfileEdited,
                    IsFirstLoggedIn = user.IsFirstLoggedIn
                };

                return new JsonResult(new ApiResponse<TokenWithSidVm> { Data = tokenWithSidVmPayload, Message = ResponseMessages.LoginSuccess, StatusCode = ResponseStatusCode.OK, Result = true });

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred for Email : {loginVm.Email} in UserAuthenticationRepository.Login \n MESSAGE : {ex.Message} \n INNER EXCEPTION : {ex.InnerException} \n");
                return new JsonResult(new ApiResponse<string> { Message = ResponseMessages.InternalError, StatusCode = ResponseStatusCode.InternalServerError, Result = false });
            }
        }
        #endregion

        #region ForgetPassword
        public async Task<JsonResult> ForgetPassword(string email)
        {
            try
            {
                User? user = _context.Users.Where(u => u.Email == email).FirstOrDefault();
                if (user == null)
                {
                    return new JsonResult(new ApiResponse<string> { Message = string.Format(ResponseMessages.NotFound, ModuleNames.User), StatusCode = ResponseStatusCode.NotFound, Result = false });
                }
                var sent = SendMailForResetPassword(user.FirstName, user.LastName, user.Email);
                if (sent)
                {
                    return new JsonResult(new ApiResponse<string> { Message = ResponseMessages.MailSentForForgetPassword, StatusCode = ResponseStatusCode.OK, Result = true });
                }
                else
                {
                    return new JsonResult(new ApiResponse<string> { Message = ResponseMessages.InternalError, StatusCode = ResponseStatusCode.InternalServerError, Result = false });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred for Email : {email} in UserAuthenticationRepository.ForgetPassword \n MESSAGE : {ex.Message} \n INNER EXCEPTION : {ex.InnerException} \n");
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

                User? user = _context.Users.Where(u => u.Email == decryptedEmail).FirstOrDefault();
                if (user == null)
                {
                    return new JsonResult(new ApiResponse<string> { Message = ResponseMessages.BadRequest, StatusCode = ResponseStatusCode.BadRequest, Result = false });
                }
                user.Password = resetPassword.NewPassword;
                user.UpdatedBy = user.Id;
                user.UpdatedDate = DateTime.Now.ToUniversalTime();
                _context.Update(user);
                _context.SaveChanges();
                return new JsonResult(new ApiResponse<string> { Message = ResponseMessages.PasswordUpdatedSuccess, StatusCode = ResponseStatusCode.OK, Result = true });

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in UserAuthenticationRepository.ResetPassword \n MESSAGE : {ex.Message} \n INNER EXCEPTION : {ex.InnerException} \n");
                return new JsonResult(new ApiResponse<string> { Message = ResponseMessages.InternalError, StatusCode = ResponseStatusCode.InternalServerError, Result = false });
            }
        }
        #endregion

        #region ChangePassword
        public async Task<JsonResult> ChangePassword(ChangePasswordVM changePassword)
        {
            try
            {
                User? user = await Task.FromResult(_context.Users.Where(x => x.Email.Equals(changePassword.Email) && x.Password.Equals(changePassword.CurrentPassword)).FirstOrDefault());
                if (user == null)
                {
                    return new JsonResult(new ApiResponse<string> { Message = ResponseMessages.WrongCurrentPassword, StatusCode = ResponseStatusCode.NotFound, Result = false });
                }
                if (user.Password.Equals(changePassword.NewPassword))
                {
                    return new JsonResult(new ApiResponse<string> { Message = ResponseMessages.currentAndNewSame, StatusCode = ResponseStatusCode.Forbidden, Result = false });
                }

                if (!changePassword.NewPassword.Equals(changePassword.confirmPassword))
                {
                    return new JsonResult(new ApiResponse<string> { Message = ResponseMessages.passwordNotMatched, StatusCode = ResponseStatusCode.Forbidden, Result = false });
                }
                user.Password = changePassword.NewPassword;
                user.IsFirstLoggedIn = true;
                user.UpdatedDate = DateTime.Now.ToUniversalTime();
                _context.Update(user);
                _context.SaveChanges();
                return new JsonResult(new ApiResponse<string> { Message = string.Format(ResponseMessages.UpdateSuccess, ModuleNames.Password), StatusCode = ResponseStatusCode.OK, Result = true });

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in UserAuthenticationRepository.ChangePassword \n MESSAGE : {ex.Message} \n INNER EXCEPTION : {ex.InnerException} \n");
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
                    string email = string.Empty;
                    var jwtHelper = new JwtHelper(_appSettingConfiguration);
                    string? accessToken = tokens.AccessToken;
                    string? refreshToken = tokens.RefreshToken;
                    if (!accessToken.IsNullOrEmpty())
                    {
                        var principal = jwtHelper.GetPrincipleFromExpiredToken(accessToken);
                        var allClaims = principal.Claims.ToList();
                        email = allClaims[4].Value;
                    }

                    RefreshTokens.GetValueOrDefault(email);
                    var user = await _context.Users.FirstOrDefaultAsync(U => U.Email == email);
                    if (user == null || RefreshTokens[email].RefreshToken != refreshToken || RefreshTokens[email].RefreshTokenExpiryTime <= DateTime.Now)
                    {
                        return new JsonResult(new ApiResponse<string> { Message = ResponseMessages.BadRequest, StatusCode = ResponseStatusCode.BadRequest, Result = false });
                    }
                    var newAccessToken = jwtHelper.GenerateJwtToken(user, null);
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
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in UserAuthenticationRepository.RefreshToken \n MESSAGE : {ex.Message} \n INNER EXCEPTION : {ex.InnerException} \n");
                return new JsonResult(new ApiResponse<string> { Data = null, Message = ResponseMessages.InternalError, StatusCode = ResponseStatusCode.InternalServerError, Result = false });
            }
        }
        #endregion

        #region SendEmail
        private static bool SendMailForResetPassword(string firstName, string lastName, string email)
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

                var subject = "Password reset request";
                var body = $"<h3>Hello {firstName} {lastName},</h3>We have received Password reset request from you,<br />Please click on the following link to reset your password.<br /><a href='{resetLink}'>Reset Password</a>";

                var emailHelper = new EmailHelper(_appSettingConfiguration, _context);
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

        #region Helpers

        private string GenerateSessionId()
        {
            Guid guid = Guid.NewGuid();
            return guid.ToString();
        }

        #endregion
    }
}
