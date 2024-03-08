using AptitudeTest.Common.Helpers;
using AptitudeTest.Core.Entities.Test;
using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using AptitudeTest.Data.Common;
using APTITUDETEST.Common.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Drawing;
using System.Drawing.Imaging;

namespace AptitudeTest.Data.Data
{
    public class ScreenCaptureRepository : IScreenCaptureRepository
    {
        #region Properies

        private readonly AppDbContext _appDbContext;
        private readonly ILoggerManager _logger;
        private readonly UserActiveTestHelper _userActiveTestHelper;
        private readonly string? root;
        private readonly string? parent;
        private readonly string? screenshot;
        private readonly string? faceCam;

        #endregion

        #region Constructor
        public ScreenCaptureRepository(AppDbContext appDbContext, ILoggerManager logger, UserActiveTestHelper userActiveTestHelper, IConfiguration config)
        {
            _appDbContext = appDbContext;
            _logger = logger;
            IConfiguration _config;
            _config = config;
            _userActiveTestHelper = userActiveTestHelper;
            root = _config["FileSavePath:Root"];
            parent = _config["FileSavePath:ParentFolder"];
            screenshot = _config["FileSavePath:ScreenShot"];
            faceCam = _config["FileSavePath:UserFaceCam"];
        }
        #endregion

        #region Methods

        public async Task<JsonResult> CameraCapture([FromForm] ScreenCaptureVM data)
        {
            try
            {
                if (data.userId < 0)
                {
                    return new JsonResult(new ApiResponse<string>() { Message = ResponseMessages.BadRequest, Result = false, StatusCode = ResponseStatusCode.BadRequest });
                }
                //Test? test = _userActiveTestHelper.GetTestOfUser(data.userId);
                Test? test = _userActiveTestHelper.GetValidTestOfUser(data.userId);
                if (test == null)
                {
                    return new JsonResult(new ApiResponse<string>() { Message = string.Format(ResponseMessages.NotFound, ModuleNames.Test), Result = false, StatusCode = ResponseStatusCode.NotFound });
                }

                int testId = test.Id;
                string rootPath = Path.Combine(root, parent, testId.ToString(), data.userId.ToString());

                if (data.file != null)
                {
                    string path = Path.Combine(rootPath, faceCam);
                    Directory.CreateDirectory(path);
                    using (var image = Image.FromStream(data.file.OpenReadStream()))
                    using (var thumbnail = CreateThumbnail((Bitmap)image.Clone(), 500))
                    {
                        string thumbnailFileName = Guid.NewGuid().ToString() + "---" + data.userId + "---" + DateTime.Now.ToString().Replace(' ', '_').Replace(':', '_').Replace('-', '_') + ".jpeg";
                        string thumbnailPath = Path.Combine(path, thumbnailFileName);
                        thumbnail.Save(thumbnailPath, ImageFormat.Jpeg);
                    }
                }

                if (data.screenShot != null)
                {
                    string path = Path.Combine(rootPath, screenshot);
                    Directory.CreateDirectory(path);
                    using (var image = Image.FromStream(data.screenShot.OpenReadStream()))
                    using (var thumbnail = CreateThumbnail((Bitmap)image.Clone(), 5000))
                    {
                        string thumbnailFileName = Guid.NewGuid().ToString() + "---" + data.userId + "---" + DateTime.Now.ToString().Replace(' ', '_').Replace(':', '_').Replace('-', '_') + ".jpeg";
                        string thumbnailPath = Path.Combine(path, thumbnailFileName);
                        thumbnail.Save(thumbnailPath, ImageFormat.Jpeg);
                    }
                }
                return new JsonResult(new ApiResponse<string>() { Message = ResponseMessages.Success, Result = true, StatusCode = ResponseStatusCode.Success });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in ScreenCaptureRepository.CameraCapture \n MESSAGE : {ex.Message} \n INNER EXCEPTION : {ex.InnerException} \n");
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
