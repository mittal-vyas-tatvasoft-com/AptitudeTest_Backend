using AptitudeTest.Core.Interfaces.Master;
using AptitudeTest.Core.ViewModels.Master;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Application.Services.Master
{
    public class StreamService : IStreamService
    {
        #region Properties
        private readonly IStreamRepository _repository;
        #endregion

        #region Constructor
        public StreamService(IStreamRepository repository)
        {
            _repository = repository;
        }
        #endregion

        #region Methods

        public async Task<JsonResult> Getstreams(string? searchQuery, int? filter, List<int>? degreelist, int? currentPageIndex, int? pageSize)
        {
            return await _repository.Getstreams(searchQuery, filter, degreelist, currentPageIndex, pageSize);
        }

        public async Task<JsonResult> Upsert(StreamVM stream)
        {
            return await _repository.Upsert(stream);
        }

        public async Task<JsonResult> CheckUncheckAll(bool check)
        {
            return await _repository.CheckUncheckAll(check);
        }

        public async Task<JsonResult> Delete(int id)
        {
            return await _repository.Delete(id);
        }
        #endregion
    }
}
