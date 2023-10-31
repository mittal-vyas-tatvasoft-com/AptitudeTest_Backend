using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Core.Interfaces
{
    public interface IGroupRepository
    {
        public Task<JsonResult> GetActiveGroups();
    }
}
