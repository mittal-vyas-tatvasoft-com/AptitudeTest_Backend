namespace AptitudeTest.Core.Interfaces
{
    public interface ISessionIdHelperInDbService
    {
        bool CheckSessionId(string sessionId, string email);
    }
}
