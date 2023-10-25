using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using AptitudeTest.Data.Common;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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

        [HttpPost("[action]")]
        public async Task<JsonResult> Create([FromForm] QuestionVM question)
        {
            if (ModelState.IsValid)
            {
                return await _service.Create(question);
            }
            return new JsonResult(new ApiResponse<string>() { Message = ResponseMessages.BadRequest, Result = false, StatusCode = ResponseStatusCode.BadRequest });

        }

        #endregion
    }
}
