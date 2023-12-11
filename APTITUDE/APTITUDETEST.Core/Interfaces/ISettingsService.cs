using AptitudeTest.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Core.Interfaces
{
    public interface ISettingsService
    {
        public Task<JsonResult> Get();
        public Task<JsonResult> Update(UpdateSettingsVM updateSettingsVM);
    }
}
