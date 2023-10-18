using AptitudeTest.Core.Entities.Questions;
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
    public class QuestionMarksController : ControllerBase
    {
        #region Properties
        private readonly IQuestionMarksService _service;
        #endregion

        #region Constructor
        public QuestionMarksController(IQuestionMarksService service)
        {
            _service = service;
        }
        #endregion

        #region Methods

        /// <summary>
        /// This gives List of QuestionMarks with searching,filtering and pagination
        /// </summary>
        /// <param name="searchQuery">Word that we want to search</param>
        /// <param name="currentPageIndex">Page index which is page number-1</param>
        /// <param name="pageSize">Length of records in 1 page</param>
        /// <param name="searchQuery">word that we want to search</param>
        /// <returns>filtered list of QuestionMarks</returns>
        [HttpGet("{currentPageIndex:int}/{pageSize:int}")]
        public async Task<JsonResult> GetAllQuestionMarks(string? searchQuery, int? currentPageIndex = 0, int? pageSize = 10)
        {
            return await _service.GetAllQuestionMarks(searchQuery, currentPageIndex, pageSize);
        }

        /// <summary>
        /// This method Creates QuestionMarks
        /// </summary>
        /// <param name="newMark"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<JsonResult> Create(QuestionMarks newMark)
        {
            if (ModelState.IsValid)
            {
                return await _service.Create(newMark);
            }

            return new JsonResult(new ApiResponse<string>() { Message = ResponseMessages.BadRequest, Result = false, StatusCode = ResponseStatusCode.BadRequest });
        }

        /// <summary>
        /// This method Updates QuestionMarks
        /// </summary>
        /// <param name="updatedMark"></param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public async Task<JsonResult> Update(QuestionMarks updatedMark)
        {
            if (ModelState.IsValid)
            {
                return await _service.Update(updatedMark);
            }
            return new JsonResult(new ApiResponse<string>() { Message = ResponseMessages.BadRequest, Result = false, StatusCode = ResponseStatusCode.BadRequest });
        }

        /// <summary>
        /// This method soft deletes QuestionMarks
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
