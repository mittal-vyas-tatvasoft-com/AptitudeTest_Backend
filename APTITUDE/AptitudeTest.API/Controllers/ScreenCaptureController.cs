using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using AptitudeTest.Data.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AptitudeTest.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ScreenCaptureController : ControllerBase
    {
        #region Properties
        private readonly IScreenCaptureService _screenCaptureService;
        #endregion

        #region Constructor
        public ScreenCaptureController(IScreenCaptureService screenCaptureService)
        {
            _screenCaptureService = screenCaptureService;
        }
        #endregion

        #region Methods

        #region CameraCapture

        [HttpPost("[action]")]
        public async Task<JsonResult> CameraCapture([FromForm] ScreenCaptureVM data)
        {
            return await _screenCaptureService.CameraCapture(data);
        }

        #endregion

        #endregion
    }
}
