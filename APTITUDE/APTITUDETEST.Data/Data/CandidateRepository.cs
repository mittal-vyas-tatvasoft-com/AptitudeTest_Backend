using AptitudeTest.Core.Entities.CandidateSide;
using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using AptitudeTest.Data.Common;
using APTITUDETEST.Common.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace AptitudeTest.Data.Data
{
    public class CandidateRepository : ICandidateRepository
    {
        #region Properies
        private readonly AppDbContext _appDbContext;
        private readonly IConfiguration _config;
        private readonly string connectionString;
        #endregion

        #region Constructor
        public CandidateRepository(AppDbContext appDbContext, IConfiguration config)
        {
            _appDbContext = appDbContext;
            _config = config;
            connectionString = _config["ConnectionStrings:AptitudeTest"];
        }
        #endregion

        #region Method
        public async Task<JsonResult> CreateUserTest(CreateUserTestVM userTest)
        {
            try
            {
                if (userTest != null)
                {
                    UserTest userTestToBeAdded = new UserTest()
                    {
                        UserId = userTest.UserId,
                        TestId = userTest.TestId,
                        Status = userTest.Status,
                        IsFinished = userTest.IsFinished,
                        CreatedBy = userTest.CreatedBy,
                    };

                    UserTest? userTestAlreadyExists = _appDbContext.UserTests.Where(x => x.UserId == userTest.UserId && x.TestId == userTest.TestId && x.IsDeleted == false).FirstOrDefault();
                    if (userTestAlreadyExists == null)
                    {
                        _appDbContext.Add(userTestToBeAdded);
                        int count = _appDbContext.SaveChanges();
                        if (count == 1)
                        {
                            return new JsonResult(new ApiResponse<string>
                            {
                                Message = string.Format(ResponseMessages.AddSuccess, ModuleNames.UserTest),
                                Result = true,
                                StatusCode = ResponseStatusCode.OK
                            });
                        }
                        else
                        {
                            return new JsonResult(new ApiResponse<string>
                            {
                                Message = ResponseMessages.InternalError,
                                Result = false,
                                StatusCode = ResponseStatusCode.InternalServerError
                            });
                        }
                    }
                    else
                    {
                        return new JsonResult(new ApiResponse<string>
                        {
                            Message = string.Format(ResponseMessages.AlreadyExists, ModuleNames.UserTest),
                            Result = false,
                            StatusCode = ResponseStatusCode.AlreadyExist
                        });
                    }

                }
                return new JsonResult(new ApiResponse<string>
                {
                    Message = ResponseMessages.BadRequest,
                    Result = false,
                    StatusCode = ResponseStatusCode.BadRequest
                });
            }
            catch
            {
                return new JsonResult(new ApiResponse<string>
                {
                    Message = ResponseMessages.InternalError,
                    Result = false,
                    StatusCode = ResponseStatusCode.InternalServerError
                });
            }

        }
        public async Task<JsonResult> CreateTempUserTest(CreateTempUserTestVM tempUserTest)
        {
            try
            {
                if (tempUserTest != null)
                {
                    TempUserTest tempUserTestToBeAdded = new TempUserTest()
                    {
                        UserId = tempUserTest.UserId,
                        TestId = tempUserTest.TestId,
                        Status = tempUserTest.Status,
                        TimeRemaining = tempUserTest.TimeRemaining,
                        IsAdminApproved = tempUserTest.IsAdminApproved,
                        IsFinished = tempUserTest.IsFinished,
                        CreatedBy = tempUserTest.CreatedBy,
                    };

                    TempUserTest? tempUserTestAlreadyExists = _appDbContext.TempUserTests.Where(x => x.UserId == tempUserTest.UserId && x.TestId == tempUserTest.TestId && x.IsDeleted == false).FirstOrDefault();
                    if (tempUserTestAlreadyExists == null)
                    {
                        _appDbContext.Add(tempUserTestToBeAdded);
                        int count = _appDbContext.SaveChanges();
                        if (count == 1)
                        {
                            return new JsonResult(new ApiResponse<string>
                            {
                                Message = string.Format(ResponseMessages.AddSuccess, ModuleNames.TempUserTest),
                                Result = true,
                                StatusCode = ResponseStatusCode.OK
                            });
                        }
                        else
                        {
                            return new JsonResult(new ApiResponse<string>
                            {
                                Message = ResponseMessages.InternalError,
                                Result = false,
                                StatusCode = ResponseStatusCode.InternalServerError
                            });
                        }
                    }
                    else
                    {
                        return new JsonResult(new ApiResponse<string>
                        {
                            Message = string.Format(ResponseMessages.AlreadyExists, ModuleNames.TempUserTest),
                            Result = false,
                            StatusCode = ResponseStatusCode.AlreadyExist
                        });
                    }

                }
                return new JsonResult(new ApiResponse<string>
                {
                    Message = ResponseMessages.BadRequest,
                    Result = false,
                    StatusCode = ResponseStatusCode.BadRequest
                });
            }
            catch
            {
                return new JsonResult(new ApiResponse<string>
                {
                    Message = ResponseMessages.InternalError,
                    Result = false,
                    StatusCode = ResponseStatusCode.InternalServerError
                });
            }
        }
        #endregion
    }
}
