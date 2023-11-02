using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Application.Services
{
    public class AdminService : IAdminService
    {
        #region Properties
        private readonly IAdminRepository _adminRepository;
        #endregion

        #region Constructor
        public AdminService(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }
        #endregion

        #region Methods
        public async Task<JsonResult> GetAllAdmin(string? searchQuery, bool? Status, int? currentPageIndex, int? pageSize)
        {
            return await _adminRepository.GetAllAdmin(searchQuery, Status, currentPageIndex, pageSize);
        }

        public async Task<JsonResult> GetAdminById(int? id)
        {
            return await _adminRepository.GetAdminById(id);
        }

        public async Task<JsonResult> Create(CreateAdminVM admin)
        {
            return await _adminRepository.Create(admin);
        }

        #endregion
    }
}
