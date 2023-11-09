using AptitudeTest.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Core.Interfaces
{
    public interface IProfileRepository
    {
        public Task<JsonResult> GetProfiles(string? sortField, string? sortOrder);
        public Task<JsonResult> Create(ProfileVM profile);
        public Task<JsonResult> Update(ProfileVM profile);
        public Task<JsonResult> CheckUncheckAll(bool check);
        public Task<JsonResult> Delete(int id);

        public Task<JsonResult> GetProfileById(int? id);

        public Task<JsonResult> UpdateStatus(StatusVM status);
    }
}
