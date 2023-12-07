using Microsoft.Extensions.Configuration;
using Npgsql;

namespace AptitudeTest.Data.Common
{
    public class DbConnection : IDisposable
    {
        public NpgsqlConnection Connection { get; set; }

        readonly IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();

        public DbConnection()
        {
            var connectionString = configuration.GetConnectionString("AptitudeTest");
            Connection = new NpgsqlConnection(connectionString);
            Connection.Open();
        }

        public void Close()
        {
            Connection.Close();
        }

        public void Dispose()
        {
            Connection.Dispose();
            GC.SuppressFinalize(this);
        }

        ~DbConnection()
        {
            Connection.Dispose();
        }
    }
}
