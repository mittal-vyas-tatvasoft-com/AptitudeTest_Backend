using AptitudeTest.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Application.Services
{
    public class ResultService : IResultService
    {
        #region Properties
        private readonly IResultRepository _repository;
        #endregion

        #region Constructor
        public ResultService(IResultRepository repository)
        {
            _repository = repository;
        }
        #endregion

        #region Methods
        public async Task<JsonResult> Get(int id, int testId, int marks, int pageSize, int pageIndex)
        {
            return await _repository.Get(id, testId, marks, pageSize, pageIndex);
        }

        public Task<JsonResult> GetResults(string? searchQuery, int? TestId, int? GroupId, int? CollegeId, int? Year, int? currentPageIndex, int? pageSize, string? sortField, string? sortOrder)
        {
            return _repository.GetResults(searchQuery, TestId, GroupId, CollegeId, Year, currentPageIndex, pageSize, sortField, sortOrder);
        }
        #endregion
    }
}
