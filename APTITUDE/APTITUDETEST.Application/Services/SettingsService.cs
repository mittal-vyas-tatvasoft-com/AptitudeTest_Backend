using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Application.Services
{
    public class SettingsService : ISettingsService
    {
        #region Properties
        private readonly ISettingsRepository _repository;
        #endregion

        #region Constructor
        public SettingsService(ISettingsRepository repository)
        {
            _repository = repository;
        }
        #endregion

        #region Methods
        public async Task<JsonResult> Get()
        {
            return await _repository.Get();

        }

        public async Task<JsonResult> Update(UpdateSettingsVM updateSettingsVM)
        {
            return await _repository.Update(updateSettingsVM);

        }
        #endregion
    }
}
