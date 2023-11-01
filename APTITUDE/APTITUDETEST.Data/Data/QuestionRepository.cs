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
                            Message = string.Format(ResponseMessages.NotFound, ModuleNames.Question),
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

        public async Task<JsonResult> GetQuestions(int? topic, bool? status)
        {
            try
            {
                using (DbConnection connection = new DbConnection())
                {
                    var parameter = new
                    {
                        filter_topic = topic,
                        filter_status = status
                    };
                    var data = await connection.Connection.QueryAsync<QuestionDataVM>("select * from getAllQuestions(@filter_topic,@filter_status)", parameter);
                    List<QuestionVM> questionVMList = new List<QuestionVM>();
                    questionVMList = data.GroupBy(question => question.QuestionId).Select(
                        x =>
                        {
                            var q = x.FirstOrDefault();
                            return new QuestionVM()
                            {
                                Id = q.QuestionId,
                                Difficulty = q.Difficulty,
                                OptionType = q.OptionType,
                                QuestionText = q.QuestionText,
                                QuestionType = q.QuestionType,
                                Status = q.Status,
                                TopicId = q.Topic,
                                Options = x.Select(x =>
                                {
                                    return new OptionVM()
                                    {
                                        IsAnswer = x.IsAnswer,
                                        OptionId = x.OptionId,
                                        OptionValue = x.OptionData
                                    };
                                }).ToList()
                            };
                        }
                        ).ToList();

                    return new JsonResult(new ApiResponse<List<QuestionVM>>
                    {
                        Data = questionVMList,
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

        public async Task<JsonResult> Create(QuestionVM questionVM)
        {

            try
            {
                if (questionVM.Id != 0 || !ValidateQuestion(questionVM) || (questionVM.OptionType == (int)Common.Enums.OptionType.ImageType && !ValidateImageExtension(questionVM.Options, true)) || !ValidateOptionText(questionVM))
                {
                    return new JsonResult(new ApiResponse<string>() { Message = ResponseMessages.BadRequest, Result = false, StatusCode = ResponseStatusCode.BadRequest });
                }

                if (DoesQuestionExists(questionVM))
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.AlreadyExists, ModuleNames.Question),
                        Result = false,
                        StatusCode = ResponseStatusCode.AlreadyExist
                    });
                }
                Question question = new Question();
                question.QuestionText = questionVM.QuestionText.Trim();
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
                    if (questionVM.OptionType == (int)Common.Enums.OptionType.TextType)
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
                    Message = string.Format(ResponseMessages.AddSuccess, ModuleNames.Question),
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

        public async Task<JsonResult> Update(QuestionVM questionVM)
        {
            return null;
        }

        public async Task<JsonResult> UpdateStatus(StatusVM status)
        {
            try
            {
                Question question = await Task.FromResult(_context.Questions.Where(question => question.IsDeleted != true && question.Id == status.Id).FirstOrDefault());
                if (question == null)
                {
                    return new JsonResult(new ApiResponse<int>
                    {
                        Message = string.Format(ResponseMessages.NotFound, ModuleNames.Question),
                        Result = true,
                        StatusCode = ResponseStatusCode.NotFound
                    });
                }

                question.Status = status.Status;
                _context.Update(question);
                _context.SaveChanges();

                return new JsonResult(new ApiResponse<int>
                {
                    Message = string.Format(ResponseMessages.UpdateSuccess, ModuleNames.Question),
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

        public async Task<JsonResult> Delete(int id)
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

                Question question = await Task.FromResult(_context.Questions.Where(d => d.Id == id && d.IsDeleted != true).FirstOrDefault());
                if (question != null)
                {
                    question.IsDeleted = true;
                    _context.Questions.Update(question);
                    List<QuestionOptions> questionOptions = _context.QuestionOptions.Where(option => option.QuestionId == question.Id).ToList();
                    questionOptions.ForEach(option => option.IsDeleted = true);
                    _context.UpdateRange(questionOptions);
                    _context.SaveChanges();
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.DeleteSuccess, ModuleNames.Question),
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }
                return new JsonResult(new ApiResponse<string>
                {
                    Message = string.Format(ResponseMessages.NotFound, ModuleNames.Question),
                    Result = false,
                    StatusCode = ResponseStatusCode.NotFound
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

        #region Helper Method

        private bool ValidateQuestion(QuestionVM questionVM)
        {
            if (questionVM.Options.Count() == 4)
            {
                int answerCount = questionVM.Options.Where(option => option.IsAnswer == true).Count();
                if (questionVM.QuestionType == (int)Common.Enums.QuestionType.SingleAnswer && answerCount == 1)
                {
                    return true;
                }
                else if (questionVM.QuestionType == (int)Common.Enums.QuestionType.MultiAnswer && answerCount > 1)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        private bool ValidateOptionText(QuestionVM questionVM)
        {
            bool result = true;
            if (questionVM.OptionType == (int)Common.Enums.OptionType.TextType)
            {
                questionVM.Options.ForEach(option =>
                {
                    if (option.OptionValue == null || option.OptionValue?.Trim() == "")
                    {
                        result = false;
                    }
                });
            }
            return result;
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

        private bool DoesQuestionExists(QuestionVM questionVM)
        {
            Question question = _context.Questions.Where(question => question.Topic == questionVM.TopicId && question.Difficulty == questionVM.Difficulty && question.QuestionText.Trim().ToLower() == questionVM.QuestionText.Trim().ToLower() && question.QuestionType == questionVM.QuestionType && question.OptionType == questionVM.OptionType && question.IsDeleted != true).FirstOrDefault();
            if (question != null)
            {
                if (questionVM.OptionType == (long)Common.Enums.QuestionType.MultiAnswer)
                {
                    if (questionVM.DuplicateFromQuestionId != 0 && questionVM.Options.Where(option => option.OptionImage != null).Count() == 0)
                    {
                        return true;
                    }
                    return false;
                }

                List<QuestionOptions> options = _context.QuestionOptions.Where(option => option.QuestionId == question.Id).OrderBy(option => option.Id).ToList();
                bool[] flag = new bool[] { false, false, false, false };
                for (int i = 0; i < 4; i++)
                {
                    var option = options[i];
                    foreach (var questionOption in questionVM.Options)
                    {
                        if (questionOption.IsAnswer == option.IsAnswer && questionOption.OptionValue?.Trim().ToLower() == option.OptionData.Trim().ToLower())
                        {
                            flag[i] = true;
                            break;
                        }
                    }
                    if (flag[i] == false) { break; }

                }
                return flag.All(x => x);
            }
            return false;
        }

        #endregion
    }
}
