using AptitudeTest.Core.Entities.Admin;
using AptitudeTest.Core.ViewModels;
using APTITUDETEST.Core.Entities.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AptitudeTest.Common.Helpers
{
    public class JWTHelper
    {
        private readonly IConfiguration _config;

        public JWTHelper(IConfiguration config)
        {
            _config = config;
        }
        #region 
        private static string TokenExpired = "Token has been expired please login again";
        private static string TokenInvalid = "token is invalid";
        #endregion

        #region JwtGenerateToken
        public string GenerateJwtToken(User? user, Admin? admin)
        {
            string jwtToken = string.Empty;
            try
            {
                // generate token that is valid for 20 minutes
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_config["JWT:AuthKey"]);
                double tokenExpiry = double.Parse(_config["JWT:TokenValidityInMinutes"]);
                var claims = new List<Claim>();
                if (user != null)
                {
                    claims.Add(new Claim("Id", user.Id.ToString()));
                    claims.Add(new Claim("FirstName", user.FirstName));
                    claims.Add(new Claim("Name", user.LastName));
                    claims.Add(new Claim("Role", "User"));
                    claims.Add(new Claim("Email", user.Email));
                }
                else
                {
                    if (admin.IsSuperAdmin == true)
                    {
                        claims.Add(new Claim("Role", "SuperAdmin"));
                    }
                    else
                    {
                        claims.Add(new Claim("Role", "Admin"));
                    }
                    claims.Add(new Claim("Id", admin.Id.ToString()));
                    claims.Add(new Claim("FirstName", admin.FirstName));
                    claims.Add(new Claim("Name", admin.LastName));
                    claims.Add(new Claim("Email", admin.Email));
                }
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
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
        public string CreateRefreshToken(string email, Dictionary<string, TokenVm> RefreshTokens)
        {
            try
            {
                var tokenBytes = RandomNumberGenerator.GetBytes(64);
                var refreshToken = Convert.ToBase64String(tokenBytes);
                TokenVm oldTokens = new TokenVm();
                var tokenInDictionary = RefreshTokens.TryGetValue(email, out oldTokens);
                if (oldTokens == null)
                {
                    return refreshToken;
                }
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
                    throw new SecurityTokenException(TokenExpired);
                }

            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region GetPrincipleFromExpiredToken
        public ClaimsPrincipal GetPrincipleFromExpiredToken(string token)
        {
            try
            {
                var TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:AuthKey"]))
                };

                var tokenHandler = new JwtSecurityTokenHandler();

                SecurityToken securityToken;
                var principle = tokenHandler.ValidateToken(token, TokenValidationParameters, out securityToken);
                var jwtSecurityToken = securityToken as JwtSecurityToken;
                if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    throw new SecurityTokenException();
                return principle;
            }
            catch
            {
                throw;
            }
        }
        #endregion

    }
}
