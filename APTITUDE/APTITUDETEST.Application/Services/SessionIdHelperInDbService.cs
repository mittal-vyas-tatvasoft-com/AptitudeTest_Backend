using AptitudeTest.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AptitudeTest.Application.Services
{
    public class SessionIdHelperInDbService : ISessionIdHelperInDbService
    {
        #region Properties
        private readonly ISessionIdHelperInDbRepository _sessionIdHelperInDbRepository;
        #endregion

        #region Constructor
        public SessionIdHelperInDbService(ISessionIdHelperInDbRepository sessionIdHelperInDbRepository)
        {
            _sessionIdHelperInDbRepository = sessionIdHelperInDbRepository;
        }
        #endregion

        #region Method
        public bool CheckSessionId(string sessionId, string email)
        {
            return _sessionIdHelperInDbRepository.CheckSessionId(sessionId, email);
        }
        #endregion
    }
}
