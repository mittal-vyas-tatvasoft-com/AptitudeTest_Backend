using AptitudeTest.Core.ViewModels.Master;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Core.Interfaces.Master
{
    public interface ITechnologyRepository
    {
        public Task<JsonResult> GetTechnologies(string? searchQuery, int? filter, int? currentPageIndex, int? pageSize);

        public Task<JsonResult> Upsert(TechnologyVM technology);

        public Task<JsonResult> CheckUncheckAll(bool check);

        public Task<JsonResult> Delete(int id);
    }
}
