using AptitudeTest.Common.Data;
using AptitudeTest.Core.Entities.Questions;
using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using AptitudeTest.Data.Common;
using APTITUDETEST.Common.Data;
using CsvHelper;
using CsvHelper.Configuration;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web;
using static AptitudeTest.Data.Common.Enums;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AptitudeTest.Data.Data
{
    public class QuestionRepository : IQuestionRepository
    {
        #region Properties
        readonly AppDbContext _context;
        private readonly DapperAppDbContext _dapperContext;
        private readonly ILoggerManager _logger;
        #endregion

        #region Constructor
        public QuestionRepository(AppDbContext context, DapperAppDbContext dapperContext, ILoggerManager logger)
        {
            _context = context;
            _dapperContext = dapperContext;
            _logger = logger;
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
                    if (!data.Any())
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
                    QuestionVM questionVM = new QuestionVM();
                    if (question != null)
                    {
                        questionVM.Id = question.QuestionId;
                        questionVM.TopicId = question.Topic;
                        questionVM.Difficulty = question.Difficulty;
                        questionVM.OptionType = question.OptionType;
                        questionVM.QuestionType = question.QuestionType;
                        questionVM.Status = question.Status;
                        questionVM.QuestionText = HttpUtility.HtmlDecode(question.QuestionText);
                        questionVM.ParentId = question.ParentId;
                    }
                    foreach (var item in data)
                    {
                        OptionVM optionVM = new OptionVM()
                        {
                            OptionId = item.OptionId,
                            IsAnswer = item.IsAnswer,
                            OptionValue = question.OptionType == (int)OptionType.TextType ? HttpUtility.HtmlDecode(item.OptionData) : item.OptionData
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
                _logger.LogError($"Error occurred in QuestionRepository.Get:{ex}");
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
                    if (pageSize < 1)
                    {
                        pageSize = (int)Enums.Pagination.DefaultPageSize;
                    }
                    if (pageIndex < (int)Enums.Pagination.DefaultIndex)
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
                    var data = await connection.Connection.QueryAsync<QuestionDataVM>("select * from getallquestions(@filter_topic,@filter_status,@page_size,@page_index)", parameter);
                    if (data != null)
                    {
                        List<QuestionVM> questionVMList;
                        questionVMList = data.GroupBy(question => question.QuestionId).Select(
                            x =>
                            {
                                var q = x.FirstOrDefault();

                                return new QuestionVM()
                                {
                                    Sequence = q.Sequence,
                                    Id = q.QuestionId,
                                    Difficulty = q.Difficulty,
                                    OptionType = q.OptionType,
                                    QuestionText = HttpUtility.HtmlDecode(q.QuestionText),
                                    QuestionType = q.QuestionType,
                                    Status = q.Status,
                                    TopicId = q.Topic,
                                    ParentId = q.ParentId,
                                    Options = x.Select(x =>
                                    {
                                        return new OptionVM()
                                        {
                                            IsAnswer = x.IsAnswer,
                                            OptionId = x.OptionId,
                                            OptionValue = HttpUtility.HtmlDecode(x.OptionData)
                                        };
                                    }).OrderBy(option => option.OptionId).ToList()
                                };
                            }
                            ).ToList();

                        int PageCount = 0;
                        int totalItemsCount = 0;
                        bool isNextPage = false;
                        var temp = data.FirstOrDefault();
                        if (temp != null)
                        {
                            PageCount = (int)temp?.TotalPages;
                            totalItemsCount = (int)temp?.TotalRecords;
                            isNextPage = IsNextPage(temp);
                        }
                        bool isPreviousPage = IsPreviousPage(pageIndex);

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
                    else
                    {
                        return new JsonResult(new ApiResponse<string>
                        {
                            Message = string.Format(ResponseMessages.NotFound, ModuleNames.Question),
                            Result = false,
                            StatusCode = ResponseStatusCode.NotFound
                        });
                    }

                }
            }

            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in QuestionRepository.GetQuestions:{ex}");
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
                using (DbConnection connection = new DbConnection())
                {
                    List<TestTopicWiseCountVM> data = connection.Connection.Query<TestTopicWiseCountVM>("Select * from gettopicwisequestionscountforquestionmodule(@filterStatus)", new { filterStatus = status }).ToList();
                    questionCount.TotalCount = data.Sum(x =>x.CountOfQuestions);
                    questionCount.MathsCount = data.Where(x => x.TopicId == (int)QuestionTopic.Maths).Select(x=>x.CountOfQuestions).FirstOrDefault();
                    questionCount.ReasoningCount = data.Where(x=>x.TopicId==(int)QuestionTopic.Reasoning).Select(x => x.CountOfQuestions).FirstOrDefault();
                    questionCount.TechnicalCount = data.Where(x => x.TopicId == (int)QuestionTopic.Technical).Select(x => x.CountOfQuestions).FirstOrDefault();
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
                _logger.LogError($"Error occurred in QuestionRepository.GetQUestionCount:{ex}");
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
                if (CheckOptions(questionVM.Options))
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = ResponseMessages.InvalidOptions,
                        Result = false,
                        StatusCode = ResponseStatusCode.BadRequest
                    });
                }

                List<int> duplicateOptionIds = new List<int>();
                List<QuestionOptions> duplicateOptions = new List<QuestionOptions>();
                if (IsQuestionVMInvalid(questionVM))
                {
                    return new JsonResult(new ApiResponse<string>() { Message = ResponseMessages.BadRequest, Result = false, StatusCode = ResponseStatusCode.BadRequest });
                }
                if (questionVM.DuplicateFromQuestionId != 0)
                {
                    Question? duplicateQuestion = _context.Questions.Where(x => x.Id == questionVM.DuplicateFromQuestionId).FirstOrDefault();
                    if (IsDuplicateQuestionInvalid(duplicateQuestion, questionVM))
                    {
                        return new JsonResult(new ApiResponse<string>
                        {
                            Message = ResponseMessages.BadRequest,
                            Result = false,
                            StatusCode = ResponseStatusCode.BadRequest
                        });
                    }
                    questionVM.TopicId = duplicateQuestion.Topic;
                    duplicateOptionIds = questionVM.Options.Select(x => x.OptionId).ToList();
                    duplicateOptions = _context.QuestionOptions.Where(option => duplicateOptionIds.Contains(option.Id)).ToList();

                }
                Question question = new Question();
                question.QuestionText = HttpUtility.HtmlEncode(questionVM.QuestionText.Trim());
                question.Status = questionVM.Status;
                question.Difficulty = questionVM.Difficulty;
                question.Topic = questionVM.TopicId;
                question.QuestionType = questionVM.QuestionType;
                question.OptionType = questionVM.OptionType;
                question.CreatedBy = questionVM.CreatedBy;
                if (questionVM.DuplicateFromQuestionId != 0)
                {
                    question.ParentId = questionVM.DuplicateFromQuestionId;
                }
                string sequence = GenerateSequence(questionVM.DuplicateFromQuestionId);
                question.Sequence = sequence;
                _context.Add(question);
                _context.SaveChanges();

                Question? lastCreadtedQuestion = _context.Questions.OrderByDescending(q => q.CreatedDate).FirstOrDefault();
                int questionId = 0;
                if (lastCreadtedQuestion != null)
                {
                    questionId = lastCreadtedQuestion.Id;
                }

                List<QuestionOptions> questionOptions = new List<QuestionOptions>();

                foreach (var option in questionVM.Options)
                {
                    QuestionOptions options = new QuestionOptions();
                    options.QuestionId = questionId;
                    options.IsAnswer = option.IsAnswer;
                    if (IsOptionValid(questionVM, option))
                    {
                        options.OptionData = HttpUtility.HtmlEncode(option.OptionValue);
                    }
                    else
                    {
                        if (option.OptionImage != null)
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
                        else
                        {
                            options.OptionData = duplicateOptions.Find(x => x.Id == option.OptionId).OptionData;
                        }

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
                _logger.LogError($"Error occurred in QuestionRepository.Create:{ex}");
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
                if (CheckOptions(questionVM.Options))
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = ResponseMessages.InvalidOptions,
                        Result = false,
                        StatusCode = ResponseStatusCode.BadRequest
                    });
                }

                if (questionVM.Id < 1 || !ValidateQuestion(questionVM) || !ValidateOptionText(questionVM) || questionVM.Options.Exists(option => option.OptionId == 0))
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = ResponseMessages.BadRequest,
                        Result = false,
                        StatusCode = ResponseStatusCode.BadRequest
                    });
                }
                Question? question = await Task.FromResult(_context.Questions.Where(question => question.Id == questionVM.Id).FirstOrDefault());
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
                question.QuestionText = HttpUtility.HtmlEncode(questionVM.QuestionText.Trim());
                question.Status = questionVM.Status;
                question.Difficulty = questionVM.Difficulty;
                question.QuestionType = questionVM.QuestionType;
                question.OptionType = questionVM.OptionType;
                question.UpdatedBy = questionVM.UpdatedBy;
                question.UpdatedDate = DateTime.UtcNow;
                if (question.ParentId == null || question.ParentId == 0)
                {
                    question.Topic = questionVM.TopicId;
                    List<Question> childQuestions = _context.Questions.Where(q => q.ParentId == question.Id).ToList();
                    childQuestions.ForEach(q => { q.Topic = questionVM.TopicId; q.Difficulty = questionVM.Difficulty; q.QuestionType = questionVM.QuestionType; });
                    _context.UpdateRange(childQuestions);
                }
                _context.Update(question);

                List<QuestionOptions> optionsList = await Task.FromResult(_context.QuestionOptions.Where(option => option.QuestionId == question.Id).OrderBy(option => option.Id).ToList());
                List<OptionVM> optionVMList = questionVM.Options.OrderBy(option => option.OptionId).ToList();
                for (int i = 0; i < 4; i++)
                {
                    QuestionOptions questionOptions = optionsList[i];
                    OptionVM optionVM = optionVMList[i];
                    questionOptions.IsAnswer = optionVM.IsAnswer;
                    if (questionVM.OptionType == (int)Enums.OptionType.TextType)
                    {
                        questionOptions.OptionData = HttpUtility.HtmlEncode(optionVM.OptionValue);
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
                _logger.LogError($"Error occurred in QuestionRepository.Update:{ex} for Id:{questionVM.Id}");
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
                Question? question = await Task.FromResult(_context.Questions.Where(question => question.IsDeleted != true && question.Id == status.Id).FirstOrDefault());
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
                _logger.LogError($"Error occurred in QuestionRepository.UpdateStatus:{ex}");
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
                Question? question = await Task.FromResult(_context.Questions.Where(d => d.Id == id && d.IsDeleted != true).FirstOrDefault());
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
                _logger.LogError($"Error occurred in QuestionRepository.Delete:{ex} for Id:{id}");
                return new JsonResult(new ApiResponse<string>
                {
                    Message = ResponseMessages.InternalError,
                    Result = false,
                    StatusCode = ResponseStatusCode.InternalServerError
                });
            }

        }

        public async Task<JsonResult> ImportQuestions(ImportQuestionVM importQuestionVM)
        {
            try
            {
                List<ImportQuestionCsvVM> data = ReadCsvFile(importQuestionVM.File);
                if (data == null || !data.Any())
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.InvalidFormat
                        ),
                        Result = false,
                        StatusCode = ResponseStatusCode.BadRequest
                    });
                }
                List<ImportQuestionFieldsVM> importQuestionFieldsVMList = new List<ImportQuestionFieldsVM>();
                importQuestionFieldsVMList = data.Select(x => new ImportQuestionFieldsVM
                {
                    correctoption = x.correctoption,
                    difficulty = x.difficulty,
                    optiondata1 = HttpUtility.HtmlEncode(x.optiondata1),
                    optiondata2 = HttpUtility.HtmlEncode(x.optiondata2),
                    optiondata3 = HttpUtility.HtmlEncode(x.optiondata3),
                    optiondata4 = HttpUtility.HtmlEncode(x.optiondata4),
                    questiontext = HttpUtility.HtmlEncode(x.questiontext),
                    quetionnumber = x.quetionnumber,
                    version = x.version,
                    topic = x.topic,
                    status = x.status
                }).ToList();
                if (importQuestionFieldsVMList.Count == 0)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.NoRecordsFound),
                        Result = false,
                        StatusCode = ResponseStatusCode.BadRequest
                    });
                }

                foreach (var item in importQuestionFieldsVMList)
                {
                    FillImportQuestionSequence(importQuestionFieldsVMList);
                    QuestionVM questionVM = new QuestionVM();
                    List<OptionVM> options = new List<OptionVM>();
                    questionVM.Options = options;
                    questionVM.QuestionType = item.questiontype;
                    options.Add(new OptionVM() { IsAnswer = item.isanswer1, OptionValue = item.optiondata1 });
                    options.Add(new OptionVM() { IsAnswer = item.isanswer2, OptionValue = item.optiondata2 });
                    options.Add(new OptionVM() { IsAnswer = item.isanswer3, OptionValue = item.optiondata3 });
                    options.Add(new OptionVM() { IsAnswer = item.isanswer4, OptionValue = item.optiondata4 });

                    if (!ValidateQuestion(questionVM))
                    {
                        return new JsonResult(new ApiResponse<string>
                        {
                            Message = ResponseMessages.InvalidAnswerSelection,
                            Result = false,
                            StatusCode = ResponseStatusCode.BadRequest
                        });
                    }
                    if (!ValidateTopics(item))
                    {
                        return new JsonResult(new ApiResponse<string>
                        {
                            Message = ResponseMessages.InvalidTopics,
                            Result = false,
                            StatusCode = ResponseStatusCode.BadRequest
                        });
                    }
                    if (CheckOptions(options))
                    {
                        return new JsonResult(new ApiResponse<string>
                        {
                            Message = ResponseMessages.InvalidOptions,
                            Result = false,
                            StatusCode = ResponseStatusCode.BadRequest
                        });
                    }

                }

                ValidateImportFileVM validateImportFileVM = await checkImportedData<ImportQuestionFieldsVM>(importQuestionFieldsVMList);

                if (!validateImportFileVM.isValidate)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = ResponseMessages.InsertProperData,
                        Result = false,
                        StatusCode = ResponseStatusCode.BadRequest
                    });
                }

                int count = 0;
                using (var connection = _dapperContext.CreateConnection())
                {
                    var procedure = "import_questions";
                    var parameters = new DynamicParameters();
                    parameters.Add("import_question_data", importQuestionFieldsVMList, DbType.Object, ParameterDirection.Input);
                    parameters.Add("questions_imported_count", ParameterDirection.Output);
                    count = connection.Query<int>(procedure, parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
                if (count > 0)
                {
                    return new JsonResult(new ApiResponse<int>
                    {
                        Data = count,
                        Message = string.Format(ResponseMessages.AddSuccess, ModuleNames.Questions),
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }

                return new JsonResult(new ApiResponse<string>
                {
                    Message = ResponseMessages.InternalError,
                    Result = false,
                    StatusCode = ResponseStatusCode.InternalServerError
                });
            }

            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in QuestionRepository.ImportQuestions:{ex}");
                return new JsonResult(new ApiResponse<string>
                {
                    Message = ResponseMessages.InternalError,
                    Result = false,
                    StatusCode = ResponseStatusCode.InternalServerError
                });
            }
        }

        public async Task<JsonResult> UpdateBulkStatus(BulkStatusUpdateVM bulkStatusUpdateVM)
        {
            try
            {
                if (bulkStatusUpdateVM.IdList == null || bulkStatusUpdateVM.IdList.Length == 0)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = ResponseMessages.BadRequest,
                        Result = false,
                        StatusCode = ResponseStatusCode.BadRequest
                    });
                }
                List<Question> questions = _context.Questions.Where(q => bulkStatusUpdateVM.IdList.Contains(q.Id)).ToList();
                questions.ForEach(questions => questions.Status = bulkStatusUpdateVM.Status);
                _context.UpdateRange(questions);
                _context.SaveChanges();
                return new JsonResult(new ApiResponse<string>
                {
                    Message = string.Format(ResponseMessages.StatusUpdateSuccess, ModuleNames.Question),
                    Result = true,
                    StatusCode = ResponseStatusCode.Success
                });
            }

            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in QuestionRepository.UpdateBulkStatus:{ex}");
                return new JsonResult(new ApiResponse<string>
                {
                    Message = ResponseMessages.InternalError,
                    Result = false,
                    StatusCode = ResponseStatusCode.InternalServerError
                });
            }
        }

        public async Task<JsonResult> DeleteMultipleQuestions(int[] questionIdList)
        {
            try
            {
                if (questionIdList == null || questionIdList.Length == 0)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = ResponseMessages.BadRequest,
                        Result = false,
                        StatusCode = ResponseStatusCode.BadRequest
                    });
                }

                using (var connection = _dapperContext.CreateConnection())
                {
                    var procedure = "deletemultiplequestion";
                    var parameters = new DynamicParameters();
                    parameters.Add("question_ids", questionIdList, DbType.Object, ParameterDirection.Input);
                    connection.Execute(procedure, parameters, commandType: CommandType.StoredProcedure);
                }
                return new JsonResult(new ApiResponse<string>
                {
                    Message = string.Format(ResponseMessages.DeleteSuccess, ModuleNames.Questions),
                    Result = true,
                    StatusCode = ResponseStatusCode.Success
                });
            }

            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in QuestionRepository.DeleteMultipleQuestions:{ex}");
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

        static List<ImportQuestionCsvVM> ReadCsvFile(IFormFile file)
        {
            try
            {
                using (var reader = new StreamReader(file.OpenReadStream(), Encoding.UTF8))
                using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
                {
                    List<ImportQuestionCsvVM> data = csv.GetRecords<ImportQuestionCsvVM>().ToList();
                    foreach (var item in data)
                    {
                        item.questiontext = item.questiontext.Replace("�", " ");
                        item.optiondata1 = item.optiondata1.Replace("�", " ");
                        item.optiondata2 = item.optiondata2.Replace("�", " ");
                        item.optiondata3 = item.optiondata3.Replace("�", " ");
                        item.optiondata4 = item.optiondata4.Replace("�", " ");
                    }
                    return data;
                }
            }

            catch
            {
                return null;
            }

        }
        private static bool IsOptionValid(QuestionVM questionVM, OptionVM option)
        {
            if (questionVM.OptionType == (int)Common.Enums.OptionType.TextType && option.OptionValue != null)
            {
                return true;
            }
            return false;
        }
        private static bool IsQuestionVMInvalid(QuestionVM questionVM)
        {
            if (questionVM.Id != 0 || !ValidateQuestion(questionVM) || !ValidateOptionText(questionVM) || (questionVM.DuplicateFromQuestionId == 0 && !ValidateOptionImages(questionVM)))
            {
                return true;
            }
            return false;
        }

        private static bool IsDuplicateQuestionInvalid(Question? duplicateQuestion, QuestionVM questionVM)
        {
            if (duplicateQuestion == null || (duplicateQuestion.OptionType != (int)Enums.OptionType.ImageType && questionVM.OptionType == (int)Enums.OptionType.ImageType && !ValidateOptionImages(questionVM)))
            {
                return true;
            }
            return false;
        }

        private static bool ValidateQuestion(QuestionVM questionVM)
        {
            if (questionVM.Options.Count == 4)
            {
                int answerCount = questionVM.Options.Where(option => option.IsAnswer).Count();
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

        private static bool ValidateOptionText(QuestionVM questionVM)
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

        private static bool ValidateOptionImages(QuestionVM questionVM)
        {
            if (questionVM.OptionType == (int)Common.Enums.OptionType.ImageType)
            {
                return !questionVM.Options.Exists(option => option.OptionImage == null);
            }
            return true;
        }


        private static bool ValidateTopics(ImportQuestionFieldsVM question)
        {
            string topic = question.topic.Trim().ToLower();
            switch (topic)
            {
                case "maths":
                    question.topicid = (int)Enums.QuestionTopic.Maths;
                    break;

                case "reasoning":
                    question.topicid = (int)Enums.QuestionTopic.Reasoning;
                    break;

                case "technical":
                    question.topicid = (int)Enums.QuestionTopic.Technical;
                    break;

                default:
                    return false;
            }
            return true;
        }

        private string GenerateSequence(int duplicateId)
        {
            if (duplicateId == 0)
            {
                return (getHighestSequence() + 1).ToString();
            }
            else
            {
                string? temp = _context.Questions.Where(q => q.ParentId == duplicateId && q.IsDeleted != true).Select(x => x.Sequence).ToList()
               .OrderByDescending(s => s)
               .FirstOrDefault();
                if (temp == null)
                {
                    return _context.Questions.Where(q => q.Id == duplicateId).Select(q => q.Sequence).FirstOrDefault() + " " + ((char)SequenceStart.Start).ToString();
                }
                else
                {
                    string num = new string(temp.TakeWhile(char.IsDigit).ToArray());
                    char alphabet = new string(temp.SkipWhile(char.IsDigit).ToArray())[1];
                    char nextChar = (char)(alphabet + 1);
                    return num + " " + nextChar;
                }
            }
        }

        private void FillImportQuestionSequence(List<ImportQuestionFieldsVM> questions)
        {
            int start = getHighestSequence();
            questions.ForEach(q =>
            {
                q.questiontype = (int)Enums.QuestionType.SingleAnswer;
                q.optiontype = (int)Enums.OptionType.TextType;
                switch (q.correctoption.ToString().ToUpper())
                {
                    case "A":
                        q.isanswer1 = true;
                        break;

                    case "B":
                        q.isanswer2 = true;
                        break;

                    case "C":
                        q.isanswer3 = true;
                        break;

                    case "D":
                        q.isanswer4 = true;
                        break;

                    default:
                        break;

                }
                if (q.version == null)
                {
                    start++;
                    q.sequence = start.ToString();

                }
                else
                {
                    q.isparent = true;
                    q.sequence = start.ToString() + " " + q.version;
                }

            });
        }

        private int getHighestSequence()
        {
            return _context.Questions.Select(x => x.Sequence).ToList()
               .OrderBy(s => int.Parse(new string(s.TakeWhile(char.IsDigit).ToArray())))
               .ThenBy(s => s)
               .Select(s => int.Parse(new string(s.TakeWhile(char.IsDigit).ToArray())))
               .LastOrDefault();
        }

        private static string? GetValueForHeader(string[] row, string[] headers, string headerName)
        {
            var index = Array.IndexOf(headers, headerName);
            return index >= 0 && index < row.Length ? row[index] : null;
        }

        private static async Task<ValidateImportFileVM> checkImportedData<T>(List<T> records)
        {
            ValidateImportFileVM validate = new();

            foreach (var record in records)
            {
                var context = new ValidationContext(record, serviceProvider: null, items: null);
                var results = new List<ValidationResult>();

                if (!Validator.TryValidateObject(record, context, results, validateAllProperties: true))
                {
                    validate.validationMessage = new List<string>();
                    validate.isValidate = false;
                    validate.validationMessage.AddRange(results.Select(x => x.ErrorMessage).ToList());

                }
            }
            return validate;
        }

        private static bool IsNextPage(QuestionDataVM temp)
        {
            return temp?.NextPage == null ? false : true;
        }

        private static bool IsPreviousPage(int pageIndex)
        {
            return pageIndex == (int)Enums.Pagination.DefaultIndex ? false : true;
        }
        private static bool CheckOptions(List<OptionVM> options)
        {
            List<string> optionValues = options.Select(x => x.OptionValue).ToList();
            if (optionValues != null)
            {
                for (int i = 0; i < 4; i++)
                {
                    string option = optionValues[i];
                    if (option != "" && option != null)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            if (i != j && optionValues[i] == optionValues[j])
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        #endregion
    }
}
