using AptitudeTest.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ResultsController : ControllerBase
    {
        #region Properties
        private readonly IResultService _service;
        #endregion

        #region Constructor
        public ResultsController(IResultService service)
        {
            _service = service;
        }
        #endregion

        #region Methods
        /// <summary>
        /// It gives User Result
        /// </summary>
        /// <param name="id"></param>
        /// <param name="marks"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<JsonResult> Get(int id, int marks, int pageSize, int pageIndex)
        {
            return await _service.Get(id, marks, pageSize, pageIndex);
        }
        #endregion
    }
}
