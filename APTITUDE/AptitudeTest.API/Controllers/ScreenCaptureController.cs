using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using AptitudeTest.Data.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AptitudeTest.Controllers
{
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

        #region CameraTest

        [HttpPost("[action]")]
        public async Task<JsonResult> CameraTest([FromForm] ScreenCaptureVM data)
        {
            string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ScreenShots");
            string fileName = Guid.NewGuid().ToString() + "_" + 1 +".jpeg";
            string filePath = Path.Combine(uploadFolder, fileName);
            string type = data.file.ContentType;
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                data.file.CopyTo(fileStream);
            }

            return new JsonResult(new ApiResponse<string>() { Message = ResponseMessages.BadRequest, Result = false, StatusCode = ResponseStatusCode.BadRequest });
        }

        #endregion

        #endregion
    }
}
