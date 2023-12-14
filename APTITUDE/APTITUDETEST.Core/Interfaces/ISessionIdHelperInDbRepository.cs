namespace AptitudeTest.Core.Interfaces
{
    public interface ISessionIdHelperInDbRepository
    {
        bool CheckSessionId(string sessionId, string email);
    }
}
