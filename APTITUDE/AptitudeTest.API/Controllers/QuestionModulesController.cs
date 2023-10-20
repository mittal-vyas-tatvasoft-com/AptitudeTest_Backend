using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using AptitudeTest.Data.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QuestionModulesController : ControllerBase
    {
        #region Properties
        private readonly IQuestionModuleService _service;
        #endregion

        #region Constructor
        public QuestionModulesController(IQuestionModuleService service)
        {
            _service = service;
        }
        #endregion

        #region Methods

        /// <summary>
        /// This gives List of QuestionModules with searching,filtering and pagination
        /// </summary>
        /// <param name="searchQuery">Word that we want to search</param>
        /// <param name="filter">Filter QuestionModules on status 1 for Active  2 for Inactive </param>
        /// <param name="currentPageIndex">Page index which is page number-1</param>
        /// <param name="pageSize">Length of records in 1 page</param>
        /// <param name="searchQuery">word that we want to search</param>
        /// <returns>filtered list of QuestionModules</returns>
        [HttpGet]
        public async Task<JsonResult> GetQuestionModules(string? searchQuery, int? filter, int? currentPageIndex, int? pageSize)
        {
            return await _service.GetQuestionModules(searchQuery, filter, currentPageIndex, pageSize);
        }

        /// <summary>
        /// This method Creates QuestionModule
        /// </summary>
        /// <param name="questionModule"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<JsonResult> Create(QuestionModuleVM questionModule)
        {
            if (ModelState.IsValid)
            {
                return await _service.Create(questionModule);
            }

            return new JsonResult(new ApiResponse<string>() { Message = ResponseMessages.BadRequest, Result = false, StatusCode = ResponseStatusCode.BadRequest });
        }

        /// <summary>
        /// This method Updates QuestionModule
        /// </summary>
        /// <param name="QuestionModule"></param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public async Task<JsonResult> Update(QuestionModuleVM questionModule)
        {
            if (ModelState.IsValid)
            {
                return await _service.Update(questionModule);
            }
            return new JsonResult(new ApiResponse<string>() { Message = ResponseMessages.BadRequest, Result = false, StatusCode = ResponseStatusCode.BadRequest });
        }

        /// <summary>
        /// This method gives QuestionModule by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("[action]/{id:int}")]
        public async Task<JsonResult> Get(int id)
        {
            return await _service.Get(id);
        }

        /// <summary>
        /// This method soft deletes QuestionModule
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("[action]")]
        public async Task<JsonResult> Delete(int id)
        {
            return await _service.Delete(id);
        }


        #endregion
    }

}

