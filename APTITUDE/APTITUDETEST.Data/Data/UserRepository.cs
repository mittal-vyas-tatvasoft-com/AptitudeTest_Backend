using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using AptitudeTest.Data.Common;
using APTITUDETEST.Common.Data;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace AptitudeTest.Data.Data
{
    public class UserRepository : IUsersRepository
    {

        #region Properies
        private readonly AppDbContext _appDbContext;
        private readonly IConfiguration _config;
        private readonly string connectionString;

        #endregion

        #region Constructor
        public UserRepository(AppDbContext appDbContext, IConfiguration config)
        {
            _appDbContext = appDbContext;
            _config = config;
            connectionString = _config["ConnectionStrings:AptitudeTest"];
        }
        #endregion

        #region methods

        #region GetAllUsers
        public async Task<JsonResult> GetAllUsers(string? searchQuery, int? currentPageIndex, int? pageSize)
        {
            try
            {
                if (!string.IsNullOrEmpty(searchQuery))
                {
                    using (var connection = new NpgsqlConnection(connectionString))
                    {
                        connection.Open();
                        List<UserViewModel> data = connection.Query<UserViewModel>("Select * from getAllUsers(@SearchQuery)", new { SearchQuery = searchQuery }).ToList();
                        PaginationVM<UserViewModel> paginatedData = Pagination<UserViewModel>.Paginate(data, pageSize, currentPageIndex);
                        connection.Close();
                        return new JsonResult(new ApiResponse<PaginationVM<UserViewModel>>
                        {
                            Data = paginatedData,
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
                        List<UserViewModel> data = connection.Query<UserViewModel>("Select * from getAllUsers(@SearchQuery)", new { SearchQuery = "" }).ToList();
                        PaginationVM<UserViewModel> paginatedData = Pagination<UserViewModel>.Paginate(data, pageSize, currentPageIndex);
                        connection.Close();
                        return new JsonResult(new ApiResponse<PaginationVM<UserViewModel>>
                        {
                            Data = paginatedData,
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

        #endregion

        #region GetUserById
        public async Task<JsonResult> GetUserById(int id)
        {
            try
            {
                UserDetailsVM userDetails = new UserDetailsVM();
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    var data = connection.Query("Select * from GetUserbyId(@user_id)", new { user_id = id }).ToList();

                    if (data.Count == 0)
                    {
                        return new JsonResult(new ApiResponse<UserDetailsVM>
                        {
                            Data = null,
                            Message = string.Format(ResponseMessages.NotFound, "User"),
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
                return new JsonResult(new ApiResponse<string>
                {
                    Message = ResponseMessages.InternalError,
                    Result = false,
                    StatusCode = ResponseStatusCode.InternalServerError
                });
            }
        }
        #endregion

        #region HelpingMethods

        private void FillUserData(UserDetailsVM userDetails, dynamic data)
        {
            userDetails.UserId = data[0].userid ?? 0;
            userDetails.FirstName = data[0].firstname ?? "";
            userDetails.LastName = data[0].lastname ?? "";
            userDetails.Email = data[0].email ?? "";
            userDetails.PhoneNumber = data[0].phonenumber ?? 0;
            userDetails.FatherName = data[0].fathername ?? "";
            userDetails.Level = data[0].level ?? 0;
            userDetails.DateOfBirth = data[0].dateofbirth ?? new DateTime();
            userDetails.PermanentAddress = data[0].permanentaddress ?? "";
            userDetails.UserGroup = data[0].usergroup ?? 0;
            userDetails.GroupName = data[0].groupname ?? "";
            userDetails.AppliedThrough = data[0].appliedthrough ?? 0;
            userDetails.TechnologyInterestedIn = data[0].technologyinterestedIn ?? 0;
            userDetails.TechnologyName = data[0].technologyname ?? "";
            userDetails.ACPCMeritRank = data[0].acpcmeritrank ?? 0;
            userDetails.GUJCETScore = data[0].gujcetscore ?? 0;
            userDetails.JEEScore = data[0].jeescore ?? 0;
            userDetails.Gender = data[0].gender ?? 0;
            userDetails.PreferedLocation = data[0].preferedlocation ?? 0;
            userDetails.RelationshipWithExistingEmployee = data[0].relationshipwithexistingemployee ?? "";
            userDetails.Status = data[0].status ?? 0;
            userDetails.RoleId = data[0].roleid ?? 0;
        }

        private void FillAcademicAndFamilyData(dynamic data, UserDetailsVM userDetails, List<int> acadamicIds, List<int> familyIds)
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
        #endregion

        #endregion

    }
}
