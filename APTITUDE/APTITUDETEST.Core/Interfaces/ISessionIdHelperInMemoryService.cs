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
