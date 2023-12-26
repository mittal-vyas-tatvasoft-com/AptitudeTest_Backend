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
        private readonly ILoggerManager _logger;
        #endregion

        #region Constructor
        public SettingsRepository(AppDbContext context, ILoggerManager logger)
        {
            _context = context;
            _logger = logger;
        }
        #endregion

        #region Methods
        public async Task<JsonResult> Get()
        {
            try
            {
                _logger.LogInfo($"SettingsRepository.Get");
                SettingConfigurations? settingConfigurations = await Task.FromResult(_context.SettingConfigurations.FirstOrDefault());
                return new JsonResult(new ApiResponse<SettingConfigurations>
                {
                    Data = settingConfigurations,
                    Message = ResponseMessages.Success,
                    Result = true,
                    StatusCode = ResponseStatusCode.Success
                });
            }

            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in SettingsRepository.Get:{ex}");
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
                _logger.LogInfo($"SettingsRepository.Update");
                SettingConfigurations? settingConfigurations = await Task.FromResult(_context.SettingConfigurations.FirstOrDefault());
                if (settingConfigurations == null)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.NotFound, ModuleNames.Setting),
                        Result = false,
                        StatusCode = ResponseStatusCode.NotFound
                    });
                }
                settingConfigurations.UserRegistration = updateSettingsVM.UserRegistration;
                settingConfigurations.Camera = updateSettingsVM.Camera;
                settingConfigurations.ClearResponseButton = updateSettingsVM.ClearResponseButton;
                settingConfigurations.ScreenCapture = updateSettingsVM.ScreenCapture;
                settingConfigurations.IntervalForScreenCapture = updateSettingsVM.IntervalForScreenCapture;
                settingConfigurations.CutOff = updateSettingsVM.CutOff;
                _context.Update(settingConfigurations);
                _context.SaveChanges();
                return new JsonResult(new ApiResponse<string>
                {
                    Message = string.Format(ResponseMessages.UpdateSuccess, ModuleNames.Setting),
                    Result = true,
                    StatusCode = ResponseStatusCode.Success
                });
            }

            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in SettingsRepository.Update:{ex}");
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
