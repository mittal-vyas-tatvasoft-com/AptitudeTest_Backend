using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        #region Properties
        private readonly ISettingsService _service;
        #endregion

        #region Constructor
        public SettingsController(ISettingsService service)
        {
            _service = service;
        }
        #endregion

        [HttpGet("[action]")]
        public async Task<JsonResult> Get()
        {
            return await _service.Get();
        }

        [Authorize]
        [HttpPut("[action]")]
        public async Task<JsonResult> Update(UpdateSettingsVM updateSettingsVM)
        {
            return await _service.Update(updateSettingsVM);
        }
    }
}
