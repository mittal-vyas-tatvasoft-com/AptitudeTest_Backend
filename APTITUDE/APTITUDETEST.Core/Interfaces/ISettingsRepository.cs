using AptitudeTest.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Core.Interfaces
{
    public interface ISettingsRepository
    {
        public Task<JsonResult> Get();
        public Task<JsonResult> Update(UpdateSettingsVM updateSettingsVM);
    }
}
