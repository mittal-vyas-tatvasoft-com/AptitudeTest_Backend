using AptitudeTest.Core.Interfaces;
using APTITUDETEST.Common.Data;
using AptitudeTest.Core.ViewModels;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Net.Http;
using System.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using AptitudeTest.Core.Enums;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Globalization;
using AptitudeTest.Application.Services;

namespace AptitudeTest.Filters
{
    public class SessionIdCheckFilterAttribute : IAsyncActionFilter
    {
        #region Properies
        private readonly ISessionIdHelperInMemoryService _sessionIdHelperInMemoryService;
        private readonly ISessionIdHelperInDbService _sessionIdHelperInDbService;
        #endregion

        #region Constructor
        public SessionIdCheckFilterAttribute(ISessionIdHelperInMemoryService sessionIdHelperInMemoryService, ISessionIdHelperInDbService sessionIdHelperInDbService)
        {
            _sessionIdHelperInMemoryService = sessionIdHelperInMemoryService;
            _sessionIdHelperInDbService = sessionIdHelperInDbService;
        }
        #endregion

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            const string HeaderKeyName = "Xid";
            const string HeaderKeyNameAuth = "Authorization";
            context.HttpContext.Request.Headers.TryGetValue(HeaderKeyName, out StringValues headerValue);
            context.HttpContext.Request.Headers.TryGetValue(HeaderKeyNameAuth, out StringValues tokenValue);

            var sessionId = headerValue.FirstOrDefault();
            var token = tokenValue.FirstOrDefault();

            if (sessionId != null && token != null)
            {
                var decodedToken = new JwtSecurityToken(token.Replace("Bearer ", ""));
                var email = decodedToken.Claims.Where(c => c.Type == "Email").Select(c => c.Value).SingleOrDefault();
                CheckSessionIdResEnum isValid = _sessionIdHelperInMemoryService.CheckSessionId(sessionId, email);

                if (isValid == CheckSessionIdResEnum.Valid)
                {
                    await next();
                }
                else if (isValid == CheckSessionIdResEnum.NotFound)
                {
                    var isValidInDb = _sessionIdHelperInDbService.CheckSessionId(sessionId, email);
                    if (isValidInDb)
                    {
                        _sessionIdHelperInMemoryService.AddSessionId(sessionId, email);
                        await next();
                    }
                    else
                    {
                        context.Result = new ContentResult()
                        {
                            StatusCode = 430,
                            Content = "Already logged in other device."
                        };
                    }
                }
                else
                {
                    context.Result = new ContentResult()
                    {
                        StatusCode = 430,
                        Content = "Already logged in other device."
                    };
                }
            }
        }
    }
}
