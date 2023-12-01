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

        public async Task<JsonResult> GetAllAdmin(string? searchQuery, bool? Status, string? sortField, string? sortOrder, int? currentPageIndex, int? pageSize)
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
                            PageSize = pageSize,
                            SortField = sortField,
                            sortOrder = sortOrder,
                        };
                        List<AdminDataVM> data = connection.Query<AdminDataVM>("Select * from getAllAdmins(@SearchQuery,@Status,@PageNumber,@PageSize,@SortField,@SortOrder)", parameter).ToList();
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
                            PageSize = pageSize,
                            SortField = sortField,
                            sortOrder = sortOrder,
                        };
                        List<AdminDataVM> data = connection.Query<AdminDataVM>("Select * from getAllAdmins(@SearchQuery,@Status,@PageNumber,@PageSize,@SortField,@SortOrder)", parameter).ToList();
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
                    Admin? admin = _appDbContext.Admins.Where(ad => ad.Id == id && ad.IsSuperAdmin == false).FirstOrDefault();
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
                        FirstName = admin.FirstName.Trim(),
                        LastName = admin.LastName.Trim(),
                        FatherName = admin.MiddleName.Trim(),
                        Email = admin.Email,
                        Password = pass,
                        Status = admin.Status,
                        PhoneNumber = admin.PhoneNumber,
                        CreatedBy = admin.CreatedBy,
                    };

                    Admin? adminAlreadyExists = _appDbContext.Admins.Where(ad => ad.Email == admin.Email).FirstOrDefault();
                    if (adminAlreadyExists == null)
                    {
                        bool isEmailSent;
                        _appDbContext.Add(adminToBeAdded);
                        int count = _appDbContext.SaveChanges();
                        if (count == 1)
                        {
                            isEmailSent = SendMailForPassword(admin.FirstName, pass, admin.Email);
                            if (isEmailSent)
                            {
                                return new JsonResult(new ApiResponse<string>
                                {
                                    Message = string.Format(ResponseMessages.AddSuccess, ModuleNames.Admin),
                                    Result = true,
                                    StatusCode = ResponseStatusCode.OK
                                });
                            }
                            else
                            {
                                return new JsonResult(new ApiResponse<List<UserViewModel>>
                                {
                                    Message = ResponseMessages.InternalError,
                                    Result = false,
                                    StatusCode = ResponseStatusCode.InternalServerError
                                });
                            }
                        }


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
                return new JsonResult(new ApiResponse<string>
                {
                    Message = ResponseMessages.BadRequest,
                    Result = false,
                    StatusCode = ResponseStatusCode.BadRequest
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

        public async Task<JsonResult> Update(CreateAdminVM admin)
        {
            try
            {

                if (admin != null)
                {
                    Admin? adminAlreadyExists = _appDbContext.Admins.Where(ad => ad.Email == admin.Email && ad.Id != admin.Id).FirstOrDefault();

                    if (adminAlreadyExists == null)
                    {
                        Admin? adminToBeUpdated = _appDbContext.Admins.Where(ad => ad.Id == admin.Id && ad.IsSuperAdmin == false).FirstOrDefault();
                        if (adminToBeUpdated != null &&
                           adminToBeUpdated.FirstName.Equals(admin.FirstName.Trim()) &&
                           adminToBeUpdated.LastName.Equals(admin.LastName.Trim()) &&
                           adminToBeUpdated.FatherName.Equals(admin.MiddleName.Trim()) &&
                           adminToBeUpdated.Status.Equals(admin.Status) &&
                           adminToBeUpdated.Email.Equals(admin.Email) &&
                           adminToBeUpdated.PhoneNumber == admin.PhoneNumber
                                )
                        {
                            return new JsonResult(new ApiResponse<string>
                            {
                                Message = string.Format(ResponseMessages.NoChanges, ModuleNames.Admin),
                                Result = true,
                                StatusCode = ResponseStatusCode.Success
                            });
                        }
                        if (adminToBeUpdated != null)
                        {
                            adminToBeUpdated.FirstName = admin.FirstName.Trim();
                            adminToBeUpdated.LastName = admin.LastName.Trim();
                            adminToBeUpdated.FatherName = admin.MiddleName.Trim();
                            adminToBeUpdated.Status = admin.Status;
                            adminToBeUpdated.Email = admin.Email;
                            adminToBeUpdated.PhoneNumber = admin.PhoneNumber;
                            adminToBeUpdated.UpdatedBy = admin.CreatedBy;
                            adminToBeUpdated.UpdatedDate = DateTime.UtcNow;

                            _appDbContext.Update(adminToBeUpdated);
                            int count = _appDbContext.SaveChanges();

                            if (count == 1)
                            {
                                return new JsonResult(new ApiResponse<string>
                                {
                                    Message = string.Format(ResponseMessages.UpdateSuccess, ModuleNames.Admin),
                                    Result = true,
                                    StatusCode = ResponseStatusCode.Success
                                });
                            }
                            else
                            {
                                return new JsonResult(new ApiResponse<List<UserViewModel>>
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
                                Message = string.Format(ResponseMessages.NotFound, ModuleNames.Admin),
                                Result = false,
                                StatusCode = ResponseStatusCode.NotFound
                            });
                        }


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

        public async Task<JsonResult> ActiveInActiveAdmin(AdminStatusVM adminStatusVM)
        {
            try
            {
                if (adminStatusVM != null)
                {
                    Admin? adminStatusToBeUpdated = _appDbContext.Admins.Where(ad => ad.Id == adminStatusVM.Id && ad.IsSuperAdmin == false).FirstOrDefault();
                    if (adminStatusToBeUpdated != null)
                    {
                        adminStatusToBeUpdated.Status = adminStatusVM.status;
                        _appDbContext.Update(adminStatusToBeUpdated);
                        int count = _appDbContext.SaveChanges();

                        if (count == 1)
                        {
                            return new JsonResult(new ApiResponse<string>
                            {
                                Message = string.Format(ResponseMessages.UpdateSuccess, ModuleNames.Admin),
                                Result = true,
                                StatusCode = ResponseStatusCode.Success
                            });
                        }
                        else
                        {
                            return new JsonResult(new ApiResponse<List<UserViewModel>>
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
                            Message = string.Format(ResponseMessages.NotFound, ModuleNames.Admin),
                            Result = false,
                            StatusCode = ResponseStatusCode.NotFound
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

        public async Task<JsonResult> Delete(int id)
        {
            try
            {
                if (id != 0)
                {
                    Admin? adminToBeDeleted = _appDbContext.Admins.Where(ad => ad.Id == id && ad.IsSuperAdmin == false).FirstOrDefault();
                    if (adminToBeDeleted != null)
                    {
                        adminToBeDeleted.IsDeleted = true;
                        _appDbContext.Update(adminToBeDeleted);
                        int count = _appDbContext.SaveChanges();

                        if (count == 1)
                        {
                            return new JsonResult(new ApiResponse<string>
                            {
                                Message = string.Format(ResponseMessages.DeleteSuccess, ModuleNames.Admin),
                                Result = true,
                                StatusCode = ResponseStatusCode.Success
                            });
                        }
                        else
                        {
                            return new JsonResult(new ApiResponse<List<UserViewModel>>
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
                            Message = string.Format(ResponseMessages.NotFound, ModuleNames.Admin),
                            Result = false,
                            StatusCode = ResponseStatusCode.NotFound
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


        #region helpingMethods

        #region SendEmailForPassword
        private bool SendMailForPassword(string firstName, string password, string email)
        {
            try
            {
                var subject = "Password reset request";
                var body = $"<h3>Hello {firstName}, </h3><br />We have received admin registration request for you.<br /><br />Here is your credentials to login!!<br /><br /><h2>User name: {email}</h2><br /><h2>Password: {password}</h2><br/>You can login using the following link:<br/><a href=http://aptitudetest-frontend.web2.anasource.com/admin/login>http://aptitudetest-frontend.web2.anasource.com/admin/login</a><br/><br/>Regards<br/>Tatvasoft";
                var emailHelper = new EmailHelper(_config);
                var isEmailSent = emailHelper.SendEmail(email, subject, body);
                return isEmailSent;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #endregion

        #endregion
    }
}
