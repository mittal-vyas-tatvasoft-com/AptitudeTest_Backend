using AptitudeTest.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Core.Interfaces
{
    public interface IDegreeRepository
    {
        public Task<JsonResult> GetActiveDegrees();
        public Task<JsonResult> GetDegrees(string? sortField, string? sortOrder);
        public Task<JsonResult> Get(int id);

        public Task<JsonResult> Create(DegreeVM degree);
        public Task<JsonResult> Update(DegreeVM degree);
        public Task<JsonResult> UpdateStatus(StatusVM status);

        public Task<JsonResult> CheckUncheckAll(bool check);

        public Task<JsonResult> Delete(int id);
    }
}
