using AptitudeTest.Core.Entities.Users;
using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using AptitudeTest.Data.Common;
using APTITUDETEST.Common.Data;
using APTITUDETEST.Core.Entities.Users;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace AptitudeTest.Data.Data
{
    public class UserRepository : RepositoryBase<User>, IUsersRepository
    {

        #region Properies
        private readonly AppDbContext _appDbContext;
        private readonly IConfiguration _config;
        private readonly string connectionString;

        #endregion

        #region Dependacy
        public UserRepository(AppDbContext appDbContext, IConfiguration config) : base(appDbContext)
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
                var AllUserData = await Users(_appDbContext.Users);

                if (!string.IsNullOrEmpty(searchQuery))
                {
                    List<UserViewModel> searchedUsers = await SearchedUsers(AllUserData, searchQuery);
                    PaginationVM<UserViewModel> paginatedData = Pagination<UserViewModel>.Paginate(searchedUsers, pageSize, currentPageIndex);
                    return new JsonResult(new ApiResponse<PaginationVM<UserViewModel>>
                    {
                        Data = paginatedData,
                        Message = ResponseMessages.Success,
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }
                else
                {
                    PaginationVM<UserViewModel> paginatedData = Pagination<UserViewModel>.Paginate(AllUserData.ToList(), pageSize, currentPageIndex);
                    return new JsonResult(new ApiResponse<PaginationVM<UserViewModel>>
                    {
                        Data = paginatedData,
                        Message = ResponseMessages.Success,
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }
            }
            catch
            {
                return new JsonResult(new ApiResponse<List<UserViewModel>>
                {
                    Data = null,
                    Message = ResponseMessages.Success,
                    Result = true,
                    StatusCode = ResponseStatusCode.Success
                });
            }
        }

        #endregion

        #region GetUserById
        public async Task<JsonResult> GetUserById(int id)
        {
            try
            {
                var UserDataFromId = await Users(_appDbContext.Users.Where(u => u.Id == id));
                List<UserViewModel>? user = UserDataFromId.ToList();
                return new JsonResult(new ApiResponse<List<UserViewModel>>
                {
                    Data = user,
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

        #region FetchUserDataAccordingToRequirement
        private async Task<IEnumerable<UserViewModel>> Users(IQueryable<User> user)
        {
            try
            {
                var UserDataFromId = await Task.FromResult(from users in user
                                                           join userAcademics in _appDbContext.UserAcademics on users.Id equals userAcademics.UserId into userAcademicsGroup
                                                           from userAcademics in userAcademicsGroup.DefaultIfEmpty()
                                                           join userFamilyData in _appDbContext.UserFamily on users.Id equals userFamilyData.UserId into userFamilyDataGroup
                                                           from userFamilyData in userFamilyDataGroup.DefaultIfEmpty()
                                                           join masterDegree in _appDbContext.MasterDegree on userAcademics.DegreeId equals masterDegree.Id into masterDegreeGroup
                                                           from masterDegree in masterDegreeGroup.DefaultIfEmpty()
                                                           join masterStream in _appDbContext.MasterStream on userAcademics.DegreeId equals masterStream.Id into masterStreamGroup
                                                           from masterStream in masterStreamGroup.DefaultIfEmpty()
                                                           join masterGroup in _appDbContext.MasterGroup on users.Group equals masterGroup.Id into masterGroups
                                                           from masterGroup in masterGroups.DefaultIfEmpty()
                                                           join masterTechnology in _appDbContext.MasterTechnology on users.Group equals masterTechnology.Id into masterTechnologies
                                                           from masterTechnology in masterTechnologies.DefaultIfEmpty()
                                                           select new UserViewModel
                                                           {
                                                               UserId = users.Id,
                                                               FirstName = users.FirstName,
                                                               LastName = users.LastName,
                                                               FatherName = users.FatherName,
                                                               Email = users.Email,
                                                               PhoneNumber = users.PhoneNumber,
                                                               Level = users.Level,
                                                               Group = masterGroup,
                                                               PreferedLocation = users.PreferedLocation,
                                                               RoleId = users.RoleId,
                                                               ACPCMeritRank = users.ACPCMeritRank,
                                                               AppliedThrough = users.AppliedThrough,
                                                               DateOfBirth = users.DateOfBirth,
                                                               Gender = users.Gender,
                                                               JEEScore = users.JEEScore,
                                                               GUJCETScore = users.GUJCETScore,
                                                               RelationshipWithExistingEmployee = users.RelationshipWithExistingEmployee,
                                                               Status = users.Status,
                                                               PermanentAddress = users.PermanentAddress,
                                                               TechnologyInterestedIn = masterTechnology,
                                                               UserAcademics = new UserAcademics
                                                               {
                                                                   MasterDegrees = masterDegree,
                                                                   MasterStreams = masterStream
                                                               },
                                                               UserFamilyData = userFamilyData
                                                           });

                return UserDataFromId;
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region SearchUsersInFullData
        private async Task<List<UserViewModel>> SearchedUsers(IEnumerable<UserViewModel> AllUserData, string? searchQuery)
        {
            return AllUserData.Where(u => u.UserFamilyData.Qualification.Contains(searchQuery) || u.UserFamilyData.Occupation.Contains(searchQuery) || u.UserAcademics.MasterDegrees.Name.Contains(searchQuery) || u.UserAcademics.MasterStreams.Name.Contains(searchQuery) || u.FirstName.Contains(searchQuery) || u.LastName.Contains(searchQuery) || u.FatherName.Contains(searchQuery) || u.Email.Contains(searchQuery) || u.RelationshipWithExistingEmployee.Contains(searchQuery) || u.JEEScore.ToString().Contains(searchQuery) || u.GUJCETScore.ToString().Contains(searchQuery) || u.ACPCMeritRank.ToString().Contains(searchQuery) || u.Group.Name.Contains(searchQuery) || u.TechnologyInterestedIn.Name.Contains(searchQuery)).ToList();
        }
        #endregion

        #region Dapper
        public async Task<JsonResult> GetUserByIdUsingDapper(int id)
        {
            try
            {
                UserDetailsVM userDetails = new UserDetailsVM();
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    var data = connection.Query("Select * from GetUserbyId(@user_id)", new { user_id = id }).ToList();

                    userDetails.AcademicsDetails = new List<UserAcademicsVM>();
                    userDetails.FamilyDetails = new List<UserFamilyVM>();
                    List<int> FamilyIds = new List<int>();
                    List<int> AcadamicIds = new List<int>();

                    userDetails.UserId = data[0].userid ?? 0;
                    userDetails.FirstName = data[0].firstname ?? "";
                    userDetails.LastName = data[0].lastname ?? "";
                    userDetails.Email = data[0].email ?? "";
                    userDetails.PhoneNumber = data[0].phonenumber ?? 0;
                    userDetails.FatherName = data[0].fathername ?? "";
                    userDetails.Level = data[0].level ?? 0;
                    userDetails.DateOfBirth = data[0].dateofbirth ?? new DateOnly();
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
                    foreach (var entity in data)
                    {

                        if (!AcadamicIds.Contains(entity.AcademicId))
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
                        AcadamicIds.Add(entity.AcademicId);
                        if (!FamilyIds.Contains(entity.familyid))
                        {
                            UserFamilyVM family = new UserFamilyVM();
                            family.FamilyId = entity.familyid;
                            family.FamilyPerson = entity.FamilyPerson;
                            family.Qualification = entity.Qualification;
                            family.Occupation = entity.Occupation;
                            userDetails.FamilyDetails.Add(family);
                        }
                        FamilyIds.Add(entity.familyid);
                    }
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

        #endregion

    }
}
