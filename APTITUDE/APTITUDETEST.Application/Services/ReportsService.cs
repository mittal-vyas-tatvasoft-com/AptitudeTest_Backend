using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
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
        public async Task<JsonResult> GetScreenShots(int userId, int testId, int imageType)
        {
            return await _repository.GetScreenShots(userId, testId, imageType);
        }

        public async Task<JsonResult> GetTests()
        {
            return await _repository.GetTests();
        }

        public async Task<JsonResult> GetUsers(int testId)
        {
            return await _repository.GetUsers(testId);
        }

        public async Task<JsonResult> GetUserDirectories(int userId, int testId)
        {
            return await _repository.GetUserDirectories(userId, testId);
        }

        public async Task<JsonResult> DeleteDirectory(DeleteScreenShotsVM deleteScreenShotsVM)
        {
            return await _repository.DeleteDirectory(deleteScreenShotsVM);
        }
        #endregion
    }
}
