using AptitudeTest.Core.Entities.Setting;
using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using AptitudeTest.Data.Common;
using APTITUDETEST.Common.Data;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Data.Data
{
    public class SettingsRepository : ISettingsRepository
    {
        #region Properties
        readonly AppDbContext _context;
        #endregion

        #region Constructor
        public SettingsRepository(AppDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Methods
        public async Task<JsonResult> Get()
        {
            try
            {

                SettingConfigurations settingConfigurations = await Task.FromResult(_context.SettingConfigurations.FirstOrDefault());
                return new JsonResult(new ApiResponse<SettingConfigurations>
                {
                    Data = settingConfigurations,
                    Message = ResponseMessages.Success,
                    Result = true,
                    StatusCode = ResponseStatusCode.Success
                });
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

        public async Task<JsonResult> Update(UpdateSettingsVM updateSettingsVM)
        {
            try
            {

                SettingConfigurations settingConfigurations = await Task.FromResult(_context.SettingConfigurations.FirstOrDefault());
                settingConfigurations.UserRegistration = updateSettingsVM.UserRegistration;
                settingConfigurations.Camera = updateSettingsVM.Camera;
                settingConfigurations.ScreenCapture = updateSettingsVM.ScreenCapture;
                _context.Update(settingConfigurations);
                _context.SaveChanges();
                return new JsonResult(new ApiResponse<string>
                {
                    Message = string.Format(ResponseMessages.UpdateSuccess, ModuleNames.Setting),
                    Result = true,
                    StatusCode = ResponseStatusCode.Success
                });
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
