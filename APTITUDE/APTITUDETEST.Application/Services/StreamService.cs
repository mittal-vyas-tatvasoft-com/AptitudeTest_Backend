using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Application.Services
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
        public async Task<JsonResult> GetAllActiveStreams()
        {
            return await _repository.GetAllActiveStreams();
        }

        public async Task<JsonResult> Create(StreamVM stream)
        {
            return await _repository.Create(stream);
        }

        public async Task<JsonResult> Update(StreamVM stream)
        {
            return await _repository.Update(stream);
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
