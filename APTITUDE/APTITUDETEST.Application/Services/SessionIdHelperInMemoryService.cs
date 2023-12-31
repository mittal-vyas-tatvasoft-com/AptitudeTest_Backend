﻿using AptitudeTest.Core.Enums;
using AptitudeTest.Core.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace AptitudeTest.Application.Services
{
    public class EmailWithSessionId
    {
        public string Email { get; set; }
        public string SessionId { get; set; }
    }

    public class SessionIdHelperInMemoryService : ISessionIdHelperInMemoryService
    {
        private readonly IMemoryCache _memoryCache;

        public SessionIdHelperInMemoryService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public CheckSessionIdResEnum CheckSessionId(string sessionId, string email)
        {
            List<EmailWithSessionId> SessionIds = _memoryCache.Get<List<EmailWithSessionId>>("SessionIds") ?? new List<EmailWithSessionId>();
            var ele = SessionIds.Find(x => x.Email == email);

            if (ele != null && sessionId == ele.SessionId)
            {
                return CheckSessionIdResEnum.Valid;
            }
            else if (ele == null)
            {
                return CheckSessionIdResEnum.NotFound;
            }

            return CheckSessionIdResEnum.NotValid;
        }

        public bool AddSessionId(string sessionId, string email)
        {
            List<EmailWithSessionId> SessionIds = _memoryCache.Get<List<EmailWithSessionId>>("SessionIds") ?? new List<EmailWithSessionId>();
            var ele = SessionIds.Find(x => x.Email == email);

            if (ele != null)
            {
                ele.SessionId = sessionId;
            }
            else
            {
                SessionIds.Add(new EmailWithSessionId() { Email = email, SessionId = sessionId });
            }

            _memoryCache.Set("SessionIds", SessionIds);
            return false;
        }

        public bool RemoveSessionId(string sessionId, string email)
        {
            List<EmailWithSessionId> SessionIds = _memoryCache.Get<List<EmailWithSessionId>>("SessionIds") ?? new List<EmailWithSessionId>();
            var ele = SessionIds.Find(x => x.Email == email);

            if (ele != null)
            {
                SessionIds.Remove(ele);
            }

            _memoryCache.Set("SessionIds", SessionIds);
            return true;
        }
    }
}
