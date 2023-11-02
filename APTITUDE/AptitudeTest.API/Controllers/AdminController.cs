using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using AptitudeTest.Data.Common;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        #region Properties
        private readonly IAdminService _adminService;
        #endregion

        #region Constructor
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }
        #endregion

        [HttpGet("[action]")]
        public async Task<JsonResult> GetAllAdmin(string? searchQuery, bool? Status, int currentPageIndex = 0, int pageSize = 10)
        {
            return await _adminService.GetAllAdmin(searchQuery, Status, currentPageIndex, pageSize);
        }

        [HttpGet("[action]/{id:int}")]
        public async Task<JsonResult> GetAdminById(int? id)
        {
            return await _adminService.GetAdminById(id);
        }

        [HttpPost("[action]")]
        public async Task<JsonResult> Create(CreateAdminVM admin)
        {
            if (ModelState.IsValid)
            {
                return await _adminService.Create(admin);
            }

            return new JsonResult(new ApiResponse<string>() { Message = ResponseMessages.BadRequest, Result = false, StatusCode = ResponseStatusCode.BadRequest });
        }

    }
}
