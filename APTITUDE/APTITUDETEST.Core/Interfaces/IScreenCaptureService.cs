using AptitudeTest.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AptitudeTest.Core.Interfaces
{
    public interface IScreenCaptureService
    {
        Task<JsonResult> CameraCapture([FromForm] ScreenCaptureVM data);
    }
}
