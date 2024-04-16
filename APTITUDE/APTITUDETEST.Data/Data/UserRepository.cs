using AptitudeTest.Common.Data;
using AptitudeTest.Common.Helpers;
using AptitudeTest.Core.Entities.Master;
using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using AptitudeTest.Data.Common;
using APTITUDETEST.Common.Data;
using APTITUDETEST.Core.Entities.Users;
using CsvHelper;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using NpgsqlTypes;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;
using static AptitudeTest.Data.Common.Enums;

namespace AptitudeTest.Data.Data
{
    public class UserRepository : IUsersRepository
    {

        #region Properies
        private readonly AppDbContext _appDbContext;
        private readonly DapperAppDbContext _dapperContext;
        private readonly IConfiguration _config;
        private readonly string? connectionString;
        private readonly string? userLoginUrl;
        private readonly ILoggerManager _logger;

        #endregion

        #region Constructor
        public UserRepository(AppDbContext appDbContext, IConfiguration config, DapperAppDbContext dapperContext, ILoggerManager logger)
        {
            _appDbContext = appDbContext;
            _dapperContext = dapperContext;
            _config = config;
            connectionString = _config["ConnectionStrings:AptitudeTest"];
            userLoginUrl = _config["EmailGeneration:UserUrlForBody"];
            _logger = logger;
        }
        #endregion

        #region methods

