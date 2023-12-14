using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Application.Services
{
    public class ScreenCaptureService : IScreenCaptureService
    {
        #region Properties
        private readonly IScreenCaptureRepository _screenCaptureRepository;
        #endregion

        #region Constructor
        public ScreenCaptureService(IScreenCaptureRepository screenCaptureRepository)
        {
            _screenCaptureRepository = screenCaptureRepository;
        }
        #endregion

        #region Methods
        public async Task<JsonResult> CameraCapture([FromForm] ScreenCaptureVM data)
        {
            return await _screenCaptureRepository.CameraCapture(data);
        }
        #endregion

    }
}
