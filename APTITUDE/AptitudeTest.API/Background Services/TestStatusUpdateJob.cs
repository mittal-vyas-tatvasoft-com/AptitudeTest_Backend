using AptitudeTest.Core.Entities.Test;
using AptitudeTest.Core.Interfaces;
using APTITUDETEST.Common.Data;
using Microsoft.EntityFrameworkCore;
using static AptitudeTest.Data.Common.Enums;

namespace AptitudeTest.Background_Services
{
    public class TestStatusUpdateJob : BackgroundService
    {
        #region Properties
        private string connectionString;
        private AppDbContext _context;
        private readonly ILoggerManager _logger;
        private Timer? _timer;
        int jobRunTime;
        int extraTime;
        #endregion

        #region Constructor
        public TestStatusUpdateJob(ILoggerManager logger, IConfiguration config)
        {

            IConfiguration _config;
            _config = config;
            jobRunTime = Int32.Parse(_config["TestStatusUpdate:JobRunTime"]);
            extraTime = Int32.Parse(_config["TestStatusUpdate:TimeBeforeStartTimetoUpdateStatus"]);
            connectionString = _config.GetConnectionString("AptitudeTest");
            var optionBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionBuilder.UseNpgsql(connectionString);
            _context = new AppDbContext(optionBuilder.Options);
            _logger = logger;
            SetMainTimer();
        }
        #endregion

        #region Methods
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }

        private void SetMainTimer()
        {
            DateTime now = DateTime.Now;
            DateTime scheduledTime = new DateTime(now.Year, now.Month, now.Day, jobRunTime, 0, 0);
            if (now > scheduledTime)
            {
                scheduledTime = scheduledTime.AddDays(1);
            }
            TimeSpan timeUntilScheduled = scheduledTime - now;
            _timer = new Timer(x => CalculateTime(), null, (int)timeUntilScheduled.TotalMilliseconds, 24 * 60 * 60 * 1000);
        }

        private void CalculateTime()
        {
            try
            {
                DateTime today = DateTime.Today;
                List<DateTime> dateTimes = new List<DateTime>();
                List<Test> tests = _context.Tests.Where(t => t.StartTime.Date == today.Date).ToList();
                if (tests != null)
                {
                    foreach (var test in tests)
                    {
                        Task.Delay((int)CalculateDelay(test.StartTime)).ContinueWith(task => UpdateStatus(test.Id));
                    }
                }
            }

            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in TestStatusUpdateJob.CalculateTime:{ex} at: {DateTime.Now}");
            }

        }

        private void UpdateStatus(int testId)
        {
            try
            {
                Test test = _context.Tests.Where(t => t.Id == testId).FirstOrDefault();
                test.Status = (int)TestStatus.Active;
                _context.Update(test);
                _context.SaveChanges();
            }

            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in TestStatusUpdateJob.UpdateStatus:{ex} at: {DateTime.Now}");
            }

        }

        private double CalculateDelay(DateTime dateTime)
        {
            DateTime now = DateTime.Now;
            TimeSpan timeSpan = dateTime - now;
            return timeSpan.TotalMilliseconds - extraTime * 60 * 1000;
        }
        #endregion
    }
}
