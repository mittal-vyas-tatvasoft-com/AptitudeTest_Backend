namespace AptitudeTest.Core.Interfaces
{
    public interface ILoggerManager
    {
        public void LogInfo(string message);
        public void LogError(string error);
    }
}
