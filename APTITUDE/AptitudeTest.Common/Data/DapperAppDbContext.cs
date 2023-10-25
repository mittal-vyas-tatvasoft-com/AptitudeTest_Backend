using AptitudeTest.Core.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;
namespace AptitudeTest.Common.Data
{
    public class DapperAppDbContext : DbContext
    {
        private IConfiguration _configuration;
        private readonly string _connectionString;
        public DapperAppDbContext(DbContextOptions<DapperAppDbContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;

            _connectionString = _configuration.GetConnectionString("AptitudeTest");
        }

        public IDbConnection CreateConnection()
        {
            //var dataSourceBuilder = new NpgsqlDataSourceBuilder(_connectionString);
            //dataSourceBuilder.MapComposite<userfamilyvm>();
            //var dataSource = dataSourceBuilder.Build();

            //return new NpgsqlConnection(_connectionString);


            NpgsqlDataSourceBuilder dataSourceBuilder = new NpgsqlDataSourceBuilder(_connectionString);
            using NpgsqlDataSource dataSource = dataSourceBuilder.Build();
            dataSourceBuilder.MapComposite<DapperUserFamilyVM>("userfamily_datatype");
            dataSourceBuilder.MapComposite<DapperUserAcademicsVM>("useracademics_datatype");
            NpgsqlConnection conn = dataSource.OpenConnection();
            return conn;
        }
    }
}
