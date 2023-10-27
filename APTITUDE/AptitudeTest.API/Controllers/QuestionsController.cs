using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using AptitudeTest.Data.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QuestionsController : ControllerBase
    {
        #region Properties
        private readonly IQuestionService _service;
        #endregion

        #region Constructor
        public QuestionsController(IQuestionService service)
        {
            _service = service;
        }
        #endregion

        #region Methods
        /// <summary>
        /// This method Adds new College
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<JsonResult> Create([FromForm] QuestionVM question)
        {
            if (ModelState.IsValid)
            {
                return await _service.Create(question);
            }
            return new JsonResult(new ApiResponse<string>() { Message = ResponseMessages.BadRequest, Result = false, StatusCode = ResponseStatusCode.BadRequest });
        }

        /// <summary>
        /// This method gives Question from its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("[action]{id:int}")]
        public async Task<JsonResult> Get(int id)
        {
            return await _service.Get(id);
        }
        #endregion
    }
}
