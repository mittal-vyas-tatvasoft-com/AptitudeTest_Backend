using AptitudeTest.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Data.Data
{
    public class ResultRepository : IResultRepository
    {
        public async Task<JsonResult> Get(int id, int marks, int pageSize, int pageIndex)
        {
            return null;
        }
    }
}
