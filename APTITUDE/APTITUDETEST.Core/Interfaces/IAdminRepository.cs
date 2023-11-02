using AptitudeTest.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Core.Interfaces
{
    public interface IAdminRepository
    {
        Task<JsonResult> GetAllAdmin(string? searchQuery, bool? Status, int? currentPageIndex, int? pageSize);
        Task<JsonResult> GetAdminById(int? id);
        Task<JsonResult> Create(CreateAdminVM admin);

    }
}
