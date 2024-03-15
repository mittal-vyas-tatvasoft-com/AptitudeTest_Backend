using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using AptitudeTest.Data.Common;
using APTITUDETEST.Common.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using static AptitudeTest.Data.Common.Enums;

namespace AptitudeTest.Data.Data
{
    public class ReportsRepository : IReportsRepository
    {
        #region Properties
        private readonly string? root;
        private readonly string? parent;
        private readonly string? screenshot;
        private readonly string? faceCam;
        private AppDbContext _context;
        private readonly ILoggerManager _logger;
        #endregion

        #region Constructor
        public ReportsRepository(AppDbContext context, IConfiguration config, ILoggerManager logger)
        {
            _logger = logger;
            IConfiguration _config;
            _config = config;
            root = _config["FileSavePath:Root"];
            parent = _config["FileSavePath:ParentFolder"];
            screenshot = _config["FileSavePath:ScreenShot"];
            faceCam = _config["FileSavePath:UserFaceCam"];
            _context = context;
        }
        #endregion

        #region Methods
        public async Task<JsonResult> GetScreenShots(int userId, int testId, int imageType)
        {
            try
            {
                List<UserImageVM> imagePaths = new List<UserImageVM>();
                string path = Path.Combine(root, parent, testId.ToString(), userId.ToString());
                if (Directory.Exists(path))
                {
                    if (imageType == (int)ImageType.ScreenShot)
                    {
                        string finalPath = Path.Combine(path, screenshot);
                        if (Directory.Exists(finalPath))
                        {
                            imagePaths = Directory.GetFiles(finalPath).Select(d => new UserImageVM() { Path = Path.GetRelativePath(finalPath, d) }).ToList();
                            return new JsonResult(new ApiResponse<List<UserImageVM>>() { Data = imagePaths, Message = ResponseMessages.Success, Result = true, StatusCode = ResponseStatusCode.Success });

                        }
                        return new JsonResult(new ApiResponse<string>
                        {
                            Message = ResponseMessages.BadRequest,
                            Result = false,
                            StatusCode = ResponseStatusCode.BadRequest
                        });
                    }

                    else if (imageType == (int)ImageType.FaceCam)
                    {
                        string finalPath = Path.Combine(path, faceCam);
                        if (Directory.Exists(finalPath))
                        {
                            imagePaths = Directory.GetFiles(finalPath).Select(d => new UserImageVM() { Path = Path.GetRelativePath(finalPath, d) }).ToList();
                            return new JsonResult(new ApiResponse<List<UserImageVM>>() { Data = imagePaths, Message = ResponseMessages.Success, Result = true, StatusCode = ResponseStatusCode.Success });
                        }
                        return new JsonResult(new ApiResponse<string>
                        {
                            Message = ResponseMessages.BadRequest,
                            Result = false,
                            StatusCode = ResponseStatusCode.BadRequest
                        });
                    }

                    else
                    {
                        return new JsonResult(new ApiResponse<string>
                        {
                            Message = ResponseMessages.BadRequest,
                            Result = false,
                            StatusCode = ResponseStatusCode.BadRequest
                        });
                    }

                }
                return new JsonResult(new ApiResponse<string>
                {
                    Message = string.Format(ResponseMessages.NotFound, ModuleNames.Reports),
                    Result = false,
                    StatusCode = ResponseStatusCode.NotFound
                });
            }

            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in ReportsRepository.GetScreenShots \n MESSAGE : {ex.Message} \n INNER EXCEPTION : {ex.InnerException} \n");
                return new JsonResult(new ApiResponse<string>
                {
                    Message = ResponseMessages.InternalError,
                    Result = false,
                    StatusCode = ResponseStatusCode.InternalServerError
                });
            }
        }

