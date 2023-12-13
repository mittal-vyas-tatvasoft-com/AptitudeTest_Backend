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
    public class ResultRepository : IResultRepository
    {
        #region Properties
        private readonly string? connectionString;
        #endregion

        #region Constructor
        public ResultRepository(AppDbContext context, IConfiguration config)
        {
            IConfiguration _config;
            _config = config;
            connectionString = _config["ConnectionStrings:AptitudeTest"];
        }
        #endregion

        #region Methods
        public async Task<JsonResult> Get(int id, int marks, int pageSize, int pageIndex)
        {
            return null;
        }

        public async Task<JsonResult> GetResults(string? searchQuery, int? TestId, int? GroupId, int? CollegeId, int? Year, int? currentPageIndex, int? pageSize, string? sortField, string? sortOrder)
        {
            try
            {
                searchQuery = string.IsNullOrEmpty(searchQuery) ? string.Empty : searchQuery;
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    List<ResultsVM> data = connection.Query<ResultsVM>("Select * from getallresults_3(@SearchQuery,@GroupId,@CollegeId,@TestId,@YearAttended,@PageNumber,@PageSize,@SortField,@SortOrder)", new { SearchQuery = searchQuery, GroupId = (object)GroupId!, CollegeId = (object)CollegeId!, TestId = (object)TestId!, YearAttended = Year, PageNumber = currentPageIndex, PageSize = pageSize, SortField = sortField, SortOrder = sortOrder }).ToList();
                    connection.Close();
                    return new JsonResult(new ApiResponse<List<ResultsVM>>
                    {
                        Data = data,
                        Message = ResponseMessages.Success,
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }

            }

            catch
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
