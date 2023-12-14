using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using AptitudeTest.Data.Common;
using APTITUDETEST.Common.Data;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Drawing.Imaging;

namespace AptitudeTest.Data.Data
{
    public class ScreenCaptureRepository : IScreenCaptureRepository
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
            try
            {
                if (data.file != null)
                {
                    using (var image = Image.FromStream(data.file.OpenReadStream()))
                    using (var thumbnail = CreateThumbnail((Bitmap)image.Clone(), 500))
                    {
                        string wwwrootPath = "wwwroot/UserFaceCamShots";
                        string thumbnailFileName = Guid.NewGuid().ToString() + "---" + data.userId + "---" + DateTime.Now.ToString().Replace(' ', '_').Replace(':', '_').Replace('-', '_') + ".jpeg";
                        string thumbnailPath = Path.Combine(wwwrootPath, thumbnailFileName);
                        thumbnail.Save(thumbnailPath, ImageFormat.Jpeg);
                    }
                }

                if (data.screenShot != null)
                {
                    using (var image = Image.FromStream(data.screenShot.OpenReadStream()))
                    using (var thumbnail = CreateThumbnail((Bitmap)image.Clone(), 500))
                    {
                        string wwwrootPath = "wwwroot/ScreenShots";
                        string thumbnailFileName = Guid.NewGuid().ToString() + "---" + data.userId + "---" + DateTime.Now.ToString().Replace(' ', '_').Replace(':', '_').Replace('-', '_') + ".jpeg";
                        string thumbnailPath = Path.Combine(wwwrootPath, thumbnailFileName);
                        thumbnail.Save(thumbnailPath, ImageFormat.Jpeg);
                    }
                }
                return new JsonResult(new ApiResponse<string>() { Message = ResponseMessages.Success, Result = true, StatusCode = ResponseStatusCode.Success });
            }
            catch
            {

                return new JsonResult(new ApiResponse<string>() { Message = ResponseMessages.InternalError, Result = false, StatusCode = ResponseStatusCode.InternalServerError });
            }
        }


        #endregion

        #region helper

        private Bitmap CreateThumbnail(Image original, int width)
        {
            int height = (int)(width * ((float)original.Height / original.Width));
            var thumbnail = new Bitmap(original, new Size(width, height));
            return new Bitmap(thumbnail);
        }
        #endregion

    }
}
