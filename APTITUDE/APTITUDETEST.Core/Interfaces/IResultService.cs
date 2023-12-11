using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Core.Interfaces
{
    public interface IResultService
    {
        public Task<JsonResult> Get(int id, int marks, int pageSize, int pageIndex);
    }
}
