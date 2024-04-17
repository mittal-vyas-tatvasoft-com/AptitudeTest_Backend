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
        private readonly IConfiguration _config;
        private readonly string? connectionString;
        private readonly string? adminLoginUrl;
        private readonly ILoggerManager _logger;
        #endregion

        #region Constructor
        public AdminRepository(AppDbContext appDbContext, IConfiguration config, ILoggerManager logger)
        {
            _appDbContext = appDbContext;
            _config = config;
            connectionString = _config["ConnectionStrings:AptitudeTest"];
            adminLoginUrl = _config["EmailGeneration:AdminUrlForBody"];
            _logger = logger;
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
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in AdminRepository.GetAllAdmin \n MESSAGE : {ex.Message} \n INNER EXCEPTION : {ex.InnerException} \n");
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
            catch (Exception ex)
            {
                _logger.LogError($"AdminRepository.GetAdminById \n MESSAGE : {ex.Message} \n INNER EXCEPTION : {ex.InnerException} \n");
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
                    //var pass = RandomPasswordGenerator.GenerateRandomPassword(8);
                    var contactNumber = admin.PhoneNumber.ToString();
                    var password = admin.FirstName.ToUpper() + contactNumber.Substring(contactNumber.Length - 4);

                    Admin adminToBeAdded = new Admin()
                    {
                        FirstName = admin.FirstName.Trim(),
                        LastName = admin.LastName.Trim(),
                        FatherName = admin.MiddleName.Trim(),
                        Email = admin.Email,
                        Password = password,
                        Status = admin.Status,
                        PhoneNumber = admin.PhoneNumber,
                        CreatedBy = admin.CreatedBy,
                    };

                    Admin? adminAlreadyExists = _appDbContext.Admins.Where(ad => (ad.Email.Trim() == admin.Email.Trim() || ad.PhoneNumber == admin.PhoneNumber) && ad.IsDeleted == false).FirstOrDefault();
                    if (adminAlreadyExists == null)
                    {
                        //bool isEmailSent;
                        _appDbContext.Add(adminToBeAdded);
                        int count = _appDbContext.SaveChanges();
                        if (count == 1)
                        {
                            return new JsonResult(new ApiResponse<string>
                            {
                                Message = string.Format(ResponseMessages.AddSuccess, ModuleNames.Admin),
                                Result = true,
                                StatusCode = ResponseStatusCode.OK
                            });
                            //isEmailSent = SendMailForPassword(admin.FirstName, admin.LastName, pass, admin.Email);
                            //if (isEmailSent)
                            //{
                            //    return new JsonResult(new ApiResponse<string>
                            //    {
                            //        Message = string.Format(ResponseMessages.AddSuccess, ModuleNames.Admin),
                            //        Result = true,
                            //        StatusCode = ResponseStatusCode.OK
                            //    });
                            //}
                            //else
                            //{
                            //    _logger.LogError($"Error occurred in AdminRepository.Create while adding admin \n");
                            //    return new JsonResult(new ApiResponse<List<UserViewModel>>
                            //    {
                            //        Message = ResponseMessages.InternalError,
                            //        Result = false,
                            //        StatusCode = ResponseStatusCode.InternalServerError
                            //    });
                            //}
                        }
                        else
                        {
                            _logger.LogError($"Error occurred in AdminRepository.Create while adding admin \n");
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
                            Message = ModuleNames.EmailNumberAlreadyExists,
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
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in AdminRepository.Create \n MESSAGE : {ex.Message} \n INNER EXCEPTION : {ex.InnerException} \n");
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
                    Admin? adminAlreadyExists = _appDbContext.Admins.Where(ad => (ad.Email == admin.Email || ad.PhoneNumber == admin.PhoneNumber) && ad.Id != admin.Id).FirstOrDefault();

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
                                _logger.LogError($"Error occurred in AdminRepository.Update for Id : {admin.Id}\n");
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
                            Message = ModuleNames.EmailNumberAlreadyExists,
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
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred for id : {admin.Id} in AdminRepository.Update \n MESSAGE : {ex.Message} \n INNER EXCEPTION : {ex.InnerException} \n");
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
                            _logger.LogError($"Error occurred in AdminRepository.ActiveInActiveAdmin \n");
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
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in AdminRepository.ActiveInActiveAdmin \n MESSAGE : {ex.Message} \n INNER EXCEPTION : {ex.InnerException} \n");
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
                            _logger.LogError($"Error occurred in AdminRepository.Delete for Id: {id} \n");
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
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred for id : {id} in AdminRepository.Delete \n MESSAGE : {ex.Message} \n INNER EXCEPTION : {ex.InnerException} \n");
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
        private bool SendMailForPassword(string firstName, string lastName, string password, string email)
        {
            try
            {
                var subject = "Admin registration succesful for Tatvasoft - Aptitude Test portal";
                var body = $"<h3>Welcome {firstName} {lastName}, </h3>We have received admin registration request for you.<br />Here are your credentials to login!!<br /><h4>User name: {email}</h4><h4>Password: {password}</h4><You can login using the following link:<br/><a href={adminLoginUrl}>{adminLoginUrl}</a><br/><br/>Regards<br/>Tatvasoft";
                var emailHelper = new EmailHelper(_config, _appDbContext);
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
