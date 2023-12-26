﻿using AptitudeTest.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportsController : ControllerBase
    {
        #region Properties
        private readonly IReportsService _service;
        #endregion

        #region Constructor
        public ReportsController(IReportsService service)
        {
            _service = service;
        }
        #endregion

        #region Methods
        [HttpGet("[action]/{userId:int}/{testId:int}")]
        public async Task<JsonResult> GetScreenShots(int userId, int testId)
        {
            return await _service.GetScreenShots(userId, testId);
        }

        [HttpGet("[action]")]
        public async Task<JsonResult> GetTests()
        {
            return await _service.GetTests();
        }

        [HttpGet("[action]/{testId:int}")]
        public async Task<JsonResult> GetUsers(int testId)
        {
            return await _service.GetUsers(testId);
        }
        #endregion
    }
}
