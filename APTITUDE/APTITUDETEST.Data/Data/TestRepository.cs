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
        #endregion
    }
}
