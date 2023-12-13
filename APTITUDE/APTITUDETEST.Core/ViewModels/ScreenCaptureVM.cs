using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AptitudeTest.Core.ViewModels
{
    public class ScreenCaptureVM
    {
        public IFormFile file { get; set; }
        public IFormFile screenShot { get; set; }
    }
}
