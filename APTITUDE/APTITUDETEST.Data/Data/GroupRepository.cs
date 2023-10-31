using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using AptitudeTest.Data.Common;
using APTITUDETEST.Common.Data;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Data.Data
{
    public class GroupRepository : IGroupRepository
    {
        private readonly AppDbContext _context;


        public GroupRepository(AppDbContext context)
        {
            _context = context;
        }
        #region Methods
        public async Task<JsonResult> GetActiveGroups()
        {
            try
            {
                var collegeList = await Task.FromResult(_context.MasterGroup
            .Where(x => (x.IsDeleted == null || x.IsDeleted == false) && x.Status == true)
            .Select(x => new { Id = x.Id, Name = x.Name })
            .ToList());

                if (collegeList != null)
                {
                    return new JsonResult(new ApiResponse<IEnumerable<object>>
                    {
                        Data = collegeList,
                        Message = ResponseMessages.Success,
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }
                else
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Data = string.Format(ResponseMessages.NotFound, "Group"),
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
