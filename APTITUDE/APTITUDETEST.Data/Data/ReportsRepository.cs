using AptitudeTest.Core.Interfaces;
using APTITUDETEST.Common.Data;
using Microsoft.Extensions.Configuration;

namespace AptitudeTest.Data.Data
{
    public class ReportsRepository : IReportsRepository
    {
        #region Properties
        private readonly string? connectionString;
        private AppDbContext _context;
        #endregion

        #region Constructor
        public ReportsRepository(AppDbContext context, IConfiguration config)
        {
            IConfiguration _config;
            _config = config;
            connectionString = _config["ConnectionStrings:AptitudeTest"];
            _context = context;
        }
        #endregion

        #region Methods
        #endregion
    }
}
