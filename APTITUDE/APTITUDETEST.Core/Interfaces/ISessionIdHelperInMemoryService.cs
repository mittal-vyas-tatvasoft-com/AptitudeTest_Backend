using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AptitudeTest.Core.Enums;

namespace AptitudeTest.Core.Interfaces
{
    public interface ISessionIdHelperInMemoryService
    {
        CheckSessionIdResEnum CheckSessionId(string sessionId, string email);
        bool AddSessionId(string sessionId, string email);
        bool RemoveSessionId(string sessionId, string email);
    }
}