        #region GetAllUsers
        public async Task<JsonResult> GetAllUsers(string? searchQuery, int? CollegeId, int? GroupId, bool? Status, int? Year, string? sortField, string? sortOrder, int? currentPageIndex, int? pageSize)
        {
            try
            {
                if (!string.IsNullOrEmpty(searchQuery))
                {
                    using (var connection = new NpgsqlConnection(connectionString))
                    {
                        connection.Open();
                        List<UserViewModel> data = connection.Query<UserViewModel>("Select * from getallUsers(@SearchQuery,@CollegeId,@GroupId,@Status,@YearFilter,@PageNumber,@PageSize,@SortField,@SortOrder)", new { SearchQuery = searchQuery, CollegeId = (object)CollegeId, GroupId = (object)GroupId, Status = Status, YearFilter = Year, SortField = sortField, SortOrder = sortOrder, PageNumber = currentPageIndex, PageSize = pageSize }).ToList();
                        connection.Close();
                        return new JsonResult(new ApiResponse<List<UserViewModel>>
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
                        List<UserViewModel> data = connection.Query<UserViewModel>("Select * from getallUsers(@SearchQuery,@CollegeId,@GroupId,@Status,@YearFilter,@PageNumber,@PageSize,@SortField,@SortOrder)", new { SearchQuery = "", CollegeId = (object)CollegeId, GroupId = (object)GroupId, Status = Status, YearFilter = Year, PageNumber = currentPageIndex, PageSize = pageSize, SortField = sortField, SortOrder = sortOrder }).ToList();
                        connection.Close();
                        return new JsonResult(new ApiResponse<List<UserViewModel>>
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
                _logger.LogError($"Error occurred in UserRepository.GetAllUsers \n MESSAGE : {ex.Message} \n INNER EXCEPTION : {ex.InnerException} \n");
                return new JsonResult(new ApiResponse<List<UserViewModel>>
                {
                    Message = ResponseMessages.InternalError,
                    Result = false,
                    StatusCode = ResponseStatusCode.InternalServerError
                });
            }
        }

        #endregion

        #region GetUserById
        public async Task<JsonResult> GetUserById(int id)
        {
            try
            {
                UserDetailsVM userDetails = new UserDetailsVM();
                using (var dbConnection = new DbConnection())
                {
                    var data = dbConnection.Connection.Query("Select * from GetUserbyUserId(@user_id)", new { user_id = id }).ToList();

                    if (data.Count == 0)
                    {
                        return new JsonResult(new ApiResponse<UserDetailsVM>
                        {
                            Data = null,
                            Message = string.Format(ResponseMessages.NotFound, ModuleNames.User),
                            Result = false,
                            StatusCode = ResponseStatusCode.NotFound
                        });
                    }
                    userDetails.AcademicsDetails = new List<UserAcademicsVM>();
                    userDetails.FamilyDetails = new List<UserFamilyVM>();
                    List<int> FamilyIds = new List<int>();
                    List<int> AcadamicIds = new List<int>();
                    FillUserData(userDetails, data);
                    FillAcademicAndFamilyData(data, userDetails, AcadamicIds, FamilyIds);

                }

                return new JsonResult(new ApiResponse<UserDetailsVM>
                {
                    Data = userDetails,
                    Message = ResponseMessages.Success,
                    Result = true,
                    StatusCode = ResponseStatusCode.Success
                });
            }

            catch (Exception ex)
            {
                _logger.LogError($"UserRepository.GetUserById \n MESSAGE : {ex.Message} \n INNER EXCEPTION : {ex.InnerException} \n");
                return new JsonResult(new ApiResponse<string>
                {
                    Message = ResponseMessages.InternalError,
                    Result = false,
                    StatusCode = ResponseStatusCode.InternalServerError
                });
            }
        }
        #endregion

        #region GetAllStates
        public async Task<JsonResult> GetAllState()
        {

            try
            {
                var stateList = await Task.FromResult(_appDbContext.States
                .Where(x => x.IsDeleted == null || x.IsDeleted == false)
                .Select(x => new { Id = x.Id, Name = x.name })
                .ToList());

                if (stateList != null)
                {
                    return new JsonResult(new ApiResponse<IEnumerable<object>>
                    {
                        Data = stateList,
                        Message = ResponseMessages.Success,
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }
                else
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Data = string.Format(ResponseMessages.NotFound, ModuleNames.State),
                        Message = ResponseMessages.Success,
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in UserRepository.GetAllStates \n MESSAGE : {ex.Message} \n INNER EXCEPTION : {ex.InnerException} \n");
                return new JsonResult(new ApiResponse<string>
                {
                    Message = ResponseMessages.InternalError,
                    Result = false,
                    StatusCode = ResponseStatusCode.InternalServerError
                });
            }
        }
        #endregion

        #region Create
        public async Task<JsonResult> Create(CreateUserVM user)
        {
            var password = RandomPasswordGenerator.GenerateRandomPassword(8);
            try
            {
                if (user == null)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.BadRequest, ModuleNames.Candidate),
                        Result = false,
                        StatusCode = ResponseStatusCode.BadRequest
                    });
                }
                User? users = _appDbContext.Users.Where(t => (t.Email.Trim().ToLower() == user.Email.Trim().ToLower() || t.PhoneNumber == user.PhoneNumber) && t.IsDeleted != true).FirstOrDefault();
                if (users != null)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = ModuleNames.EmailNumberAlreadyExists,
                        Result = false,
                        StatusCode = ResponseStatusCode.AlreadyExist
                    });
                }
                using (var connection = _dapperContext.CreateConnection())
                {
                    var procedure = "insert_user";
                    var parameters = new DynamicParameters(
                        new
                        {
                            p_firstname = user.FirstName.Trim(),
                            p_lastname = user.LastName.Trim(),
                            p_fathername = user.FatherName.Trim(),
                            p_email = user.Email,
                            p_password = password,
                            p_phonenumber = user.PhoneNumber,
                            p_groupid = user.GroupId,
                            p_collegeid = user.CollegeId,
                            p_gender = user.Gender,
                            p_status = user.Status,
                            p_createdby = user.CreatedBy,
                            next_id = 0
                        });

                    var userId = connection.Query<int>(procedure, parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (userId > 0)
                    {
                        SendMailForPassword(user.FirstName, user.LastName, user.Email, password);

                        return new JsonResult(new ApiResponse<string>
                        {
                            Message = string.Format(ResponseMessages.AddSuccess, ModuleNames.Candidate),
                            Result = true,
                            StatusCode = ResponseStatusCode.Success
                        });
                    }
                    else
                    {
                        return new JsonResult(new ApiResponse<string>
                        {
                            Message = string.Format(ResponseMessages.InternalError, ModuleNames.Candidate),
                            Result = false,
                            StatusCode = ResponseStatusCode.RequestFailed
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in UserRepository.Create \n MESSAGE : {ex.Message} \n INNER EXCEPTION : {ex.InnerException} \n");
                return new JsonResult(new ApiResponse<string>
                {
                    Message = string.Format(ResponseMessages.InternalError, ModuleNames.Candidate),
                    Result = false,
                    StatusCode = ResponseStatusCode.RequestFailed
                });
            }
        }
        #endregion

        #region Update
        public async Task<JsonResult> Update(UserVM user)
        {
            try
            {
                if (user == null)
                {
                    return new JsonResult(new ApiResponse<List<string>>
                    {
                        Message = ResponseMessages.InsertProperData,
                        Result = false,
                        StatusCode = ResponseStatusCode.BadRequest
                    });
                }
                if (user.UserAcademicsVM == null)
                {
                    user.UserAcademicsVM = new List<DapperUserAcademicsVM>();
                }
                if (user.UserFamilyVM == null)
                {
                    user.UserFamilyVM = new List<DapperUserFamilyVM>();
                }
                using (var connection = _dapperContext.CreateConnection())
                {
                    var procedure = "udpate_user_detail";
                    var dateParameter = new NpgsqlParameter("p_dateofbirth", NpgsqlDbType.Date);
                    dateParameter.Value = user.DateOfBirth;
                    if (user.OtherCollege != null && user.OtherCollege.Trim() != "")
                    {
                        int collegeStatus = addOtherCollege(user);
                        if (collegeStatus == (int)CollegeStatus.Exists)
                        {
                            return new JsonResult(new ApiResponse<string>
                            {
                                Message = string.Format(ResponseMessages.AlreadyExists, ModuleNames.College),
                                Result = false,
                                StatusCode = ResponseStatusCode.AlreadyExist
                            });
                        }
                        if (collegeStatus == (int)CollegeStatus.InActive)
                        {
                            return new JsonResult(new ApiResponse<string>
                            {
                                Message = ResponseMessages.InActiveCollege,
                                Result = false,
                                StatusCode = ResponseStatusCode.AlreadyExist
                            });
                        }
                    }
                    var parameters = new DynamicParameters(
                    new
                    {
                        p_userid = user.Id,
                        p_groupid = user.GroupId,
                        p_collegeid = user.CollegeId,
                        p_status = user.Status,
                        p_firstname = user.FirstName.Trim(),
                        p_lastname = user.LastName.Trim(),
                        p_fathername = user.FatherName.Trim(),
                        p_gender = user.Gender,
                        p_dateofbirth = dateParameter.Value,
                        p_email = user.Email,
                        p_phonenumber = user.PhoneNumber,
                        p_appliedthrough = user.AppliedThrough,
                        p_technologyinterestedin = user.TechnologyInterestedIn,
                        p_preferredlocation = user.PreferredLocation,
                        p_permanentaddress1 = user.PermanentAddress1,
                        p_permanentaddress2 = user.PermanentAddress2,
                        p_pincode = user.Pincode,
                        p_city = user.City,
                        p_state = user.State,
                        p_acpcmeritrank = user.ACPCMeritRank,
                        p_gujcetscore = user.GUJCETScore,
                        p_jeescore = user.JEEScore,
                        p_updatedby = user.UpdatedBy,
                        p_isprofileedited = user.IsProfileEdited,
                        p_userfamilydata = user.UserFamilyVM.ToArray(),
                        p_useracademicsdata = user.UserAcademicsVM.ToArray()
                    });

                    connection.Query(procedure, parameters, commandType: CommandType.StoredProcedure);

                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.UpdateSuccess, ModuleNames.Candidate),
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in UserRepository.Update \n MESSAGE : {ex.Message} \n INNER EXCEPTION : {ex.InnerException} \n");
                return new JsonResult(new ApiResponse<string>
                {
                    Message = string.Format(ResponseMessages.InternalError, ModuleNames.Candidate),
                    Result = false,
                    StatusCode = ResponseStatusCode.RequestFailed
                });
            }
        }

        #endregion

        #region Register
        public async Task<JsonResult> RegisterUser(UserVM registerUserVM)
        {
            try
            {
                if (registerUserVM == null)
                {
                    return new JsonResult(new ApiResponse<int>
                    {
                        Message = string.Format(ResponseMessages.BadRequest),
                        Result = false,
                        StatusCode = ResponseStatusCode.BadRequest
                    });
                }
                User? users = await Task.FromResult(_appDbContext.Users.Where(t => (t.Email.Trim().ToLower() == registerUserVM.Email.Trim().ToLower() || t.PhoneNumber == registerUserVM.PhoneNumber) && t.IsDeleted != true).FirstOrDefault());
                if (users != null)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.AlreadyExists, ModuleNames.Candidate),
                        Result = false,
                        StatusCode = ResponseStatusCode.AlreadyExist
                    });
                }
                var password = RandomPasswordGenerator.GenerateRandomPassword(8);

                if (registerUserVM.UserAcademicsVM == null)
                {
                    registerUserVM.UserAcademicsVM = new List<DapperUserAcademicsVM>();
                }
                if (registerUserVM.UserFamilyVM == null)
                {
                    registerUserVM.UserFamilyVM = new List<DapperUserFamilyVM>();
                }
                using (var connection = _dapperContext.CreateConnection())
                {
                    int DefaultGroupId = _appDbContext.MasterGroup.Where(g => g.IsDefault == true).Select(g => g.Id).FirstOrDefault();
                    var procedure = "register_user_detail";
                    var dateParameter = new NpgsqlParameter("p_dateofbirth", NpgsqlDbType.Date);
                    dateParameter.Value = registerUserVM.DateOfBirth;
                    if (registerUserVM.OtherCollege != null && registerUserVM.OtherCollege != "")
                    {
                        int collegeStatus = addOtherCollege(registerUserVM);
                        if (collegeStatus == (int)CollegeStatus.Exists)
                        {
                            return new JsonResult(new ApiResponse<string>
                            {
                                Message = string.Format(ResponseMessages.AlreadyExists, ModuleNames.College),
                                Result = false,
                                StatusCode = ResponseStatusCode.AlreadyExist
                            });
                        }
                        if (collegeStatus == (int)CollegeStatus.InActive)
                        {
                            return new JsonResult(new ApiResponse<string>
                            {
                                Message = ResponseMessages.InActiveCollege,
                                Result = false,
                                StatusCode = ResponseStatusCode.AlreadyExist
                            });
                        }
                    }
                    var parameters = new DynamicParameters(
                    new
                    {
                        p_groupid = DefaultGroupId,
                        p_collegeid = registerUserVM.CollegeId,
                        p_status = true,
                        p_firstname = registerUserVM.FirstName,
                        p_lastname = registerUserVM.LastName,
                        p_fathername = registerUserVM.FatherName,
                        p_password = password,
                        p_gender = registerUserVM.Gender,
                        p_dateofbirth = dateParameter.Value,
                        p_email = registerUserVM.Email,
                        p_phonenumber = registerUserVM.PhoneNumber,
                        p_appliedthrough = registerUserVM.AppliedThrough,
                        p_technologyinterestedin = registerUserVM.TechnologyInterestedIn,
                        p_preferredlocation = registerUserVM.PreferredLocation,
                        p_permanentaddress1 = registerUserVM.PermanentAddress1,
                        p_permanentaddress2 = registerUserVM.PermanentAddress2,
                        p_pincode = registerUserVM.Pincode,
                        p_city = registerUserVM.City,
                        p_stateid = registerUserVM.State,
                        p_acpcmeritrank = registerUserVM.ACPCMeritRank,
                        p_gujcetscore = registerUserVM.GUJCETScore,
                        p_jeescore = registerUserVM.JEEScore,
                        p_userfamilydata = registerUserVM.UserFamilyVM.ToArray(),
                        p_useracademicsdata = registerUserVM.UserAcademicsVM.ToArray(),

                        next_id = 0
                    });
                    var userId = connection.Query<int>(procedure, parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    if (userId > 0)
                    {
                        SendMailForPassword(registerUserVM.FirstName, registerUserVM.LastName, registerUserVM.Email, password);

                        return new JsonResult(new ApiResponse<string>
                        {
                            Message = string.Format(ResponseMessages.RegisterSuccess, ModuleNames.User),
                            Result = true,
                            StatusCode = ResponseStatusCode.Success
                        });
                    }
                    else
                    {
                        return new JsonResult(new ApiResponse<string>
                        {
                            Message = string.Format(ResponseMessages.InternalError, ModuleNames.User),
                            Result = false,
                            StatusCode = ResponseStatusCode.RequestFailed
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in UserRepository.RegisterUser \n MESSAGE : {ex.Message} \n INNER EXCEPTION : {ex.InnerException} \n");
                return new JsonResult(new ApiResponse<string>
                {
                    Message = string.Format(ResponseMessages.InternalError, ModuleNames.User),
                    Result = false,
                    StatusCode = ResponseStatusCode.RequestFailed
                });
            }
        }
        #endregion

        #region InActiveUsers
        public async Task<JsonResult> ActiveInActiveUsers(UserStatusVM userStatusVM)
        {
            try
            {
                using (var connection = _dapperContext.CreateConnection())
                {
                    var procedure = "activeinactive_users";
                    var parameters = new DynamicParameters(
                    new
                    {
                        p_userids = userStatusVM.UserIds.ToArray(),
                        p_status = userStatusVM.status
                    });

                    connection.Query(procedure, parameters, commandType: CommandType.StoredProcedure);

                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.StatusUpdateSuccess, ModuleNames.Candidate),
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in UserRepository.ActiveInActiveUsers \n MESSAGE : {ex.Message} \n INNER EXCEPTION : {ex.InnerException} \n");
                return new JsonResult(new ApiResponse<string>
                {
                    Message = string.Format(ResponseMessages.InternalError, ModuleNames.Candidate),
                    Result = false,
                    StatusCode = ResponseStatusCode.RequestFailed
                });
            }
        }
        #endregion

        #region DeleteUsers
        public async Task<JsonResult> DeleteUsers(List<int> userIds)
        {
            try
            {
                using (var connection = _dapperContext.CreateConnection())
                {
                    var procedure = "delete_users";
                    var parameters = new DynamicParameters(
                    new
                    {
                        p_userids = userIds.ToArray()
                    });

                    connection.Query(procedure, parameters, commandType: CommandType.StoredProcedure);

                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.DeleteSuccess, ModuleNames.Candidate),
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in UserRepository.DeleteUsers \n MESSAGE : {ex.Message} \n INNER EXCEPTION : {ex.InnerException} \n");
                return new JsonResult(new ApiResponse<string>
                {
                    Message = string.Format(ResponseMessages.InternalError, ModuleNames.Candidate),
                    Result = false,
                    StatusCode = ResponseStatusCode.RequestFailed
                });
            }
        }
        #endregion

        #region ImportUsers
        public async Task<JsonResult> ImportUsers(ImportUserVM importUsers)
        {
            try
            {
                using (var reader = new StreamReader(importUsers.file.OpenReadStream()))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {

                    List<UserImportVM> records = new List<UserImportVM>();

                    var csvContent = new List<string[]>();
                    csvContent = GetCsvContent(reader);
                    var headers = csvContent.FirstOrDefault();
                    records = GetRecords(headers, csvContent);

                    if (records.Count <= 0 || records == null)
                    {
                        return new JsonResult(new ApiResponse<string>
                        {
                            Message = ResponseMessages.InsertSomeData,
                            Result = false,
                            StatusCode = ResponseStatusCode.BadRequest
                        });
                    }

                    ValidateImportFileVM checkData = await checkImportedData(records);

                    List<ImportCandidateVM> data = new List<ImportCandidateVM>();
                    ImportCandidateVM dataToBeAdd;

                    foreach (var item in records)
                    {
                        dataToBeAdd = new ImportCandidateVM();
                        dataToBeAdd.firstname = item.firstname;
                        dataToBeAdd.lastname = item.lastname;
                        dataToBeAdd.middlename = item.middlename;
                        dataToBeAdd.email = item.email;
                        dataToBeAdd.contactnumber = item.contactnumber;
                        if (IsStatusTrue(item.status))
                        {
                            dataToBeAdd.status = true;
                        }
                        else
                        {
                            dataToBeAdd.status = false;
                        }

                        data.Add(dataToBeAdd);
                    }


                    if (!checkData.isValidate)
                    {
                        return new JsonResult(new ApiResponse<List<string>>
                        {
                            Data = checkData.validationMessage,
                            Message = ResponseMessages.InsertProperData,
                            Result = false,
                            StatusCode = ResponseStatusCode.BadRequest
                        });
                    }

                    ImportCandidateResponseVM? result;
                    using (var connection = _dapperContext.CreateConnection())
                    {
                        var procedure = "import_users";
                        var parameters = new DynamicParameters();
                        parameters.Add("p_import_user_data", data, DbType.Object, ParameterDirection.Input);
                        parameters.Add("groupid", importUsers.GroupId, DbType.Int32, ParameterDirection.Input);
                        parameters.Add("collegeid", importUsers.CollegeId, DbType.Int32, ParameterDirection.Input);
                        parameters.Add("candidates_added_count", ParameterDirection.Output);
                        parameters.Add("inserted_emails", DbType.Object, direction: ParameterDirection.Output);

                        // Execute the stored procedure

                        result = connection.Query<ImportCandidateResponseVM>(procedure, parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                        if (DoesResultExists(result))
                        {
                            return new JsonResult(new ApiResponse<int>
                            {
                                Message = string.Format(ResponseMessages.AlreadyExists, ModuleNames.AllCandidates),
                                Result = true,
                                StatusCode = ResponseStatusCode.AlreadyExist
                            });
                        }
                        string[] insertedEmails = parameters.Get<string[]>("inserted_emails");
                        foreach (var email in insertedEmails)
                        {
                            var password = RandomPasswordGenerator.GenerateRandomPassword(8);
                            User? user = _appDbContext.Users.FirstOrDefault(u => u.Email == email);
                            if (user != null)
                            {
                                user.Password = password;
                            }
                            _appDbContext.SaveChanges();
                            var record = records.Find(r => r.email == email);
                            if (record != null)
                            {
                                SendMailForPassword(record.firstname, record.lastname, email, password);
                            }
                        }
                    }

                    return new JsonResult(new ApiResponse<int>
                    {
                        Data = result.candidates_added_count,
                        Message = string.Format(ResponseMessages.AddSuccess, ModuleNames.Candidates),
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in UserRepository.ImportUsers \n MESSAGE : {ex.Message} \n INNER EXCEPTION : {ex.InnerException} \n");
                return new JsonResult(new ApiResponse<string>
                {
                    Message = ResponseMessages.InternalError,
                    Result = false,
                    StatusCode = ResponseStatusCode.RequestFailed
                });
            }

        }

        #endregion

        #region ChangeUserPasswordByAdmin
        public async Task<JsonResult> ChangeUserPasswordByAdmin(string? Email, string? Password)
        {
            try
            {
                if (string.IsNullOrEmpty(Password) || Regex.Match(Password, "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!^#%*?&])[A-Za-z\\d@$!^#%*?&]{8,}$") == null)
                {
                    return new JsonResult(new ApiResponse<string>() { Message = ResponseMessages.InvalidCredentials, Result = false, StatusCode = ResponseStatusCode.NotAcceptable });
                }
                if (!string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password))
                {
                    User? user = _appDbContext.Users.Where(u => u.Email == Email.Trim() && u.IsDeleted == false)?.FirstOrDefault();
                    if (user != null)
                    {
                        user.Password = Password;
                        user.UpdatedDate = DateTime.UtcNow;
                        user.UpdatedBy = 1;
                        int count = _appDbContext.SaveChanges();
                        if (count == 1)
                        {
                            bool mailSent = SendMailAfterPasswordChangeByAdmin(user.Email, user.Password, user.FirstName, user.LastName);
                            if (mailSent)
                            {
                                return new JsonResult(new ApiResponse<string>() { Message = ResponseMessages.PasswordUpdatedSuccess, Result = true, StatusCode = ResponseStatusCode.Success });
                            }
                            return new JsonResult(new ApiResponse<string>
                            {
                                Message = ResponseMessages.InternalError,
                                Result = false,
                                StatusCode = ResponseStatusCode.RequestFailed
                            });
                        }
                    }
                }
                return new JsonResult(new ApiResponse<string>() { Message = ResponseMessages.BadRequest, Result = false, StatusCode = ResponseStatusCode.BadRequest });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in UserRepository.ChangeUserPasswordByAdmin \n MESSAGE : {ex.Message} \n INNER EXCEPTION : {ex.InnerException} \n");
                return new JsonResult(new ApiResponse<string>
                {
                    Message = ResponseMessages.InternalError,
                    Result = false,
                    StatusCode = ResponseStatusCode.RequestFailed
                });
            }

        }
        #endregion

        #region GetUsersExportData

        public async Task<JsonResult> GetUsersExportData(string? searchQuery, int? groupId, int? collegeId, int? yearAdded, string? sortField, string? sortOrder, int? currentPageIndex, int? pageSize)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    int? testId = null;
                    List<UserExportDataVM> data = connection.Query<UserExportDataVM>("Select * from getallusersexport(@SearchQuery,@GroupId,@CollegeId,@YearAttended,@PageNumber,@PageSize,@SortField,@SortOrder)", new { SearchQuery = searchQuery, GroupId = groupId, CollegeId = collegeId, TestId = testId, YearAttended = yearAdded, PageSize = pageSize, PageNumber = currentPageIndex, SortField = sortField, SortOrder = sortOrder }).ToList();
                    if (!data.Any())
                    {
                        return new JsonResult(new ApiResponse<List<UserExportDataVM>>
                        {
                            Data = data,
                            Message = ResponseMessages.NoRecordsFound,
                            Result = true,
                            StatusCode = ResponseStatusCode.NotFound
                        });
                    }

                    return new JsonResult(new ApiResponse<List<UserExportDataVM>>
                    {
                        Data = data,
                        Message = ResponseMessages.Success,
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in UserRepository.GetUsersExportData \n MESSAGE : {ex.Message} \n INNER EXCEPTION : {ex.InnerException} \n");
                return new JsonResult(new ApiResponse<string>
                {
                    Message = ResponseMessages.InternalError,
                    Result = false,
                    StatusCode = ResponseStatusCode.RequestFailed
                });
            }
        }

        #endregion

        #region HelpingMethods

        private static void FillUserData(UserDetailsVM userDetails, dynamic data)
        {
            var userData = data[0];
            userDetails.UserId = userData.userid ?? 0;
            userDetails.UserGroup = userData.usergroup ?? 0;
            userDetails.GroupName = userData.groupname ?? "";
            userDetails.UserCollege = userData.usercollege ?? 0;
            userDetails.CollegeName = userData.collegename ?? "";
            userDetails.FirstName = userData.firstname ?? "";
            userDetails.FatherName = userData.fathername ?? "";
            userDetails.LastName = userData.lastname ?? "";
            userDetails.Gender = userData.gender ?? 0;
            userDetails.Email = userData.email ?? "";
            userDetails.Password = userData.password ?? "";
            userDetails.PhoneNumber = userData.phonenumber ?? 0;
            userDetails.DateOfBirth = userData.dateofbirth ?? DateTime.Now;
            userDetails.PermanentAddress1 = userData.permanentaddress1 ?? "";
            userDetails.PermanentAddress2 = userData.permanentaddress ?? "";
            userDetails.Pincode = userData.pincode ?? 0;
            userDetails.CityName = userData.cityname ?? "";
            userDetails.State = userData.StateId ?? 0;
            userDetails.StateName = userData.statename ?? "";
            userDetails.AppliedThrough = userData.appliedthrough ?? 0;
            userDetails.TechnologyInterestedIn = userData.technologyinterestedin ?? 0;
            userDetails.TechnologyName = userData.technologyname ?? "";
            userDetails.ACPCMeritRank = userData.acpcmeritrank ?? 0;
            userDetails.GUJCETScore = userData.gujcetscore ?? 0;
            userDetails.JEEScore = userData.jeescore ?? 0;
            userDetails.Status = userData.status ?? 0;
            userDetails.CreatedYear = userData.createdyear ?? 0;
            userDetails.PreferredLocation = userData.preferredlocation ?? 0;

        }

        private static void FillAcademicAndFamilyData(dynamic data, UserDetailsVM userDetails, List<int> acadamicIds, List<int> familyIds)
        {
            foreach (var entity in data)
            {
                if (entity.AcademicId != null)
                {
                    if (!acadamicIds.Contains(entity.AcademicId))
                    {
                        UserAcademicsVM userAcademics = new UserAcademicsVM();
                        userAcademics.AcademicId = entity.AcademicId;
                        userAcademics.DegreeId = entity.DegreeId;
                        userAcademics.StreamId = entity.StreamId;
                        userAcademics.Physics = entity.Physics;
                        userAcademics.Maths = entity.Maths;
                        userAcademics.Grade = entity.Grade;
                        userAcademics.University = entity.University;
                        userAcademics.DurationFromYear = entity.DurationFromYear;
                        userAcademics.DurationFromMonth = entity.DurationFromMonth;
                        userAcademics.DurationToYear = entity.DurationToYear;
                        userAcademics.DurationToMonth = entity.DurationToMonth;
                        userAcademics.DegreeName = entity.degreename;
                        userAcademics.DegreeLevel = entity.degreelevel;
                        userAcademics.StreamName = entity.streamname;
                        userDetails.AcademicsDetails.Add(userAcademics);
                    }
                    acadamicIds.Add(entity.AcademicId);
                }

                if (entity.familyid != null)
                {
                    if (!familyIds.Contains(entity.familyid))
                    {
                        UserFamilyVM family = new UserFamilyVM();
                        family.FamilyId = entity.familyid;
                        family.FamilyPerson = entity.FamilyPerson;
                        family.Qualification = entity.Qualification;
                        family.Occupation = entity.Occupation;
                        userDetails.FamilyDetails.Add(family);
                    }
                    familyIds.Add(entity.familyid);
                }
            }
        }

        #region SendEmail
        private bool SendMailAfterPasswordChangeByAdmin(string email, string password, string firstName, string lastName)
        {
            try
            {
                var subject = "Password change request for Tatvasoft - Aptitude Test Portal";
                var body = $"<h3>Hello {firstName} {lastName},</h3>We have received Password change request from you,<br/>Here are your new credentials!!<br /><h4>User name: {email}</h4><h4>Password: {password}</h4>Click on below button to login.<br/><a href = {userLoginUrl}><button btn-primary>Login</button></a>";
                var emailHelper = new EmailHelper(_config, _appDbContext);
                var isEmailSent = emailHelper.SendEmail(email, subject, body);
                return isEmailSent;
            }
            catch
            {
                return false;
            }
        }

        private bool SendMailForPassword(string firstName, string lastName, string email, string password)
        {
            try
            {
                var subject = "Confirm new user sign-up in Tatvasoft - Aptitude Test Portal";
                var body = $"<h3>Welcome {firstName} {lastName},</h3>We have received registration request for you,<br/>Here are your credentials to login!!<br /><h4>User name: {email}</h4><h4>Password: {password}</h4>Click on below button to login.<br/><a href = {userLoginUrl}><button btn-primary>Login</button></a>";
                var emailHelper = new EmailHelper(_config, _appDbContext);
                var isEmailSent = emailHelper.SendEmail(email, subject, body);
                return isEmailSent;
            }
            catch
            {
                return false;
            }
        }

        #region validationImportedUsers
        private static async Task<ValidateImportFileVM> checkImportedData(List<UserImportVM> records)
        {

            ValidateImportFileVM validate = new();

            foreach (var record in records)
            {
                var context = new ValidationContext(record, serviceProvider: null, items: null);
                var results = new List<ValidationResult>();

                if (!Validator.TryValidateObject(record, context, results, validateAllProperties: true))
                {
                    validate.validationMessage = new List<string>();
                    validate.isValidate = false;
                    validate.validationMessage.AddRange(results.Select(x => x.ErrorMessage).ToList());

                }
            }
            return validate;
        }

        #endregion


        private static string GetValueForHeader(string[] row, string[] headers, string headerName)
        {
            var index = Array.IndexOf(headers, headerName);
            return index >= 0 && index < row.Length ? row[index] : null;
        }
        #endregion

        private static List<string[]> GetCsvContent(StreamReader reader)
        {
            var csvContent = new List<string[]>();
            while (!reader.EndOfStream)
            {
                string? line = reader.ReadLine();
                if (!line.IsNullOrEmpty())
                {
                    var values = line!.Split(',');
                    csvContent.Add(values);
                }
            }
            return csvContent;
        }

        private static List<UserImportVM> GetRecords(string[]? headers, List<string[]> csvContent)
        {
            List<UserImportVM> records = new List<UserImportVM>();
            if (headers != null)
            {
                foreach (var item in csvContent.Skip(1))
                {
                    if (item.Length > 1)
                    {

                        var viewModel = new UserImportVM
                        {
                            firstname = GetValueForHeader(item, headers, "First Name"),
                            lastname = GetValueForHeader(item, headers, "Last Name"),
                            middlename = GetValueForHeader(item, headers, "Middle Name"),
                            email = GetValueForHeader(item, headers, "Email"),
                            contactnumber = long.Parse(GetValueForHeader(item, headers, "Contact Number")),
                            status = GetValueForHeader(item, headers, "Status(true/false)"),
                        };
                        records.Add(viewModel);
                    }

                }
            }
            return records;
        }

        private static bool IsStatusTrue(string status)
        {
            if (status.ToLower() == "true" || string.IsNullOrEmpty(status))
            {
                return true;
            }
            return false;
        }

        private static bool DoesResultExists(ImportCandidateResponseVM? result)
        {
            if (result != null && result.candidates_added_count == 0)
            {
                return true;
            }
            return false;
        }

        private int addOtherCollege(UserVM user)
        {
            string collegeName = user.OtherCollege.Trim() + "(O)";
            string trimmedCollegeName = user.OtherCollege.Trim();
            MasterCollege? masterCollege = _appDbContext.MasterCollege.Where(x => (x.Name.Trim().ToLower() == collegeName.Trim().ToLower() || x.Name.Trim().ToLower() == user.OtherCollege.Trim().ToLower()) && x.IsDeleted != true).FirstOrDefault();
            if (masterCollege != null)
            {
                if ((bool)masterCollege.Status)
                {
                    return (int)CollegeStatus.Exists;
                }
                else
                {
                    return (int)CollegeStatus.InActive;
                }
            }
            MasterCollege college = new MasterCollege() { Abbreviation = trimmedCollegeName[0].ToString() + trimmedCollegeName[trimmedCollegeName.Length - 1], Name = collegeName };
            _appDbContext.MasterCollege.Add(college);
            _appDbContext.SaveChanges();
            user.CollegeId = _appDbContext.MasterCollege.Where(x => x.Name == collegeName).FirstOrDefault().Id;
            return (int)CollegeStatus.NotExists;
        }

        #endregion

        #endregion

    }
}
