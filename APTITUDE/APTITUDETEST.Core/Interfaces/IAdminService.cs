using AptitudeTest.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Core.Interfaces
{
    public interface IAdminService
    {
        Task<JsonResult> GetAllAdmin(string? searchQuery, bool? Status, string? sortField, string? sortOrder, int? currentPageIndex, int? pageSize);
        Task<JsonResult> GetAdminById(int? id);
        Task<JsonResult> Create(CreateAdminVM admin);
        Task<JsonResult> Update(CreateAdminVM admin);
        Task<JsonResult> ActiveInActiveAdmin(AdminStatusVM adminStatusVM);
        Task<JsonResult> Delete(int id);
    }
}
