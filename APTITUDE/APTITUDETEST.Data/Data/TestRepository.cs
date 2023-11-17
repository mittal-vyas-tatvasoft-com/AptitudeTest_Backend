﻿using AptitudeTest.Core.Entities.Test;
using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using AptitudeTest.Data.Common;
using APTITUDETEST.Common.Data;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;
using static AptitudeTest.Data.Common.Enums;

namespace AptitudeTest.Data.Data
{
    public class TestRepository : ITestRepository
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        private readonly string connectionString;

        public TestRepository(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
            connectionString = _config["ConnectionStrings:AptitudeTest"];
        }

        #region Methods
        public async Task<JsonResult> GetTests(string? searchQuery, int? GroupId, int? Status, DateTime? Date, int? currentPageIndex, int? pageSize)
        {
            try
            {
                searchQuery = string.IsNullOrEmpty(searchQuery) ? string.Empty : searchQuery;
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    List<TestsViewModel> data = connection.Query<TestsViewModel>("Select * from getalltests(@SearchQuery,@GroupId,@Status,@DateFilter,@PageNumber,@PageSize)", new { SearchQuery = searchQuery, GroupId = (object)GroupId, Status = Status, DateFilter = Date, PageNumber = currentPageIndex, PageSize = pageSize }).ToList();
                    connection.Close();
                    return new JsonResult(new ApiResponse<List<TestsViewModel>>
                    {
                        Data = data.OrderByDescending(x => x.Testid).ToList(),
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


        public async Task<JsonResult> CreateTest(CreateTestVM test)
        {
            try
            {
                Test? testAlreadyExists = _context.Tests.Where(t => t.Name == test.Name && t.Status == (int)Common.Enums.TestStatus.Active && t.IsDeleted == false).FirstOrDefault();
                if (testAlreadyExists == null)
                {
                    Test testToBeAdded = new Test()
                    {
                        Name = test.Name,
                        Description = test.Description,
                        Date = test.Date,
                        StartTime = test.StartTime,
                        EndTime = test.EndTime,
                        TestDuration = test.TestDuration,
                        Status = test.Status,
                        BasicPoint = test.BasicPoint,
                        MessaageAtStartOfTheTest = test.MessaageAtStartOfTheTest,
                        MessaageAtEndOfTheTest = test.MessaageAtEndOfTheTest,
                        IsRandomQuestion = test.IsRandomQuestion,
                        IsRandomAnswer = test.IsRandomAnswer,
                        IsLogoutWhenTimeExpire = test.IsLogoutWhenTimeExpire,
                        IsQuestionsMenu = test.IsQuestionsMenu,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = test.CreatedBy,
                    };

                    _context.Add(testToBeAdded);
                    _context.SaveChanges();
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.AddSuccess, ModuleNames.Test),
                        Result = true,
                        StatusCode = ResponseStatusCode.OK
                    });
                }
                else
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.AlreadyExists, ModuleNames.Test),
                        Result = false,
                        StatusCode = ResponseStatusCode.AlreadyExist
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

        public async Task<JsonResult> UpdateTestGroup(UpdateTestGroupVM updateTest)
        {
            try
            {
                Test testAlreadyExists = _context.Tests.Where(t => t.Id != updateTest.TestId && t.GroupId == updateTest.GroupId && t.Status == (int)Common.Enums.TestStatus.Active && t.IsDeleted == false).FirstOrDefault();
                if (testAlreadyExists != null)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.AlreadyExists, ModuleNames.Test),
                        Result = true,
                        StatusCode = ResponseStatusCode.OK
                    });
                }
                Test test = await Task.FromResult(_context.Tests.Where(t => t.Id == updateTest.TestId && t.Status == (int)Common.Enums.TestStatus.Active && t.IsDeleted == false).FirstOrDefault());
                if (test != null)
                {
                    test.GroupId = updateTest.GroupId;
                    test.UpdatedBy = updateTest.UpdatedBy;
                    test.UpdatedDate = DateTime.UtcNow;
                    _context.SaveChanges();

                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.UpdateSuccess, ModuleNames.Test),
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
                return new JsonResult(new ApiResponse<string>
                {
                    Message = ResponseMessages.InternalError,
                    Result = false,
                    StatusCode = ResponseStatusCode.InternalServerError
                });
            }

        }

        public async Task<JsonResult> AddTestQuestions(TestQuestionsVM addTestQuestion)
        {
            try
            {
                Test test = await Task.FromResult(_context.Tests.Where(t => t.Id == addTestQuestion.TestId && t.Status == (int)Common.Enums.TestStatus.Active && t.IsDeleted == false).FirstOrDefault());
                if (test == null)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.NotFound, ModuleNames.Test),
                        Result = true,
                        StatusCode = ResponseStatusCode.OK
                    });
                }

                int totalQuestionsCount = addTestQuestion.TestQuestionsCount.Sum(x => x.OneMarkQuestion + x.TwoMarkQuestion + x.ThreeMarkQuestion + x.FourMarkQuestion + x.FiveMarkQuestion);
                if (totalQuestionsCount != addTestQuestion.NoOfQuestions)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.NoOfQuestions),
                        Result = false,
                        StatusCode = ResponseStatusCode.BadRequest
                    });
                }

                var result = doesQuestionsAvailableInDB(addTestQuestion);
                if (!result.Item1)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.NotEnoughQuestion, result.Item2, result.Item3),
                        Result = false,
                        StatusCode = ResponseStatusCode.BadRequest
                    });
                }

                TestQuestions testQuestion = await Task.FromResult(_context.TestQuestions.Where(t => t.TestId == addTestQuestion.TestId && t.TopicId == addTestQuestion.TopicId && t.IsDeleted == false).FirstOrDefault());
                if (testQuestion != null)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.TestTopicAlreadyExists),
                        Result = false,
                        StatusCode = ResponseStatusCode.BadRequest
                    });
                }

                TestQuestions testQuestionsToBeAdded = new TestQuestions();

                testQuestionsToBeAdded.TestId = addTestQuestion.TestId;
                testQuestionsToBeAdded.TopicId = addTestQuestion.TopicId;
                testQuestionsToBeAdded.NoOfQuestions = addTestQuestion.NoOfQuestions;
                testQuestionsToBeAdded.Weightage = addTestQuestion.Weightage;
                testQuestionsToBeAdded.CreatedDate = DateTime.UtcNow;
                testQuestionsToBeAdded.CreatedBy = addTestQuestion.CreatedBy;

                _context.Add(testQuestionsToBeAdded);
                _context.SaveChanges();

                TestQuestionsCount testQuestionsCountToBeAdded = new TestQuestionsCount();

                foreach (var testQuestionCount in addTestQuestion.TestQuestionsCount)
                {
                    testQuestionsCountToBeAdded.TestQuestionId = testQuestionsToBeAdded.Id;
                    testQuestionsCountToBeAdded.QuestionType = testQuestionCount.QuestionType;
                    testQuestionsCountToBeAdded.OneMarks = testQuestionCount.OneMarkQuestion;
                    testQuestionsCountToBeAdded.TwoMarks = testQuestionCount.TwoMarkQuestion;
                    testQuestionsCountToBeAdded.ThreeMarks = testQuestionCount.ThreeMarkQuestion;
                    testQuestionsCountToBeAdded.FourMarks = testQuestionCount.FourMarkQuestion;
                    testQuestionsCountToBeAdded.FiveMarks = testQuestionCount.FiveMarkQuestion;
                    testQuestionsCountToBeAdded.CreatedDate = DateTime.UtcNow;
                    testQuestionsCountToBeAdded.CreatedBy = addTestQuestion.CreatedBy;

                    _context.Add(testQuestionsCountToBeAdded);
                    _context.SaveChanges();
                }

                return new JsonResult(new ApiResponse<string>
                {
                    Message = string.Format(ResponseMessages.AddSuccess, ModuleNames.TestQuestions),
                    Result = true,
                    StatusCode = ResponseStatusCode.OK
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

        public async Task<JsonResult> UpdateTestQuestions(TestQuestionsVM updateTestQuestion)
        {
            try
            {
                Test test = await Task.FromResult(_context.Tests.Where(t => t.Id == updateTestQuestion.TestId && t.Status == (int)Common.Enums.TestStatus.Active && t.IsDeleted == false).FirstOrDefault());
                if (test == null)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.NotFound, ModuleNames.Test),
                        Result = true,
                        StatusCode = ResponseStatusCode.OK
                    });
                }

                int totalQuestionsCount = updateTestQuestion.TestQuestionsCount.Sum(x => x.OneMarkQuestion + x.TwoMarkQuestion + x.ThreeMarkQuestion + x.FourMarkQuestion + x.FiveMarkQuestion);
                if (totalQuestionsCount != updateTestQuestion.NoOfQuestions)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.NoOfQuestions),
                        Result = false,
                        StatusCode = ResponseStatusCode.OK
                    });
                }

                var result = doesQuestionsAvailableInDB(updateTestQuestion);
                if (!result.Item1)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.NotEnoughQuestion, result.Item2, result.Item3),
                        Result = true,
                        StatusCode = ResponseStatusCode.OK
                    });
                }

                TestQuestions testQuestion = await Task.FromResult(_context.TestQuestions.Where(t => t.TestId == updateTestQuestion.TestId && t.TopicId == updateTestQuestion.TopicId && t.IsDeleted == false).FirstOrDefault());
                if (testQuestion == null)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.TestTopicQuestionsNotFound),
                        Result = true,
                        StatusCode = ResponseStatusCode.OK
                    });
                }

                testQuestion.NoOfQuestions = updateTestQuestion.NoOfQuestions;
                testQuestion.UpdatedDate = DateTime.UtcNow;
                testQuestion.UpdatedBy = updateTestQuestion.CreatedBy;
                _context.SaveChanges();

                foreach (var testQuestionCount in updateTestQuestion.TestQuestionsCount)
                {
                    TestQuestionsCount testQuestionTypeWiseCountExists = await Task.FromResult(_context.TestQuestionsCount.Where(t => t.TestQuestionId == testQuestion.Id && t.QuestionType == testQuestionCount.QuestionType && t.IsDeleted == false).FirstOrDefault());
                    if (testQuestionTypeWiseCountExists != null)
                    {
                        testQuestionTypeWiseCountExists.OneMarks = testQuestionCount.OneMarkQuestion;
                        testQuestionTypeWiseCountExists.TwoMarks = testQuestionCount.TwoMarkQuestion;
                        testQuestionTypeWiseCountExists.ThreeMarks = testQuestionCount.ThreeMarkQuestion;
                        testQuestionTypeWiseCountExists.FourMarks = testQuestionCount.FourMarkQuestion;
                        testQuestionTypeWiseCountExists.FiveMarks = testQuestionCount.FiveMarkQuestion;

                        _context.SaveChanges(true);
                    }
                }

                return new JsonResult(new ApiResponse<string>
                {
                    Message = string.Format(ResponseMessages.UpdateSuccess, ModuleNames.TestQuestions),
                    Result = true,
                    StatusCode = ResponseStatusCode.OK
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

        public async Task<JsonResult> DeleteTopicWiseTestQuestions(int testId, int topicId)
        {
            try
            {
                if (testId != 0 && topicId != 0)
                {
                    TestQuestions? testQuestionToBeDeleted = await Task.FromResult(_context.TestQuestions.Where(t => t.TestId == testId && t.TopicId == topicId && t.IsDeleted == false).FirstOrDefault());
                    if (testQuestionToBeDeleted != null)
                    {
                        testQuestionToBeDeleted.IsDeleted = true;
                        _context.Update(testQuestionToBeDeleted);
                        int count = _context.SaveChanges();

                        if (count == 1)
                        {
                            List<TestQuestionsCount> testQuestionsCountToBeDeleted = _context.TestQuestionsCount.Where(t => t.TestQuestionId == testQuestionToBeDeleted.Id && t.IsDeleted == false).ToList();
                            if (testQuestionsCountToBeDeleted != null && testQuestionsCountToBeDeleted.Count > 0)
                            {
                                foreach (var item in testQuestionsCountToBeDeleted)
                                {
                                    item.IsDeleted = true;
                                    _context.Update(item);
                                    count = _context.SaveChanges();
                                }

                                if (count == 1)
                                {
                                    return new JsonResult(new ApiResponse<string>
                                    {
                                        Message = string.Format(ResponseMessages.DeleteSuccess, ModuleNames.TestQuestions),
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
                            else
                            {
                                return new JsonResult(new ApiResponse<string>
                                {
                                    Message = string.Format(ResponseMessages.TestTopicQuestionsNotFound),
                                    Result = false,
                                    StatusCode = ResponseStatusCode.NotFound
                                });
                            }
                        }
                        else
                        {
                            return new JsonResult(new ApiResponse<string>
                            {
                                Message = ResponseMessages.InternalError,
                                Result = false,
                                StatusCode = ResponseStatusCode.InternalServerError
                            });
                        }
                    }
                    else
                    {
                        return new JsonResult(new ApiResponse<string>
                        {
                            Message = string.Format(ResponseMessages.NotFound, ModuleNames.TestQuestions),
                            Result = false,
                            StatusCode = ResponseStatusCode.NotFound
                        });
                    }
                }
                else
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = ResponseMessages.BadRequest,
                        Result = false,
                        StatusCode = ResponseStatusCode.BadRequest
                    });
                }
            }
            catch
            {
                return new JsonResult(new ApiResponse<string>
                {
                    Message = ResponseMessages.InternalError,
                    Result = false,
                    StatusCode = ResponseStatusCode.InternalServerError
                });
            }
        }

        public async Task<JsonResult> DeleteAllTestQuestions(int testId)
        {
            try
            {
                if (testId != 0)
                {
                    List<TestQuestions> testQuestionsToBeDeleted = await Task.FromResult(_context.TestQuestions.Where(t => t.TestId == testId && t.IsDeleted == false).ToList());
                    if (testQuestionsToBeDeleted != null && testQuestionsToBeDeleted.Count > 0)
                    {
                        foreach (var testQuestion in testQuestionsToBeDeleted)
                        {
                            testQuestion.IsDeleted = true;
                            testQuestion.UpdatedDate = DateTime.UtcNow;
                            _context.Update(testQuestion);
                            int count = _context.SaveChanges();

                            if (count == 1)
                            {
                                List<TestQuestionsCount> testQuestionsCountToBeDeleted = _context.TestQuestionsCount.Where(t => t.TestQuestionId == testQuestion.Id && t.IsDeleted == false).ToList();
                                if (testQuestionsCountToBeDeleted != null && testQuestionsCountToBeDeleted.Count > 0)
                                {
                                    foreach (var testQuestionCount in testQuestionsCountToBeDeleted)
                                    {
                                        testQuestionCount.IsDeleted = true;
                                        _context.Update(testQuestionCount);
                                        count = _context.SaveChanges();
                                    }
                                }
                            }
                        }
                        return new JsonResult(new ApiResponse<string>
                        {
                            Message = string.Format(ResponseMessages.DeleteSuccess, ModuleNames.TestQuestions),
                            Result = true,
                            StatusCode = ResponseStatusCode.Success
                        });
                    }
                    else
                    {
                        return new JsonResult(new ApiResponse<string>
                        {
                            Message = string.Format(ResponseMessages.NotFound, ModuleNames.TestQuestions),
                            Result = false,
                            StatusCode = ResponseStatusCode.NotFound
                        });
                    }
                }
                return new JsonResult(new ApiResponse<string>
                {
                    Message = string.Format(ResponseMessages.BadRequest),
                    Result = false,
                    StatusCode = ResponseStatusCode.BadRequest
                });
            }
            catch
            {
                return new JsonResult(new ApiResponse<string>
                {
                    Message = ResponseMessages.InternalError,
                    Result = false,
                    StatusCode = ResponseStatusCode.InternalServerError
                });
            }
        }

        public async Task<JsonResult> DeleteTest(int testId)
        {
            try
            {
                if (testId != 0)
                {
                    Test? testToBeDeleted = await Task.FromResult(_context.Tests.Where(t => t.Id == testId && t.IsDeleted == false).FirstOrDefault());
                    if (testToBeDeleted != null)
                    {
                        testToBeDeleted.IsDeleted = true;
                        int count = _context.SaveChanges();

                        if (count == 1)
                        {
                            var result = await DeleteAllTestQuestions(testId);
                            return new JsonResult(new ApiResponse<string>
                            {
                                Message = string.Format(ResponseMessages.DeleteSuccess, ModuleNames.Test),
                                Result = false,
                                StatusCode = ResponseStatusCode.BadRequest
                            });
                        }
                    }
                    else
                    {
                        return new JsonResult(new ApiResponse<string>
                        {
                            Message = ResponseMessages.NotFound,
                            Result = false,
                            StatusCode = ResponseStatusCode.NotFound
                        });
                    }
                }

                return new JsonResult(new ApiResponse<string>
                {
                    Message = ResponseMessages.BadRequest,
                    Result = false,
                    StatusCode = ResponseStatusCode.BadRequest
                });
            }
            catch
            {
                return new JsonResult(new ApiResponse<string>
                {
                    Message = ResponseMessages.InternalError,
                    Result = false,
                    StatusCode = ResponseStatusCode.InternalServerError
                });
            }
        }

        public async Task<JsonResult> GetQuestinsMarksCount(int testId)
        {
            try
            {

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    List<TestQuestionCountMarksDataVM> data = connection.Query<TestQuestionCountMarksDataVM>("Select * from getTestQuestionsCount(@test_id)", new { test_id = testId }).ToList();
                    connection.Close();
                    TestQuestionsCountMarksVM testQuestionsCountMarksVM = new();
                    if (data.Count != 0)
                    {
                        var firstRow = data.FirstOrDefault();
                        testQuestionsCountMarksVM.TestId = firstRow.TestId;
                        testQuestionsCountMarksVM.TotalMarks = firstRow.TotalMarks;
                        testQuestionsCountMarksVM.TotalQuestions = firstRow.TotalQuestion;
                        testQuestionsCountMarksVM.QuestionsCount =
                            data.GroupBy(x => x.TopicId).Select(x =>
                            {
                                var q = x.FirstOrDefault();
                                TestQuestionCountMarksDataVM? multiAns = x.Where(x => x.QuestionType == (int)Enums.QuestionType.MultiAnswer).FirstOrDefault();
                                if (multiAns == null)
                                {
                                    multiAns = new();
                                }
                                TestQuestionCountMarksDataVM? singleAns = x.Where(x => x.QuestionType == (int)Enums.QuestionType.SingleAnswer).FirstOrDefault();
                                if (singleAns == null)
                                {
                                    singleAns = new();
                                }
                                return new QuestionsCountMarksVM()
                                {
                                    TopicId = q.TopicId,
                                    TotalMarks = x.Sum(x => x.TotalTopicMarks),
                                    TotalQuestions = x.Sum(x => x.TotalTopicQuestion),
                                    MultiAnswerCount = multiAns.TotalTopicQuestion,
                                    SingleAnswerCount = singleAns.TotalTopicQuestion,
                                    MultiAnswer = new TestQuestionsCountVM()
                                    {
                                        OneMarkQuestion = multiAns.OneMarks,
                                        TwoMarkQuestion = multiAns.TwoMarks,
                                        ThreeMarkQuestion = multiAns.ThreeMarks,
                                        FourMarkQuestion = multiAns.FourMarks,
                                        FiveMarkQuestion = multiAns.FiveMarks,
                                    },
                                    SingleAnswer = new TestQuestionsCountVM()
                                    {
                                        OneMarkQuestion = singleAns.OneMarks,
                                        TwoMarkQuestion = singleAns.TwoMarks,
                                        ThreeMarkQuestion = singleAns.ThreeMarks,
                                        FourMarkQuestion = singleAns.FourMarks,
                                        FiveMarkQuestion = singleAns.FiveMarks,
                                    }
                                };
                            }).ToList();

                        return new JsonResult(new ApiResponse<TestQuestionsCountMarksVM>
                        {
                            Data = testQuestionsCountMarksVM,
                            Message = ResponseMessages.Success,
                            Result = true,
                            StatusCode = ResponseStatusCode.Success
                        });
                    }
                }

                return new JsonResult(new ApiResponse<string>
                {
                    Message = ResponseMessages.BadRequest,
                    Result = false,
                    StatusCode = ResponseStatusCode.BadRequest
                });
            }
            catch
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
        private (bool, int, string) doesQuestionsAvailableInDB(TestQuestionsVM addTestQuestion)
        {
            var questions = _context.Questions.Where(t => t.Topic == addTestQuestion.TopicId && t.IsDeleted == false).ToList();
            Func<TestQuestionsCountVM, int> func = x => x.OneMarkQuestion;
            var MarkQuestionCountReq = 0;
            for (int questionType = 1; questionType <= 2; questionType++)
            {
                for (int i = 1; i <= 5; i++)
                {
                    int MarkQuestionCountInDB = questions.Where(t => t.Topic == addTestQuestion.TopicId && t.QuestionType == questionType && t.Difficulty == i && t.IsDeleted == false).Count();
                    switch (i)
                    {
                        case 1:
                            func = x => x.OneMarkQuestion;
                            break;
                        case 2:
                            func = x => x.TwoMarkQuestion;
                            break;
                        case 3:
                            func = x => x.ThreeMarkQuestion;
                            break;
                        case 4:
                            func = x => x.FourMarkQuestion;
                            break;
                        case 5:
                            func = x => x.FiveMarkQuestion;
                            break;
                        default:
                            break;
                    }

                    MarkQuestionCountReq = addTestQuestion.TestQuestionsCount.Where(t => t.QuestionType == questionType).Select(func).FirstOrDefault();

                    if (MarkQuestionCountReq > MarkQuestionCountInDB)
                    {
                        return (false, i, Enum.GetName(typeof(QuestionType), questionType));
                    }
                }
            }
            return (true, 0, null);
        }
        #endregion
    }
}
