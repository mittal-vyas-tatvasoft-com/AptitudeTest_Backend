using AptitudeTest.Core.Entities.Admin;
using AptitudeTest.Core.Entities.CandidateSide;
using AptitudeTest.Core.Entities.Test;
using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using AptitudeTest.Data.Common;
using APTITUDETEST.Common.Data;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;
using static AptitudeTest.Data.Common.Enums;

namespace AptitudeTest.Data.Data
{
    public class CandidateRepository : ICandidateRepository
    {
        #region Properies
        private readonly AppDbContext _appDbContext;
        private readonly IConfiguration _config;
        private readonly string connectionString;

        #endregion

        #region Constructor
        public CandidateRepository(AppDbContext appDbContext, IConfiguration config)
        {
            _appDbContext = appDbContext;
            _config = config;
            connectionString = _config["ConnectionStrings:AptitudeTest"];
        }
        #endregion

        #region Method
        public async Task<JsonResult> CreateUserTest(CreateUserTestVM userTest)
        {
            try
            {
                if (userTest != null)
                {
                    UserTest? userTestAlreadyExists = _appDbContext.UserTests.Where(x => x.UserId == userTest.UserId && x.TestId == userTest.TestId && x.IsDeleted == false).FirstOrDefault();
                    if (userTestAlreadyExists == null)
                    {
                        UserTest userTestToBeAdded = new UserTest()
                        {
                            UserId = userTest.UserId,
                            TestId = userTest.TestId,
                            Status = true,
                            IsFinished = false,
                            CreatedBy = userTest.UserId,
                        };

                        _appDbContext.Add(userTestToBeAdded);
                        int count = _appDbContext.SaveChanges();
                        if (count == 1)
                        {

                            return new JsonResult(new ApiResponse<string>
                            {
                                Message = string.Format(ResponseMessages.AddSuccess, ModuleNames.UserTest),
                                Result = true,
                                StatusCode = ResponseStatusCode.OK
                            });
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
                            Message = string.Format(ResponseMessages.AlreadyExists, ModuleNames.UserTest),
                            Result = false,
                            StatusCode = ResponseStatusCode.AlreadyExist
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
        public async Task<JsonResult> CreateTempUserTest(int userId)
        {
            try
            {
                if (userId != 0)
                {
                    Test? test = GetTestOfUser(userId);
                    if (test != null)
                    {
                        TempUserTest? tempUserTestAlreadyExists = _appDbContext.TempUserTests.Where(x => x.UserId == userId && x.TestId == test.Id && x.IsDeleted == false).FirstOrDefault();
                        if (tempUserTestAlreadyExists == null)
                        {
                            TimeSpan difference = (DateTime)test.StartTime - DateTime.UtcNow;
                            int timeRamining = test.TestDuration - (int)difference.TotalMinutes;

                            TempUserTest tempUserTestToBeAdded = new TempUserTest()
                            {
                                UserId = userId,
                                TestId = test.Id,
                                Status = true,
                                TimeRemaining = timeRamining,
                                IsAdminApproved = false,
                                IsFinished = false,
                                CreatedBy = userId,
                            };

                            _appDbContext.Add(tempUserTestToBeAdded);
                            int count = _appDbContext.SaveChanges();
                            if (count == 1)
                            {
                                bool result = AddTestQuestionsToUser(userId, tempUserTestToBeAdded.Id, test.Id);
                                if (result)
                                {
                                    return new JsonResult(new ApiResponse<string>
                                    {
                                        Message = string.Format(ResponseMessages.AddSuccess, ModuleNames.TempUserTest) + string.Format(ResponseMessages.AddSuccess, ModuleNames.TestQuestions),
                                        Result = true,
                                        StatusCode = ResponseStatusCode.OK
                                    });
                                }
                                else
                                {
                                    return new JsonResult(new ApiResponse<string>
                                    {
                                        Message = string.Format(ResponseMessages.AddSuccess, ModuleNames.TempUserTest) + string.Format(ResponseMessages.InternalErrorForAddingQuestionsToTest),
                                        Result = true,
                                        StatusCode = ResponseStatusCode.OK
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
                                Message = string.Format(ResponseMessages.AlreadyExists, ModuleNames.TempUserTest),
                                Result = false,
                                StatusCode = ResponseStatusCode.AlreadyExist
                            });
                        }
                    }
                    else
                    {
                        return new JsonResult(new ApiResponse<Admin>
                        {
                            Message = string.Format(ResponseMessages.NotFound, ModuleNames.Test),
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
        public async Task<JsonResult> CreateUserTestResult(CreateUserTestResultVM userTestResult)
        {
            try
            {
                if (userTestResult != null)
                {
                    UserTestResult? userTestResultAlreadyExists = _appDbContext.UserTestResult.Where(x => x.UserTestId == userTestResult.UserTestId && x.IsDeleted == false).FirstOrDefault();
                    if (userTestResultAlreadyExists == null)
                    {
                        UserTestResult userTestResultToBeAdded = new UserTestResult()
                        {
                            UserTestId = userTestResult.UserTestId,
                            QuestionId = userTestResult.QuestionId,
                            UserAnswers = userTestResult.UserAnswers,
                            IsAttended = userTestResult.IsAttended,
                            CreatedBy = userTestResult.CreatedBy,
                        };

                        _appDbContext.Add(userTestResultToBeAdded);
                        int count = _appDbContext.SaveChanges();
                        if (count == 1)
                        {
                            return new JsonResult(new ApiResponse<string>
                            {
                                Message = string.Format(ResponseMessages.AddSuccess, ModuleNames.UserTestResult),
                                Result = true,
                                StatusCode = ResponseStatusCode.OK
                            });
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
                            Message = string.Format(ResponseMessages.AlreadyExists, ModuleNames.UserTestResult),
                            Result = false,
                            StatusCode = ResponseStatusCode.AlreadyExist
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
        public async Task<JsonResult> CreateTempUserTestResult(CreateUserTestResultVM tempUserTestResult)
        {
            try
            {
                if (tempUserTestResult != null)
                {
                    TempUserTestResult? tempUserTestResultAlreadyExists = _appDbContext.TempUserTestResult.Where(x => x.UserTestId == tempUserTestResult.UserTestId && x.IsDeleted == false).FirstOrDefault();
                    if (tempUserTestResultAlreadyExists == null)
                    {
                        TempUserTestResult tempUserTestResultToBeAdded = new TempUserTestResult()
                        {
                            UserTestId = tempUserTestResult.UserTestId,
                            QuestionId = tempUserTestResult.QuestionId,
                            UserAnswers = tempUserTestResult.UserAnswers,
                            IsAttended = tempUserTestResult.IsAttended,
                            CreatedBy = tempUserTestResult.CreatedBy,
                        };

                        _appDbContext.Add(tempUserTestResultToBeAdded);
                        int count = _appDbContext.SaveChanges();
                        if (count == 1)
                        {
                            return new JsonResult(new ApiResponse<string>
                            {
                                Message = string.Format(ResponseMessages.AddSuccess, ModuleNames.TempUserTestResult),
                                Result = true,
                                StatusCode = ResponseStatusCode.OK
                            });
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
                            Message = string.Format(ResponseMessages.AlreadyExists, ModuleNames.TempUserTestResult),
                            Result = false,
                            StatusCode = ResponseStatusCode.AlreadyExist
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

        public bool AddTestQuestionsToUser(int userId, int userTestId, int testId)
        {
            List<RandomQuestionsVM> allQuestions = new();
            List<TestWiseQuestionsCountVM> testWiseQuestionsCount = new();
            Random rand = new Random();
            bool isParent = rand.NextDouble() >= 0.5;

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                allQuestions = connection.Query<RandomQuestionsVM>("Select * from GetRandomQuestions(@IsParent)", new { IsParent = isParent }).ToList();
                testWiseQuestionsCount = connection.Query<TestWiseQuestionsCountVM>("Select * from GetTestQuestionsConfig(@Id)", new { Id = testId }).ToList();
                connection.Close();
            }

            Dictionary<(int, QuestionType, int), int> questionsPerMarkTypeTopicId = setQuestionsConfig(testWiseQuestionsCount);

            // Randomly select questions for each mark, type, and topic Id
            List<RandomQuestionsVM> selectedQuestions = SelectRandomQuestions(allQuestions, questionsPerMarkTypeTopicId);
            if (selectedQuestions.Count != 0)
            {
                var tempUserTestResults = selectedQuestions.Select(ques => new TempUserTestResult()
                {
                    UserTestId = userTestId,
                    QuestionId = ques.Id,
                    CreatedBy = userId
                });

                _appDbContext.TempUserTestResult.AddRange(tempUserTestResults);
                _appDbContext.SaveChanges();
                return true;
            }
            return false;
        }

        #endregion

        #region Helper Method
        private Test? GetTestOfUser(int userId)
        {
            int? collegeId = _appDbContext.Users.Where(x => x.Id == userId && x.IsDeleted == false).Select(x => x.CollegeId).FirstOrDefault();
            if (collegeId != null)
            {
                int? groupId = _appDbContext.MasterCollege.Where(x => x.Id == collegeId && x.IsDeleted == false).Select(x => x.GroupId).FirstOrDefault();
                if (groupId != null)
                {
                    Test? test = _appDbContext.Tests.Where(x => x.GroupId == groupId && x.IsDeleted == false).FirstOrDefault();
                    if (test != null)
                    {
                        return test;
                    }
                }
            }
            return null;
        }

        private Dictionary<(int, QuestionType, int), int> setQuestionsConfig(List<TestWiseQuestionsCountVM> testWiseQuestionsCount)
        {
            Dictionary<(int, QuestionType, int), int> questionsPerMarkTypeTopicId = new Dictionary<(int, QuestionType, int), int>();

            foreach (var row in testWiseQuestionsCount)
            {
                if (row.OneMarks != 0)
                {
                    questionsPerMarkTypeTopicId.Add((1, (QuestionType)row.QuestionType, row.TopicId), row.OneMarks);
                }
                if (row.TwoMarks != 0)
                {
                    questionsPerMarkTypeTopicId.Add((2, (QuestionType)row.QuestionType, row.TopicId), row.TwoMarks);
                }
                if (row.ThreeMarks != 0)
                {
                    questionsPerMarkTypeTopicId.Add((3, (QuestionType)row.QuestionType, row.TopicId), row.ThreeMarks);
                }
                if (row.FourMarks != 0)
                {
                    questionsPerMarkTypeTopicId.Add((4, (QuestionType)row.QuestionType, row.TopicId), row.FourMarks);
                }
                if (row.FiveMarks != 0)
                {
                    questionsPerMarkTypeTopicId.Add((5, (QuestionType)row.QuestionType, row.TopicId), row.FiveMarks);
                }
            }
            return questionsPerMarkTypeTopicId;
        }

        private List<RandomQuestionsVM> SelectRandomQuestions(List<RandomQuestionsVM> allQuestions, Dictionary<(int, QuestionType, int), int> questionsPerMarkTypeTopicId)
        {
            // Group questions by their mark, type, and topic ID
            var groupedQuestions = allQuestions.GroupBy(q => (q.Difficulty, q.QuestionType, q.Topic));

            // Initialize a list to store the selected questions
            List<RandomQuestionsVM> selectedQuestions = new List<RandomQuestionsVM>();

            // Randomly select questions based on the specified counts for each mark, type, and topic ID
            foreach (var entry in questionsPerMarkTypeTopicId)
            {
                var group = groupedQuestions.FirstOrDefault(q => q.Key.Difficulty == entry.Key.Item1 && q.Key.QuestionType == (int)entry.Key.Item2 && q.Key.Topic == entry.Key.Item3);

                if (group != null)
                {
                    var randomQuestions = group.OrderBy(q => Guid.NewGuid()).Take(entry.Value);
                    selectedQuestions.AddRange(randomQuestions);
                }
            }

            return selectedQuestions;
        }
        #endregion
    }
}
