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
        public async Task<JsonResult> Get(int id, int marks, int pageSize, int pageIndex)
        {
            return await _repository.Get(id, marks, pageSize, pageIndex);
        }
        #endregion
    }
}
