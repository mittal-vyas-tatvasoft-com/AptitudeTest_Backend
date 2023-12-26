using AptitudeTest.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Application.Services
{
    public class ReportsService : IReportsService
    {
        #region Properties
        private readonly IReportsRepository _repository;
        #endregion

        #region Constructor
        public ReportsService(IReportsRepository repository)
        {
            _repository = repository;
        }
        #endregion

        #region Methods
        public async Task<JsonResult> GetScreenShots(int userId, int testId)
        {
            return await _repository.GetScreenShots(userId, testId);
        }

        public async Task<JsonResult> GetTests()
        {
            return await _repository.GetTests();
        }

        public async Task<JsonResult> GetUsers(int testId)
        {
            return await _repository.GetUsers(testId);
        }
        #endregion
    }
}
