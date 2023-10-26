using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using AptitudeTest.Data.Common;
using APTITUDETEST.Common.Data;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Data.Data
{
    public class QuestionRepository : IQuestionRepository
    {
        #region Properties
        AppDbContext _context;
        #endregion

        #region Constructor
        public QuestionRepository(AppDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Methods

        public async Task<JsonResult> Create(QuestionVM question)
        {
            try
            {
                return new JsonResult(new ApiResponse<string>
                {
                    Message = string.Format(ResponseMessages.AddSuccess, "Question"),
                    Result = true,
                    StatusCode = ResponseStatusCode.Success
                });

            }

            catch (Exception ex)
            {
                return new JsonResult(new ApiResponse<string>
                {
                    Message = ResponseMessages.InternalError,
                    Result = false,
                    StatusCode = ResponseStatusCode.InternalServerError
                });
            }
        }

        #endregion
    }
}
