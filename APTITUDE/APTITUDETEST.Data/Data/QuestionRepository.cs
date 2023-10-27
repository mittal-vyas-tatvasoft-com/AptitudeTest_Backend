using AptitudeTest.Core.Entities.Questions;
using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using AptitudeTest.Data.Common;
using APTITUDETEST.Common.Data;
using Dapper;
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

        public async Task<JsonResult> Create(QuestionVM questionVM)
        {

            try
            {
                if (!ValidateQuestion(questionVM) || !ValidateImageExtension(questionVM.Options, true))
                {
                    return new JsonResult(new ApiResponse<string>() { Message = ResponseMessages.BadRequest, Result = false, StatusCode = ResponseStatusCode.BadRequest });
                }

                Question question = new Question();
                question.Id = questionVM.Id;
                question.QuestionText = questionVM.QuestionText;
                question.Status = questionVM.Status;
                question.Difficulty = questionVM.Difficulty;
                question.Topic = questionVM.TopicId;
                question.QuestionType = questionVM.QuestionType;
                question.OptionType = questionVM.OptionType;
                question.CreatedBy = questionVM.CreatedBy;
                _context.Add(question);
                _context.SaveChanges();

                int questionId = _context.Questions.OrderByDescending(q => q.CreatedDate).FirstOrDefault().Id;

                List<QuestionOptions> questionOptions = new List<QuestionOptions>();
                foreach (var option in questionVM.Options)
                {
                    QuestionOptions options = new QuestionOptions();
                    options.QuestionId = questionId;
                    options.IsAnswer = option.IsAnswer;
                    // OptionType 1 refers to string option value and 2 refers to image option value
                    if (questionVM.OptionType == 1)
                    {
                        options.OptionData = option.OptionValue;
                    }
                    else
                    {
                        string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files");
                        string fileName = Guid.NewGuid().ToString() + "_" + option.OptionImage.FileName;
                        string filePath = Path.Combine(uploadFolder, fileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            option.OptionImage.CopyTo(fileStream);
                        }
                        options.OptionData = fileName;
                    }
                    questionOptions.Add(options);
                }
                await _context.QuestionOptions.AddRangeAsync(questionOptions);
                _context.SaveChanges();
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

        public async Task<JsonResult> Get(int id)
        {
            try
            {
                if (id < 1)
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
                    var data = await connection.Connection.QueryAsync<QuestionDataVM>("select * from getQuestionbyid(@question_id)", new { question_id = id });
                    if (data.Count() == 0)
                    {
                        return new JsonResult(new ApiResponse<UserDetailsVM>
                        {
                            Data = null,
                            Message = string.Format(ResponseMessages.NotFound, "Question"),
                            Result = false,
                            StatusCode = ResponseStatusCode.NotFound
                        });
                    }

                    var question = data.FirstOrDefault();
                    QuestionVM questionVM = new QuestionVM()
                    {
                        Id = question.QuestionId,
                        TopicId = question.Topic,
                        Difficulty = question.Difficulty,
                        OptionType = question.OptionType,
                        QuestionType = question.QuestionType,
                        Status = question.Status,
                        QuestionText = question.QuestionText,
                    };
                    foreach (var item in data)
                    {
                        OptionVM optionVM = new OptionVM()
                        {
                            OptionId = item.OptionId,
                            IsAnswer = item.IsAnswer,
                            OptionValue = item.OptionData
                        };
                        questionVM.Options.Add(optionVM);
                    }
                    return new JsonResult(new ApiResponse<QuestionVM>
                    {
                        Data = questionVM,
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

        #endregion

        #region Helper Method

        private bool ValidateQuestion(QuestionVM questionVM)
        {
            if (questionVM.Options.Count() == 4)
            {
                int answerCount = questionVM.Options.Where(option => option.IsAnswer == true).Count();
                if (questionVM.QuestionType == 1 && answerCount == 1)
                {
                    return true;
                }
                else if (questionVM.QuestionType == 2 && answerCount > 1)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        private bool ValidateImageExtension(List<OptionVM> options, bool strictCheck)
        {
            List<string> extenaions = options.Select(x => x.OptionImage?.ContentType).ToList();
            List<string> acceptableExtensions = acceptableExtensions = new List<string>() { "image/png", "image/jpg", "image/jpeg" };
            if (!strictCheck)
            {
                acceptableExtensions.Add(null);
            }
            return extenaions.Except(acceptableExtensions).ToArray().Length == 0;
        }

        #endregion
    }
}
