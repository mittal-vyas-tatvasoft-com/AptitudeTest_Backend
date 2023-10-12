using AptitudeTest.Core.Entities.Users;
using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using AptitudeTest.Data.Common;
using APTITUDETEST.Common.Data;
using APTITUDETEST.Core.Entities.Users;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Data.Data
{
    public class UserRepository : RepositoryBase<User>, IUsersRepository
    {

        #region Properies
        private readonly AppDbContext _appDbContext;
        #endregion

        #region Dependacy
        public UserRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
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
                UserViewModel? user = UserDataFromId.FirstOrDefault();
                return new JsonResult(new ApiResponse<UserViewModel>
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
                                                               TechnologyInterestedIn = users.TechnologyInterestedIn,
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
            return AllUserData.Where(u => u.UserFamilyData.Qualification.Contains(searchQuery) || u.UserFamilyData.Occupation.Contains(searchQuery) || u.UserAcademics.MasterDegrees.Name.Contains(searchQuery) || u.UserAcademics.MasterStreams.Name.Contains(searchQuery) || u.FirstName.Contains(searchQuery) || u.LastName.Contains(searchQuery) || u.FatherName.Contains(searchQuery) || u.Email.Contains(searchQuery) || u.RelationshipWithExistingEmployee.Contains(searchQuery) || u.JEEScore.ToString().Contains(searchQuery) || u.GUJCETScore.ToString().Contains(searchQuery) || u.ACPCMeritRank.ToString().Contains(searchQuery) || u.Group.Name.Contains(searchQuery)).ToList();
        }
        #endregion

        #endregion

    }
}
