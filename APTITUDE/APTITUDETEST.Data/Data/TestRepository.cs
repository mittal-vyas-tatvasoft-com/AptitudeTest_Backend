using AptitudeTest.Core.Entities.Admin;
using AptitudeTest.Core.Entities.Master;
using AptitudeTest.Core.Entities.Test;
using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using AptitudeTest.Data.Common;
using APTITUDETEST.Common.Data;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AptitudeTest.Data.Data
{
    public class TestRepository : ITestRepository
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        private readonly string connectionString;

        public TestRepository(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
            connectionString = _config["ConnectionStrings:AptitudeTest"];
        }

        #region Methods
        public async Task<JsonResult> GetTests(string? searchQuery, int? GroupId, int? Status, DateTime? Date, int? currentPageIndex, int? pageSize)
        {
            try
            {
                searchQuery = string.IsNullOrEmpty(searchQuery) ? string.Empty : searchQuery;
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    List<TestsViewModel> data = connection.Query<TestsViewModel>("Select * from getalltests(@SearchQuery,@GroupId,@Status,@DateFilter,@PageNumber,@PageSize)", new { SearchQuery = searchQuery, GroupId = (object)GroupId, Status = Status, DateFilter = Date, PageNumber = currentPageIndex, PageSize = pageSize }).ToList();
                    connection.Close();
                    return new JsonResult(new ApiResponse<List<TestsViewModel>>
                    {
                        Data = data.OrderByDescending(x => x.Testid).ToList(),
                        Message = ResponseMessages.Success,
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }

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


        public async Task<JsonResult> CreateTest(CreateTestVM test)
        {
            try
            {           
                Test? testAlreadyExists = _context.Tests.Where(t => t.Name == test.Name && t.Status == (int)Common.Enums.TestStatus.Active && t.IsDeleted == false).FirstOrDefault();
                if (testAlreadyExists == null)
                {
                    Test testToBeAdded = new Test()
                    {
                        Name = test.Name,
                        Description = test.Description,
                        Date = test.Date,
                        StartTime = test.StartTime,
                        EndTime = test.EndTime,
                        TestDuration = test.TestDuration,
                        Status = test.Status,
                        BasicPoint = test.BasicPoint,
                        MessaageAtStartOfTheTest = test.MessaageAtStartOfTheTest,
                        MessaageAtEndOfTheTest = test.MessaageAtEndOfTheTest,
                        IsRandomQuestion = test.IsRandomQuestion,
                        IsRandomAnswer = test.IsRandomAnswer,
                        IsLogoutWhenTimeExpire = test.IsLogoutWhenTimeExpire,
                        IsQuestionsMenu = test.IsQuestionsMenu,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = test.CreatedBy,
                    };

                    _context.Add(testToBeAdded);
                    _context.SaveChanges();
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.AddSuccess, ModuleNames.Test),
                        Result = true,
                        StatusCode = ResponseStatusCode.OK
                    });
                }
                else
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.AlreadyExists, ModuleNames.Test),
                        Result = false,
                        StatusCode = ResponseStatusCode.AlreadyExist
                    });
                }

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

        public async Task<JsonResult> UpdateTestGroup(UpdateTestGroupVM updateTest)
        {
            try
            {
                Test testAlreadyExists =  _context.Tests.Where(t => t.Id != updateTest.TestId && t.GroupId == updateTest.GroupId && t.Status == (int)Common.Enums.TestStatus.Active && t.IsDeleted == false).FirstOrDefault();
                if (testAlreadyExists != null)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.AlreadyExists, ModuleNames.Test),
                        Result = true,
                        StatusCode = ResponseStatusCode.OK
                    });
                }
                Test test = await Task.FromResult(_context.Tests.Where(t => t.Id == updateTest.TestId && t.Status == (int)Common.Enums.TestStatus.Active && t.IsDeleted == false).FirstOrDefault());
                if (test != null)
                {
                    test.GroupId = updateTest.GroupId;
                    test.UpdatedBy = updateTest.UpdatedBy;
                    test.UpdatedDate = DateTime.UtcNow;
                    _context.SaveChanges();

                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.UpdateSuccess, ModuleNames.Test),
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }
                return new JsonResult(new ApiResponse<string>
                {
                    Message = ResponseMessages.InternalError,
                    Result = false,
                    StatusCode = ResponseStatusCode.InternalServerError
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
    }
}
