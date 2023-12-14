using Microsoft.AspNetCore.Http;

namespace AptitudeTest.Core.ViewModels
{
    public class ScreenCaptureVM
    {
        public int userId { get; set; }
        public IFormFile? file { get; set; }
        public IFormFile? screenShot { get; set; }
    }
}