        public async Task<JsonResult> GetTests()
        {
            try
            {
                List<int> testDirectories = new List<int>();
                List<ScreenShotFolderVM> screenShotFolderVM = new List<ScreenShotFolderVM>();
                string parentDirectory = Path.Combine(root, parent);
                if (Directory.Exists(parentDirectory))
                {
                    testDirectories = Directory.GetDirectories(parentDirectory).Select(d => Int32.TryParse(Path.GetRelativePath(parentDirectory, d), out int id) ? id : 0).ToList();
                    screenShotFolderVM = _context.Tests.Where(t => testDirectories.Contains(t.Id)).Select(t => new ScreenShotFolderVM() { Id = t.Id, Name = t.Name }).OrderBy(t => t.Name).ToList();
                    return new JsonResult(new ApiResponse<List<ScreenShotFolderVM>>() { Data = screenShotFolderVM, Message = ResponseMessages.Success, Result = true, StatusCode = ResponseStatusCode.Success });
                }
                return new JsonResult(new ApiResponse<string>
                {
                    Message = string.Format(ResponseMessages.NotFound, ModuleNames.Directory),
                    Result = false,
                    StatusCode = ResponseStatusCode.NotFound
                });
            }

            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in ReportsRepository.GetTests \n MESSAGE : {ex.Message} \n INNER EXCEPTION : {ex.InnerException} \n");
                return new JsonResult(new ApiResponse<string>
                {
                    Message = ResponseMessages.InternalError,
                    Result = false,
                    StatusCode = ResponseStatusCode.InternalServerError
                });
            }
        }

        public async Task<JsonResult> GetUsers(int testId)
        {
            try
            {
                List<int> userFolders = new List<int>();
                List<ScreenShotFolderVM> screenShotFolderVM = new List<ScreenShotFolderVM>();
                string parentDirectory = Path.Combine(root, parent, testId.ToString());
                if (Directory.Exists(parentDirectory))
                {
                    userFolders = Directory.GetDirectories(parentDirectory).Select(d => Int32.TryParse(Path.GetRelativePath(parentDirectory, d), out int id) ? id : 0).ToList();
                    screenShotFolderVM = _context.Users.Where(t => userFolders.Contains(t.Id)).Select(t => new ScreenShotFolderVM() { Id = t.Id, Name = t.FirstName + " " + t.LastName }).OrderBy(t => t.Name).ToList();
                    return new JsonResult(new ApiResponse<List<ScreenShotFolderVM>>() { Data = screenShotFolderVM, Message = ResponseMessages.Success, Result = true, StatusCode = ResponseStatusCode.Success });
                }
                return new JsonResult(new ApiResponse<string>
                {
                    Message = string.Format(ResponseMessages.NotFound, ModuleNames.Directory),
                    Result = false,
                    StatusCode = ResponseStatusCode.NotFound
                });
            }

            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in ReportsRepository.GetUsers \n MESSAGE : {ex.Message} \n INNER EXCEPTION : {ex.InnerException} \n");
                return new JsonResult(new ApiResponse<string>
                {
                    Message = ResponseMessages.InternalError,
                    Result = false,
                    StatusCode = ResponseStatusCode.InternalServerError
                });
            }
        }

        public async Task<JsonResult> GetUserDirectories(int userId, int testId)
        {
            try
            {
                List<UserDirectoryVM> userDirectories = new List<UserDirectoryVM>();
                List<ScreenShotFolderVM> screenShotFolderVM = new List<ScreenShotFolderVM>();
                string parentDirectory = Path.Combine(root, parent, testId.ToString(), userId.ToString());
                if (Directory.Exists(parentDirectory))
                {
                    userDirectories = Directory.GetDirectories(parentDirectory).Select((d) =>
                    {
                        string name = Path.GetRelativePath(parentDirectory, d);
                        int id = Path.GetRelativePath(parentDirectory, d).ToLower() == screenshot.ToLower() ? (int)ImageType.ScreenShot : (int)ImageType.FaceCam;
                        return new UserDirectoryVM() { Name = name, Id = id };
                    }).OrderBy(d => d.Name).ToList();

                    return new JsonResult(new ApiResponse<List<UserDirectoryVM>>() { Data = userDirectories, Message = ResponseMessages.Success, Result = true, StatusCode = ResponseStatusCode.Success });
                }
                return new JsonResult(new ApiResponse<string>
                {
                    Message = string.Format(ResponseMessages.NotFound, ModuleNames.Directory),
                    Result = false,
                    StatusCode = ResponseStatusCode.NotFound
                });
            }

            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in ReportsRepository.GetUserDirectories \n MESSAGE : {ex.Message} \n INNER EXCEPTION : {ex.InnerException} \n");
                return new JsonResult(new ApiResponse<string>
                {
                    Message = ResponseMessages.InternalError,
                    Result = false,
                    StatusCode = ResponseStatusCode.InternalServerError
                });
            }
        }

        public async Task<JsonResult> DeleteDirectory(DeleteScreenShotsVM deleteScreenShotsVM)
        {
            try
            {
                if (!ValidateDeleteModel(deleteScreenShotsVM))
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = ResponseMessages.BadRequest,
                        Result = false,
                        StatusCode = ResponseStatusCode.BadRequest
                    });
                }
                string path = Path.Combine(root, parent);
                string folderName;
                switch (deleteScreenShotsVM.Level)
                {
                    case (int)DirectoryLevel.Test:
                        path = Path.Combine(path, deleteScreenShotsVM.TestId.ToString());
                        break;
                    case (int)DirectoryLevel.User:
                        path = Path.Combine(path, deleteScreenShotsVM.TestId.ToString(), deleteScreenShotsVM.UserId.ToString());
                        break;
                    case (int)DirectoryLevel.Folder:
                        folderName = deleteScreenShotsVM.Folder == (int)ImageType.ScreenShot ? screenshot : faceCam;
                        path = Path.Combine(path, deleteScreenShotsVM.TestId.ToString(), deleteScreenShotsVM.UserId.ToString(), folderName);
                        break;
                    case (int)DirectoryLevel.Image:
                        folderName = deleteScreenShotsVM.Folder == (int)ImageType.ScreenShot ? screenshot : faceCam;
                        path = Path.Combine(path, deleteScreenShotsVM.TestId.ToString(), deleteScreenShotsVM.UserId.ToString(), folderName, deleteScreenShotsVM.FileName);
                        break;
                }

                if (deleteScreenShotsVM.Level == (int)DirectoryLevel.Image)
                {
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                        return await GetScreenShots((int)deleteScreenShotsVM.UserId, deleteScreenShotsVM.TestId, (int)deleteScreenShotsVM.Folder);
                    }
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.NotFound, ModuleNames.Directory),
                        Result = false,
                        StatusCode = ResponseStatusCode.NotFound
                    });
                }
                else
                {
                    if (Directory.Exists(path))
                    {
                        Directory.Delete(path, true);
                        switch (deleteScreenShotsVM.Level)
                        {
                            case (int)DirectoryLevel.Test:
                                return await GetTests();
                            case (int)DirectoryLevel.User:
                                return await GetUsers(deleteScreenShotsVM.TestId);
                            case (int)DirectoryLevel.Folder:
                                return await GetUserDirectories((int)deleteScreenShotsVM.UserId, deleteScreenShotsVM.TestId);
                            default:
                                return new JsonResult(new ApiResponse<string>
                                {
                                    Message = string.Format(ResponseMessages.NotFound, ModuleNames.Directory),
                                    Result = false,
                                    StatusCode = ResponseStatusCode.NotFound
                                });
                        }
                    }
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.NotFound, ModuleNames.Directory),
                        Result = false,
                        StatusCode = ResponseStatusCode.NotFound
                    });

                }
            }

            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in ReportsRepository.DeleteDirectory \n MESSAGE : {ex.Message} \n INNER EXCEPTION : {ex.InnerException} \n");
                return new JsonResult(new ApiResponse<string>
                {
                    Message = ResponseMessages.InternalError,
                    Result = false,
                    StatusCode = ResponseStatusCode.InternalServerError
                });
            }
        }
        #endregion

        #region Helper Methods
        private bool ValidateDeleteModel(DeleteScreenShotsVM deleteScreenShotsVM)
        {
            if (deleteScreenShotsVM.TestId < 1)
            {
                return false;
            }
            else if (deleteScreenShotsVM.Level == (int)DirectoryLevel.User && deleteScreenShotsVM.UserId == null)
            {
                return false;
            }
            else if (deleteScreenShotsVM.Level == (int)DirectoryLevel.Folder && (deleteScreenShotsVM.UserId == null || deleteScreenShotsVM.Folder == null))
            {
                return false;
            }
            else if (deleteScreenShotsVM.Level == (int)DirectoryLevel.Image && (deleteScreenShotsVM.UserId == null || deleteScreenShotsVM.Folder == null || deleteScreenShotsVM.FileName == null || deleteScreenShotsVM.FileName == ""))
            {
                return false;
            }
            else
            {
                return true;
            }

        }
        #endregion
    }
}
