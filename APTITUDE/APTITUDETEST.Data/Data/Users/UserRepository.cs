using AptitudeTest.Core.Interfaces.Users;
using AptitudeTest.Core.ViewModels.Common;
using AptitudeTest.Core.ViewModels.User;
using AptitudeTest.Data.Common;
using APTITUDETEST.Common.Data;
using APTITUDETEST.Core.Entities.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AptitudeTest.Data.Data.Users
{
    public class UserRepository : RepositoryBase<User>, IUsersRepository
    {
        private readonly AppDbContext _appDbContext;


        public UserRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<JsonResult> GetAllUsers(string? searchQuery, int? currentPageIndex, int? pageSize)
        {
            try
            {
                string sqlQuery = $"Select * from SearchUsers(NULL,'{searchQuery}',NULL,NULL,NULL,NULL,{currentPageIndex},{pageSize})";
                var userVM = await Task.FromResult(_appDbContext.UsersDetails.FromSqlRaw(sqlQuery).ToList());
                return new JsonResult(new ApiResponse<List<UserViewModel>>
                {
                    Data = userVM,
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

        public async Task<JsonResult> GetUserById(int id)
        {
            try
            {
                string sqlQuery = $"Select * from SearchUsers({id},NULL,NULL,NULL,NULL,NULL,NULL,NULL)";
                var userVM = await Task.FromResult(_appDbContext.UsersDetails.FromSqlRaw(sqlQuery).ToList());
                return new JsonResult(new ApiResponse<List<UserViewModel>>
                {
                    Data = userVM,
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
        
    }
}
