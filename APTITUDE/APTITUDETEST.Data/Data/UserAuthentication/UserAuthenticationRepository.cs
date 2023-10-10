using AptitudeTest.Core.Interfaces.UserAuthentication;
using AptitudeTest.Core.ViewModels;
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

namespace AptitudeTest.Data.Data.UserAuthentication
{
    public class UserAuthenticationRepository : RepositoryBase<User>, IUserAuthenticationRepository
    {
        #region prperties
        private readonly AppDbContext _context;
        static IConfiguration _appSettingConfiguration;
        public static Dictionary<string, TokenVm> RefreshTokens = new Dictionary<string, TokenVm>();
        #endregion

        #region Constructor
        public UserAuthenticationRepository(AppDbContext context, IConfiguration appSettingConfiguration) : base(context)
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
                    User? user = _context.Users.Where(u => u.Email == loginVm.Email && u.Password == loginVm.Password).FirstOrDefault();
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
                                RefreshTokenExpiryTime = DateTime.Now.AddMinutes(1),
                            };
                            if (RefreshTokens.ContainsKey(user.Email))
                            {
                                RefreshTokens[user.Email] = tokenPayload;
                            }
                            else
                            {
                                RefreshTokens.Add(user.Email, tokenPayload);
                            }
                            return new JsonResult(new ApiResponseVm<TokenVm> { Data = tokenPayload, Message = ResponseMessages.LoginSuccess, StatusCode = ResponseStatusCodes.OK, Result = true });
                        }
                        else
                        {
                            return new JsonResult(new ApiResponseVm<string> { Message = ResponseMessages.BadRequest, StatusCode = ResponseStatusCodes.BadRequest, Result = false });
                        }
                    }
                    else
                    {
                        return new JsonResult(new ApiResponseVm<User> { Message = ResponseMessages.InvalidCredetials, StatusCode = ResponseStatusCodes.Unauthorized, Result = false });
                    }
                }
                else
                {
                    return new JsonResult(new ApiResponseVm<string> { Message = ResponseMessages.BadRequest, StatusCode = ResponseStatusCodes.BadRequest, Result = false });
                }
            }
            catch
            {
                return new JsonResult(new ApiResponseVm<string> { Message = ResponseMessages.InternalServerError, StatusCode = ResponseStatusCodes.InternalServerError, Result = false });
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
                    byte[] byteForEmail = Encoding.ASCII.GetBytes(user.Email);
                    string encryptedEmail = Convert.ToBase64String(byteForEmail);
                    UriBuilder builder = new();
                    builder.Scheme = "http";
                    builder.Host = "localhost";
                    builder.Port = 4200;
                    builder.Path = "/ResetPassword";
                    builder.Query = "&email=" + encryptedEmail;
                    var resetLink = builder.ToString();
                    // Send email to user with reset password link
                    // ...
                    var fromAddress = new MailAddress(_appSettingConfiguration["EmailGeneration:FromEmail"], _appSettingConfiguration["EmailGeneration:DisplayName"]);
                    var toAddress = new MailAddress(user.Email);
                    var subject = "Password reset request";
                    var body = $"<h3>Hello {user.FirstName}</h3>,<br />we received password reset request from your side,<br /><br />Please click on the following link to reset your password <br /><br /><a href='{resetLink}'><h3>Click here</h3></a>";

                    EmailDataVm emailData = new EmailDataVm()
                    {
                        FromAddress = fromAddress,
                        ToAddress = toAddress,
                        Subject = subject,
                        Body = body
                    };
                    SendEmailForForgetPassword(emailData);

                }
                return new JsonResult(new ApiResponseVm<string> { Message = ResponseMessages.MailSentForForgetPassword, StatusCode = ResponseStatusCodes.OK, Result = true });
            }
            catch
            {
                return new JsonResult(new ApiResponseVm<string> { Data = null, Message = ResponseMessages.InternalServerError, StatusCode = ResponseStatusCodes.InternalServerError, Result = false });

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
                    return new JsonResult(new ApiResponseVm<string> { Message = ResponseMessages.BadRequest, StatusCode = ResponseStatusCodes.BadRequest, Result = false });
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
                        return new JsonResult(new ApiResponseVm<string> { Message = ResponseMessages.BadRequest, StatusCode = ResponseStatusCodes.BadRequest, Result = false });
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
                            return new JsonResult(new ApiResponseVm<TokenVm> { Data = tokenPayload, Message = ResponseMessages.SessionRefresh, StatusCode = ResponseStatusCodes.OK, Result = true });
                        }
                        else
                        {
                            return new JsonResult(new ApiResponseVm<string> { Message = ResponseMessages.BadRequest, StatusCode = ResponseStatusCodes.BadRequest, Result = false });
                        }
                    }
                }
            }
            catch
            {
                return new JsonResult(new ApiResponseVm<string> { Data = null, Message = ResponseMessages.InternalServerError, StatusCode = ResponseStatusCodes.InternalServerError, Result = false });
            }
        }
        #endregion

        #region SendEmail
        private static bool SendEmailForForgetPassword(EmailDataVm EmailData)
        {
            var message = new MailMessage(EmailData.FromAddress, EmailData.ToAddress)
            {
                Subject = EmailData.Subject,
                Body = EmailData.Body,
                IsBodyHtml = true
            };
            message.Priority = MailPriority.High;
            try
            {
                var smtpClient = new SmtpClient(_appSettingConfiguration["EmailGeneration:Host"], 587)
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(_appSettingConfiguration["EmailGeneration:FromEmail"], _appSettingConfiguration["EmailGeneration:Key"]),
                    EnableSsl = true,
                };
                smtpClient.Send(message);
                return true;
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
                        new Claim("RoleId", user.RoleId.ToString())
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
            return new JsonResult(new ApiResponseVm<List<User>> { Data = users, Message = ResponseMessages.LoginSuccess, StatusCode = ResponseStatusCodes.OK, Result = true });
        }
        #endregion

        #endregion
    }
}
