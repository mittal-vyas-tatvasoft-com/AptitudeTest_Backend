using AptitudeTest.Common.Data;
using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using AptitudeTest.Data.Common;
using APTITUDETEST.Common.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AptitudeTest.Data.Data
{
    public class ScreenCaptureRepository:IScreenCaptureRepository
    {
        #region Properies

        private readonly AppDbContext _appDbContext;

        #endregion

        #region Constructor
        public ScreenCaptureRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        #endregion

        #region Methods

        public async Task<JsonResult> CameraCapture([FromForm] ScreenCaptureVM data)
        {
            string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ScreenShots");
            string fileName = Guid.NewGuid().ToString() + "_" + 1 + ".jpeg";
            string filePath = Path.Combine(uploadFolder, fileName);
            string uploadFolder1 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files");
            string fileName1 = Guid.NewGuid().ToString() + "_" + 1 + ".jpeg";
            string filePath1 = Path.Combine(uploadFolder1, fileName1);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                data.file.CopyTo(fileStream);
            }
            using (var fileStream = new FileStream(filePath1, FileMode.Create))
            {
                data.screenShot.CopyTo(fileStream);
            }

            return new JsonResult(new ApiResponse<string>() { Message = ResponseMessages.BadRequest, Result = false, StatusCode = ResponseStatusCode.BadRequest });
        }

        #endregion
    }
}
