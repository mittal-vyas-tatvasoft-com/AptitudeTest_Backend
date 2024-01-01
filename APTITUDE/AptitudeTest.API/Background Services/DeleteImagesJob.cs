using AptitudeTest.Core.Interfaces;

namespace AptitudeTest.Background_Services
{
    public class DeleteImagesJob : BackgroundService
    {
        private readonly ILoggerManager _logger;
        private readonly string? root;
        private readonly string? parent;
        private Timer? _timer;
        private int deleteDurationInDays;

        public DeleteImagesJob(ILoggerManager logger, IConfiguration config)
        {
            _logger = logger;
            IConfiguration _config;
            _config = config;
            root = _config["FileSavePath:Root"];
            parent = _config["FileSavePath:ParentFolder"];
            deleteDurationInDays = Int32.Parse(_config["DeleteImages:DurationInDays"]);
            SetTimer();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }

        private void SetTimer()
        {
            DateTime now = DateTime.Now;
            DateTime scheduledTime = new DateTime(now.Year, now.Month, now.Day, 20, 0, 0);
            if (now > scheduledTime)
            {
                scheduledTime = scheduledTime.AddDays(1);
            }
            TimeSpan timeUntilScheduled = scheduledTime - now;
            _timer = new Timer(x => DeleteImages(), null, (int)timeUntilScheduled.TotalMilliseconds, 24 * 60 * 60 * 1000);
        }

        private void DeleteImages()
        {
            try
            {
                string[] allFiles = Directory.GetFiles(Path.Combine(root, parent), "*.*", SearchOption.AllDirectories);
                foreach (string file in allFiles)
                {
                    FileInfo fileInfo = new FileInfo(file);
                    if (fileInfo.CreationTime < DateTime.Now.AddDays(deleteDurationInDays*-1))
                    {
                        fileInfo.Delete();
                    }
                }
                DeleteEmptyDirectory(Path.Combine(root, parent));
                _logger.LogInfo($"Images Deleted Successfully at {DateTime.Now}");
            }

            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in DeleteImagesJob.DeleteImages:{ex} at: {DateTime.Now}");
            }
        }

        public void DeleteEmptyDirectory(string path)
        {
            try
            {
                if (Directory.Exists(path))
                {
                    string[] directories = Directory.GetDirectories(path);
                    foreach (string directory in directories)
                    {
                        DeleteEmptyDirectory(directory);
                    }
                    directories = Directory.GetDirectories(path);
                    string[] files = Directory.GetFiles(path);
                    if (directories.Length == 0 && files.Length == 0 && path != Path.Combine(root, parent))
                    {
                        Directory.Delete(path, true);
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in DeleteImagesJob.DeleteEmptyDirectory:{ex} at: {DateTime.Now}");
            }
        }
    }
}
