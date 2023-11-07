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
                if (id < (int)Enums.NumberCount.One)
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
                    if (data.Count() == (int)Enums.NumberCount.Zero)
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

        public async Task<JsonResult> GetQuestions(int? topic, bool? status, int pageSize, int pageIndex)
        {
            try
            {
                using (DbConnection connection = new DbConnection())
                {
                    if (pageSize < (int)Enums.NumberCount.One)
                    {
                        pageSize = (int)Enums.Pagination.DefaultPageSize;
                    }
                    if (pageIndex < (int)Enums.NumberCount.Zero)
                    {
                        pageIndex = (int)Enums.Pagination.DefaultIndex;
                    }
                    var parameter = new
                    {
                        filter_topic = topic == (int)Enums.QuestionTopic.Maths || topic == (int)Enums.QuestionTopic.Reasoning || topic == (int)Enums.QuestionTopic.Technical ? topic : null,
                        filter_status = status == true || status == false ? status : null,
                        page_size = pageSize,
                        page_index = pageIndex
                    };
                    var data = await connection.Connection.QueryAsync<QuestionDataVM>("select * from getAllQuestions(@filter_topic,@filter_status,@page_size,@page_index)", parameter);
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

                    int PageCount = (int)Enums.NumberCount.Zero;
                    int totalItemsCount = (int)Enums.NumberCount.Zero;
                    bool isNextPage = false;
                    var temp = data.FirstOrDefault();
                    if (temp != null)
                    {
                        PageCount = (int)temp?.TotalPages;
                        totalItemsCount = (int)temp?.TotalRecords;
                        isNextPage = temp?.NextPage == null ? false : true;
                    }
                    bool isPreviousPage = pageIndex == (int)Enums.NumberCount.Zero ? false : true;

                    PaginationVM<QuestionVM> pagination = new PaginationVM<QuestionVM>()
                    {
                        EntityList = questionVMList,
                        CurrentPageIndex = pageIndex,
                        PageSize = pageSize,
                        PageCount = PageCount,
                        IsNextPage = isNextPage,
                        IsPreviousPage = isPreviousPage,
                        TotalItemsCount = totalItemsCount,
                    };
                    return new JsonResult(new ApiResponse<PaginationVM<QuestionVM>>
                    {
                        Data = pagination,
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

        public async Task<JsonResult> GetQuestionCount(int? topic, bool? status)
        {
            try
            {
                QuestionCountVM questionCount = new QuestionCountVM();
                if (status == null)
                {
                    questionCount.TotalCount = _context.Questions.Where(q => q.IsDeleted != true).Count();
                    questionCount.MathsCount = _context.Questions.Where(q => q.IsDeleted != true && q.Topic == (int)Enums.QuestionTopic.Maths).Count();
                    questionCount.ReasoningCount = _context.Questions.Where(q => q.IsDeleted != true && q.Topic == (int)Enums.QuestionTopic.Reasoning).Count();
                    questionCount.TechnicalCount = _context.Questions.Where(q => q.IsDeleted != true && q.Topic == (int)Enums.QuestionTopic.Technical).Count();
                }
                else
                {
                    questionCount.TotalCount = _context.Questions.Where(q => q.IsDeleted != true && q.Status == status).Count();
                    questionCount.MathsCount = _context.Questions.Where(q => q.IsDeleted != true && q.Topic == (int)Enums.QuestionTopic.Maths && q.Status == status).Count();
                    questionCount.ReasoningCount = _context.Questions.Where(q => q.IsDeleted != true && q.Topic == (int)Enums.QuestionTopic.Reasoning && q.Status == status).Count();
                    questionCount.TechnicalCount = _context.Questions.Where(q => q.IsDeleted != true && q.Topic == (int)Enums.QuestionTopic.Technical && q.Status == status).Count();
                }
                return new JsonResult(new ApiResponse<QuestionCountVM>
                {
                    Data = questionCount,
                    Message = ResponseMessages.Success,
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

        public async Task<JsonResult> Create(QuestionVM questionVM)
        {

            try
            {
                if (questionVM.Id != (int)Enums.NumberCount.Zero || !ValidateQuestion(questionVM) || !ValidateOptionText(questionVM) || !ValidateOptionImages(questionVM))
                {
                    return new JsonResult(new ApiResponse<string>() { Message = ResponseMessages.BadRequest, Result = false, StatusCode = ResponseStatusCode.BadRequest });
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
            try
            {
                if (questionVM.Id < (int)Enums.NumberCount.One || !ValidateQuestion(questionVM) || !ValidateOptionText(questionVM) || questionVM.Options.Any(option => option.OptionId == 0))
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = ResponseMessages.BadRequest,
                        Result = false,
                        StatusCode = ResponseStatusCode.BadRequest
                    });
                }

                Question question = await Task.FromResult(_context.Questions.Where(question => question.Id == questionVM.Id).FirstOrDefault());
                if (question == null)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.NotFound, ModuleNames.Question),
                        Result = false,
                        StatusCode = ResponseStatusCode.NotFound
                    });
                }
                if (question.OptionType != (int)Enums.OptionType.ImageType && questionVM.OptionType == (int)Enums.OptionType.ImageType && !ValidateOptionImages(questionVM))
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = ResponseMessages.BadRequest,
                        Result = false,
                        StatusCode = ResponseStatusCode.BadRequest
                    });
                }
                question.QuestionText = questionVM.QuestionText.Trim();
                question.Status = questionVM.Status;
                question.Difficulty = questionVM.Difficulty;
                question.Topic = questionVM.TopicId;
                question.QuestionType = questionVM.QuestionType;
                question.OptionType = questionVM.OptionType;
                question.UpdatedBy = questionVM.UpdatedBy;
                question.UpdatedDate = DateTime.UtcNow;
                _context.Update(question);

                List<QuestionOptions> optionsList = await Task.FromResult(_context.QuestionOptions.Where(option => option.QuestionId == question.Id).OrderBy(option => option.Id).ToList());
                List<OptionVM> optionVMList = questionVM.Options.OrderBy(option => option.OptionId).ToList();
                for (int i = (int)Enums.NumberCount.Zero; i < (int)Enums.NumberCount.Four; i++)
                {
                    QuestionOptions questionOptions = optionsList[i];
                    OptionVM optionVM = optionVMList[i];
                    questionOptions.IsAnswer = optionVM.IsAnswer;
                    if (questionVM.OptionType == (int)Enums.OptionType.TextType)
                    {
                        questionOptions.OptionData = optionVM.OptionValue;
                    }
                    else
                    {
                        if (optionVM.OptionImage != null)
                        {
                            string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files");
                            string fileName = Guid.NewGuid().ToString() + "_" + optionVM.OptionImage.FileName;
                            string filePath = Path.Combine(uploadFolder, fileName);
                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                optionVM.OptionImage.CopyTo(fileStream);
                            }
                            questionOptions.OptionData = fileName;
                        }
                    }
                }
                _context.QuestionOptions.UpdateRange(optionsList);
                _context.SaveChanges();

                return new JsonResult(new ApiResponse<string>
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
                if (id < (int)Enums.NumberCount.One)
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
            if (questionVM.Options.Count() == (int)Enums.NumberCount.Four)
            {
                int answerCount = questionVM.Options.Where(option => option.IsAnswer == true).Count();
                if (questionVM.QuestionType == (int)Common.Enums.QuestionType.SingleAnswer && answerCount == (int)Enums.NumberCount.One)
                {
                    return true;
                }
                else if (questionVM.QuestionType == (int)Common.Enums.QuestionType.MultiAnswer && answerCount > (int)Enums.NumberCount.One)
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

        private bool ValidateOptionImages(QuestionVM questionVM)
        {
            if (questionVM.OptionType == (int)Common.Enums.OptionType.ImageType)
            {
                return !questionVM.Options.Any(option => option.OptionImage == null);
            }
            return true;
        }

        private bool DoesQuestionExists(QuestionVM questionVM)
        {
            Question question = _context.Questions.Where(question => question.Topic == questionVM.TopicId && question.Difficulty == questionVM.Difficulty && question.QuestionText.Trim().ToLower() == questionVM.QuestionText.Trim().ToLower() && question.QuestionType == questionVM.QuestionType && question.OptionType == questionVM.OptionType && question.IsDeleted != true).FirstOrDefault();
            if (question != null)
            {
                if (questionVM.OptionType == (long)Common.Enums.QuestionType.MultiAnswer)
                {
                    if (questionVM.DuplicateFromQuestionId != (int)Enums.NumberCount.Zero && questionVM.Options.Where(option => option.OptionImage != null).Count() == (int)Enums.NumberCount.Zero)
                    {
                        return true;
                    }
                    return false;
                }

                List<QuestionOptions> options = _context.QuestionOptions.Where(option => option.QuestionId == question.Id).OrderBy(option => option.Id).ToList();
                bool[] flag = new bool[] { false, false, false, false };
                for (int i = (int)Enums.NumberCount.Zero; i < (int)Enums.NumberCount.Four; i++)
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
