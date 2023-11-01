using AptitudeTest.Common.Helpers;
using AptitudeTest.Core.Interfaces.UserAuthentication;
using AptitudeTest.Core.ViewModels;
using AptitudeTest.Data.Common;
using APTITUDETEST.Common.Data;
using APTITUDETEST.Core.Entities.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
//using ResponseMessages = AptitudeTest.Data.Common.ResponseMessages;

namespace AptitudeTest.Data.Data
{
    public class UserAuthenticationRepository : IUserAuthenticationRepository
    {
        #region Properies
        private readonly AppDbContext _context;
        static IConfiguration _appSettingConfiguration;
        public static Dictionary<string, TokenVm> RefreshTokens = new Dictionary<string, TokenVm>();
        #endregion

        #region Constructor
        public UserAuthenticationRepository(AppDbContext context, IConfiguration appSettingConfiguration)
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
                if (!string.IsNullOrEmpty(loginVm.Email) && !string.IsNullOrEmpty(loginVm.Email))
                {
                    User? user = _context.Users.Where(u => u.Email == loginVm.Email && u.Password == loginVm.Password)?.FirstOrDefault();
                    if (user != null)
                    {
                        string newAccessToken = GenerateJwtToken(user);
                        string newRefreshToken = CreateRefreshToken(user.Email);

                        if (!string.IsNullOrEmpty(newAccessToken) && !string.IsNullOrEmpty(newRefreshToken))
                        {

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
                            return new JsonResult(new ApiResponse<TokenVm> { Data = tokenPayload, Message = ResponseMessages.LoginSuccess, StatusCode = ResponseStatusCode.OK, Result = true });
                        }
                        else
                        {
                            return new JsonResult(new ApiResponse<string> { Message = ResponseMessages.BadRequest, StatusCode = ResponseStatusCode.BadRequest, Result = false });
                        }
                    }
                    else
                    {
                        return new JsonResult(new ApiResponse<User> { Message = ResponseMessages.InvalidCredetials, StatusCode = ResponseStatusCode.Unauthorized, Result = false });
                    }
                }
                else
                {
                    return new JsonResult(new ApiResponse<string> { Message = ResponseMessages.BadRequest, StatusCode = ResponseStatusCode.BadRequest, Result = false });
                }
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
                User? user = _context.Users.Where(u => u.Email == email).FirstOrDefault();
                if (user != null)
                {
                    var sent = SendMailForResetPassword(user.FirstName, user.Email);
                    if (sent)
                    {
                        return new JsonResult(new ApiResponse<string> { Message = ResponseMessages.MailSentForForgetPassword, StatusCode = ResponseStatusCode.OK, Result = true });
                    }
                    else
                    {
                        return new JsonResult(new ApiResponse<string> { Message = ResponseMessages.InternalError, StatusCode = ResponseStatusCode.InternalServerError, Result = false });
                    }

                }
                else
                {
                    return new JsonResult(new ApiResponse<string> { Message = string.Format(ResponseMessages.NotFound, "User"), StatusCode = ResponseStatusCode.NotFound, Result = false });
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
                if (!string.IsNullOrEmpty(resetPassword.EncryptedEmail) && !string.IsNullOrEmpty(resetPassword.NewPassword))
                {
                    byte[] byteForEmail = Convert.FromBase64String(resetPassword.EncryptedEmail);
                    string decryptedEmail = Encoding.ASCII.GetString(byteForEmail);

                    User? user = _context.Users.Where(u => u.Email == decryptedEmail).FirstOrDefault();
                    if (user != null)
                    {
                        user.Password = resetPassword.NewPassword;
                        user.UpdatedBy = user.Id;
                        user.UpdatedDate = DateTime.Now.ToUniversalTime();
                        _context.Update(user);
                        _context.SaveChanges();
                        return new JsonResult(new ApiResponse<string> { Message = ResponseMessages.PasswordUpdatedSuccess, StatusCode = ResponseStatusCode.OK, Result = true });
                    }
                    else
                    {
                        return new JsonResult(new ApiResponse<string> { Message = ResponseMessages.BadRequest, StatusCode = ResponseStatusCode.BadRequest, Result = false });
                    }
                }
                else
                {
                    return new JsonResult(new ApiResponse<string> { Message = ResponseMessages.BadRequest, StatusCode = ResponseStatusCode.BadRequest, Result = false });
                }
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
                User? user = await Task.FromResult(_context.Users.Where(x => x.Email.Equals(changePassword.Email) && x.Password.Equals(changePassword.CurrentPassword)).FirstOrDefault());
                if (user != null)
                {
                    user.Password = changePassword.NewPassword;
                    user.UpdatedDate = DateTime.Now.ToUniversalTime();
                    _context.Update(user);
                    _context.SaveChanges();
                    return new JsonResult(new ApiResponse<string> { Message = string.Format(ResponseMessages.UpdateSuccess, "Password"), StatusCode = ResponseStatusCode.OK, Result = true });
                }
                else
                {
                    return new JsonResult(new ApiResponse<string> { Message = ResponseMessages.BadRequest, StatusCode = ResponseStatusCode.BadRequest, Result = false });
                }
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
                    string accessToken = tokens.AccessToken;
                    string refreshToken = tokens.RefreshToken;
                    var principal = GetPrincipleFromExpiredToken(accessToken);
                    var allClaims = principal.Claims.ToList();
                    var email = allClaims[3].Value;
                    var tokenssss = RefreshTokens.GetValueOrDefault(email);
                    var user = await _context.Users.FirstOrDefaultAsync(U => U.Email == email);
                    if (user == null || RefreshTokens[email].RefreshToken != refreshToken || RefreshTokens[email].RefreshTokenExpiryTime <= DateTime.Now)
                    {
                        return new JsonResult(new ApiResponse<string> { Message = ResponseMessages.BadRequest, StatusCode = ResponseStatusCode.BadRequest, Result = false });
                    }
                    else
                    {
                        var newAccessToken = GenerateJwtToken(user);
                        var newRefreshToken = CreateRefreshToken(email);
                        if (!string.IsNullOrEmpty(newAccessToken) && !string.IsNullOrEmpty(newRefreshToken))
                        {
                            RefreshTokens[email].RefreshToken = newRefreshToken;
                            TokenVm tokenPayload = new TokenVm()
                            {
                                AccessToken = newAccessToken,
                                RefreshToken = newRefreshToken,
                            };
                            return new JsonResult(new ApiResponse<TokenVm> { Data = tokenPayload, Message = ResponseMessages.SessionRefresh, StatusCode = ResponseStatusCode.OK, Result = true });
                        }
                        else
                        {
                            return new JsonResult(new ApiResponse<string> { Message = ResponseMessages.BadRequest, StatusCode = ResponseStatusCode.BadRequest, Result = false });
                        }
                    }
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

        #region JwtGenerateToken
        private string GenerateJwtToken(User user)
        {
            string jwtToken = string.Empty;
            try
            {
                // generate token that is valid for 20 minutes
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettingConfiguration["JWT:AuthKey"]);
                double tokenExpiry = double.Parse(_appSettingConfiguration["JWT:TokenValidityInMinutes"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                        {
                        new Claim("Id", user.Id.ToString()),
                        new Claim("FirstName", user.FirstName),
                        new Claim("Name", user.Email),
                        new Claim("Email", user.Email),
                        new Claim("LastName", user.LastName)
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(tokenExpiry),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                jwtToken = tokenHandler.WriteToken(token);
            }
            catch (Exception)
            {
                throw;
            }
            return jwtToken;
        }

        #endregion

        #region CreateRefreshToken
        private string CreateRefreshToken(string email)
        {
            try
            {
                var tokenBytes = RandomNumberGenerator.GetBytes(64);
                var refreshToken = Convert.ToBase64String(tokenBytes);
                TokenVm oldTokens = new TokenVm();
                var tokenInDictionary = RefreshTokens.TryGetValue(email, out oldTokens);
                if (oldTokens != null)
                {
                    if (oldTokens.RefreshTokenExpiryTime > DateTime.Now)
                    {
                        return refreshToken;
                    }
                    else
                    {
                        if (tokenInDictionary)
                        {
                            RefreshTokens.Remove(email);
                        }
                        throw new SecurityTokenException(ResponseMessages.TokenExpired);
                    }
                }
                else
                {
                    return refreshToken;
                }
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region GetPrincipleFromExpiredToken
        private ClaimsPrincipal GetPrincipleFromExpiredToken(string token)
        {
            try
            {
                var TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettingConfiguration["JWT:AuthKey"]))
                };

                var tokenHandler = new JwtSecurityTokenHandler();

                SecurityToken securityToken;
                var principle = tokenHandler.ValidateToken(token, TokenValidationParameters, out securityToken);
                var jwtSecurityToken = securityToken as JwtSecurityToken;
                if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    throw new SecurityTokenException(ResponseMessages.TokenInvalid);
                return principle;
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region test
        public async Task<JsonResult> GetAllUsers()
        {
            var users = _context.Users.ToList();
            return new JsonResult(new ApiResponse<List<User>> { Data = users, Message = ResponseMessages.LoginSuccess, StatusCode = ResponseStatusCode.OK, Result = true });
        }
        #endregion

        #endregion
    }
}
