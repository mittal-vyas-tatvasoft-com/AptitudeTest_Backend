using AptitudeTest.Common.Data;
using AptitudeTest.Common.Helpers;
using AptitudeTest.Core.Entities.Admin;
using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using AptitudeTest.Data.Common;
using APTITUDETEST.Common.Data;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace AptitudeTest.Data.Data
{
    public class AdminRepository : IAdminRepository
    {

        #region Properies
        private readonly AppDbContext _appDbContext;
        private readonly DapperAppDbContext _dapperContext;
        private readonly IConfiguration _config;
        private readonly string connectionString;
        #endregion

        #region Constructor
        public AdminRepository(AppDbContext appDbContext, IConfiguration config, DapperAppDbContext dapperContext)
        {
            _appDbContext = appDbContext;
            _dapperContext = dapperContext;
            _config = config;
            connectionString = _config["ConnectionStrings:AptitudeTest"];
        }
        #endregion

        #region methods

        public async Task<JsonResult> GetAllAdmin(string? searchQuery, bool? Status, int? currentPageIndex, int? pageSize)
        {
            try
            {
                if (!string.IsNullOrEmpty(searchQuery))
                {
                    using (var connection = new NpgsqlConnection(connectionString))
                    {
                        connection.Open();
                        var parameter = new
                        {
                            SearchQuery = searchQuery,
                            Status = Status,
                            PageNumber = currentPageIndex,
                            PageSize = pageSize
                        };
                        List<AdminDataVM> data = connection.Query<AdminDataVM>("Select * from getAllAdmins(@SearchQuery,@Status,@PageNumber,@PageSize)", parameter).ToList();
                        connection.Close();
                        return new JsonResult(new ApiResponse<List<AdminDataVM>>
                        {
                            Data = data,
                            Message = ResponseMessages.Success,
                            Result = true,
                            StatusCode = ResponseStatusCode.Success
                        });
                    }
                }
                else
                {
                    using (var connection = new NpgsqlConnection(connectionString))
                    {
                        connection.Open();
                        var parameter = new
                        {
                            SearchQuery = string.Empty,
                            Status = Status,
                            PageNumber = currentPageIndex,
                            PageSize = pageSize
                        };
                        List<AdminDataVM> data = connection.Query<AdminDataVM>("Select * from getAllAdmins(@SearchQuery,@Status,@PageNumber,@PageSize)", parameter).ToList();
                        connection.Close();
                        return new JsonResult(new ApiResponse<List<AdminDataVM>>
                        {
                            Data = data,
                            Message = ResponseMessages.Success,
                            Result = true,
                            StatusCode = ResponseStatusCode.Success
                        });
                    }


                }
            }
            catch
            {
                return new JsonResult(new ApiResponse<List<UserViewModel>>
                {
                    Message = ResponseMessages.InternalError,
                    Result = false,
                    StatusCode = ResponseStatusCode.InternalServerError
                });
            }
        }

        public async Task<JsonResult> GetAdminById(int? id)
        {
            try
            {
                if (id != 0)
                {
                    Admin? admin = _appDbContext.Admins.Where(ad => ad.Id == id).FirstOrDefault();
                    if (admin != null)
                    {
                        return new JsonResult(new ApiResponse<Admin>
                        {
                            Data = admin,
                            Message = ResponseMessages.Success,
                            Result = true,
                            StatusCode = ResponseStatusCode.Success
                        });
                    }
                    else
                    {
                        return new JsonResult(new ApiResponse<Admin>
                        {
                            Message = string.Format(ResponseMessages.NotFound, ModuleNames.Admin),
                            Result = true,
                            StatusCode = ResponseStatusCode.Success
                        });
                    }

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
            catch
            {
                return new JsonResult(new ApiResponse<List<UserViewModel>>
                {
                    Message = ResponseMessages.InternalError,
                    Result = false,
                    StatusCode = ResponseStatusCode.InternalServerError
                });
            }
        }

        public async Task<JsonResult> Create(CreateAdminVM admin)
        {
            try
            {
                if (admin != null)
                {
                    var pass = RandomPasswordGenerator.GenerateRandomPassword(8);

                    Admin adminToBeAdded = new Admin()
                    {
                        FirstName = admin.FirstName,
                        LastName = admin.LastName,
                        FatherName = admin.FatherName,
                        Email = admin.Email,
                        Password = pass,
                        Status = admin.Status,
                        PhoneNumber = admin.PhoneNumber,
                        CreatedBy = admin.CreatedBy,
                    };

                    Admin? adminAlreadyExists = _appDbContext.Admins.Where(ad => ad.Email == admin.Email).FirstOrDefault();
                    if (adminAlreadyExists == null)
                    {
                        _appDbContext.Add(adminToBeAdded);
                        _appDbContext.SaveChanges();
                        return new JsonResult(new ApiResponse<string>
                        {
                            Message = string.Format(ResponseMessages.AddSuccess, ModuleNames.Admin),
                            Result = true,
                            StatusCode = ResponseStatusCode.OK
                        });
                    }
                    else
                    {
                        return new JsonResult(new ApiResponse<string>
                        {
                            Message = string.Format(ResponseMessages.AlreadyExists, ModuleNames.Admin),
                            Result = false,
                            StatusCode = ResponseStatusCode.AlreadyExist
                        });
                    }

                }
                return new JsonResult(new ApiResponse<List<UserViewModel>>
                {
                    Message = ResponseMessages.Success,
                    Result = true,
                    StatusCode = ResponseStatusCode.OK
                });
            }
            catch
            {
                return new JsonResult(new ApiResponse<List<UserViewModel>>
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
