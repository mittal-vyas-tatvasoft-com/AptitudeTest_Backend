using AptitudeTest.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Core.Interfaces
{
    public interface IScreenCaptureService
    {
        Task<JsonResult> CameraCapture([FromForm] ScreenCaptureVM data);
    }
}
