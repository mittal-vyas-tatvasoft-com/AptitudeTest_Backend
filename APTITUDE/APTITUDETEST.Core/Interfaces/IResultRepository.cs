using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Core.Interfaces
{
    public interface IResultRepository
    {
        public Task<JsonResult> Get(int id, int marks, int pageSize, int pageIndex);
    }
}
