﻿using AptitudeTest.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Core.Interfaces
{
    public interface IReportsService
    {
        public Task<JsonResult> GetScreenShots(int userId, int testId, int imageType);
        public Task<JsonResult> GetTests();
        public Task<JsonResult> GetUsers(int testId);
        public Task<JsonResult> GetUserDirectories(int userId, int testId);
        public Task<JsonResult> DeleteDirectory(DeleteScreenShotsVM deleteScreenShotsVM);
    }
}
