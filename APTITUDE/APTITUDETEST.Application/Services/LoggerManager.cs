using AptitudeTest.Core.Interfaces;
using NLog;

namespace AptitudeTest.Application.Services
{
    public class LoggerManager : ILoggerManager
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();
        public void LogError(string message) => logger.Error(message);
        public void LogInfo(string message) => logger.Info(message);
    }
}
