﻿using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using APTITUDETEST.Common.Data;
using APTITUDETEST.Core.Entities.Users;
using Microsoft.Extensions.Configuration;

namespace AptitudeTest.Data.Data
{
    public class SessionIdHelperInDbRepository : ISessionIdHelperInDbRepository
    {
        #region Properies
        private readonly AppDbContext _context;
        public static Dictionary<string, TokenVm> RefreshTokens = new Dictionary<string, TokenVm>();
        #endregion

        #region Constructor
        public SessionIdHelperInDbRepository(AppDbContext context, IConfiguration appSettingConfiguration)
        {
            _context = context;
        }
        #endregion

        #region Methods

        public bool CheckSessionId(string sessionId, string email)
        {
            try
            {
                User? user = _context.Users.Where(u => u.Email == email.Trim() && u.SessionId == sessionId.Trim() && u.IsDeleted == false)?.FirstOrDefault();
                if (user == null)
                {
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

    }
}