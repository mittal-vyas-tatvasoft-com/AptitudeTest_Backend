using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Core.Interfaces
{
    public interface IGroupService
    {
        public Task<JsonResult> GetActiveGroups();
    }
}
