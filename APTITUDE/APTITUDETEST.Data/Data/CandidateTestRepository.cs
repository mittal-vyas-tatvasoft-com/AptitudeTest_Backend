using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using AptitudeTest.Data.Common;
using APTITUDETEST.Common.Data;
using Dapper;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Data.Data
{
    public class CandidateTestRepository : ICandidateTestRepository
    {
        #region Properties
        private readonly AppDbContext _context;
        #endregion

        #region Constructor
        public CandidateTestRepository(AppDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Methods
        public async Task<JsonResult> GetCandidateTestQuestion(int questionId, int userId, int testId)
        {

            try
            {
                if ((questionId != (int)Enums.DefaultQuestionId.QuestionId && questionId < 1) || testId < 1)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = ResponseMessages.BadRequest,
                        Result = false,
                        StatusCode = ResponseStatusCode.BadRequest
                    });
                }

                using (DbConnection connection = new DbConnection())
                {
                    int userTestId = _context.TempUserTests.Where(x => x.UserId == userId && x.TestId == testId).Select(x => x.Id).FirstOrDefault();
                    if (userTestId == null || userTestId == 0)
                    {
                        return new JsonResult(new ApiResponse<string>
                        {
                            Message = ResponseMessages.BadRequest,
                            Result = false,
                            StatusCode = ResponseStatusCode.BadRequest
                        });
                    }
                    var questions = _context.TempUserTestResult.Where(t => t.UserTestId == userTestId).Select(x => x.QuestionId).ToList();
                    if (questionId == (int)Enums.DefaultQuestionId.QuestionId)
                    {
                        questionId = questions.FirstOrDefault();
                    }
                    if (questions.IndexOf(questionId) == -1)
                    {
                        return new JsonResult(new ApiResponse<string>
                        {
                            Message = ResponseMessages.BadRequest,
                            Result = false,
                            StatusCode = ResponseStatusCode.BadRequest
                        });
                    }
                    int nextIndex = questions.IndexOf(questionId) + 1;
                    int nextQuestionId = 0;
                    int questionNumber = nextIndex;
                    if (nextIndex < questions.Count)
                    {
                        nextQuestionId = questions[nextIndex];
                    }

                    var data = await connection.Connection.QueryAsync<QuestionDataVM>("select * from getQuestionbyid(@question_id)", new { question_id = questionId });
                    if (data.Count() == 0)
                    {
                        return new JsonResult(new ApiResponse<UserDetailsVM>
                        {
                            Data = null,
                            Message = string.Format(ResponseMessages.NotFound, ModuleNames.Question),
                            Result = false,
                            StatusCode = ResponseStatusCode.NotFound
                        });
                    }
                    var question = data.FirstOrDefault();
                    CandidateTestQuestionVM candidateTestQuestionVM = new CandidateTestQuestionVM()
                    {
                        Id = question.QuestionId,
                        Difficulty = question.Difficulty,
                        OptionType = question.OptionType,
                        QuestionType = question.QuestionType,
                        QuestionText = question.QuestionText,
                        NextQuestionId = nextQuestionId,
                        QuestionNumber = questionNumber,
                        TotalQuestions = questions.Count
                    };

                    foreach (var item in data)
                    {
                        candidateTestQuestionVM.Options.Add(item.OptionData);
                    }
                    return new JsonResult(new ApiResponse<CandidateTestQuestionVM>
                    {
                        Data = candidateTestQuestionVM,
                        Message = ResponseMessages.Success,
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }

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
    }
    #endregion
}

