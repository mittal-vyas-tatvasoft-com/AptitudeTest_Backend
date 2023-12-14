using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
