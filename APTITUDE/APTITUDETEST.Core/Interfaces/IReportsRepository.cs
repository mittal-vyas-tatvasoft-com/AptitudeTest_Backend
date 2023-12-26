using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Core.Interfaces
{
    public interface IReportsRepository
    {
        public Task<JsonResult> GetScreenShots(int userId, int testId);
        public Task<JsonResult> GetTests();
        public Task<JsonResult> GetUsers(int testId);
    }
}
