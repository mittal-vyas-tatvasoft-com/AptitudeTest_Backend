using AptitudeTest.Common.Helpers;
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
using Newtonsoft.Json;
using Npgsql;
using System.Data;
using static AptitudeTest.Data.Common.Enums;

namespace AptitudeTest.Data.Data
{
    public class CandidateRepository : ICandidateRepository
    {
        #region Properies
        private readonly AppDbContext _appDbContext;
        private readonly IConfiguration? _config;
        private readonly string? connectionString;
        private readonly UserActiveTestHelper _userActiveTestHelper;

        #endregion

        #region Constructor
        public CandidateRepository(AppDbContext appDbContext, IConfiguration config, UserActiveTestHelper userActiveTestHelper)
        {
            _appDbContext = appDbContext;
            _config = config;
            connectionString = _config["ConnectionStrings:AptitudeTest"];
            _userActiveTestHelper = userActiveTestHelper;
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
                if (userId <= 0)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = ResponseMessages.BadRequest,
                        Result = false,
                        StatusCode = ResponseStatusCode.BadRequest
                    });
                }
                Test? test = _userActiveTestHelper.GetTestOfUser(userId);
                if (test == null)
                {
                    return new JsonResult(new ApiResponse<Admin>
                    {
                        Message = string.Format(ResponseMessages.NotFound, ModuleNames.Test),
                        Result = false,
                        StatusCode = ResponseStatusCode.NotFound
                    });
                }

                TempUserTest? tempUserTestAlreadyExists = _appDbContext.TempUserTests.Where(x => x.UserId == userId && x.TestId == test.Id).FirstOrDefault();

                if (tempUserTestAlreadyExists != null)
                {
                    if ((bool)tempUserTestAlreadyExists.IsDeleted)
                    {
                        return new JsonResult(new ApiResponse<string>
                        {
                            Message = ResponseMessages.TestAlreadySubmitted,
                            Result = false,
                            StatusCode = ResponseStatusCode.AlreadyExist
                        });
                    }
                    else
                    {
                        if (tempUserTestAlreadyExists.IsAdminApproved)
                        {
                            return new JsonResult(new ApiResponse<string>
                            {
                                Message = ResponseMessages.ResumeTest,
                                Result = true,
                                StatusCode = ResponseStatusCode.OK
                            });
                        }
                        return new JsonResult(new ApiResponse<string>
                        {
                            Message = ResponseMessages.TestAlreadySubmitted,
                            Result = false,
                            StatusCode = ResponseStatusCode.AlreadyExist
                        });
                    }
                }

                TimeSpan difference = test.EndTime - DateTime.Now;
                int differenceInMinutes = (int)difference.TotalMinutes;
                int timeRemaining = differenceInMinutes < test.TestDuration ? differenceInMinutes : test.TestDuration;

                TempUserTest tempUserTestToBeAdded = new TempUserTest()
                {
                    UserId = userId,
                    TestId = test.Id,
                    Status = true,
                    TimeRemaining = timeRemaining,
                    IsAdminApproved = false,
                    IsFinished = false,
                    CreatedBy = userId,
                };

                _appDbContext.Add(tempUserTestToBeAdded);
                int count = _appDbContext.SaveChanges();
                if (count == 1)
                {
                    bool result = AddTestQuestionsToUser(userId, tempUserTestToBeAdded.Id, test.Id);
                    if (!result)
                    {
                        return new JsonResult(new ApiResponse<string>
                        {
                            Message = string.Format(ResponseMessages.AddSuccess, ModuleNames.TempUserTest) + string.Format(ResponseMessages.InternalErrorForAddingQuestionsToTest),
                            Result = true,
                            StatusCode = ResponseStatusCode.OK
                        });
                    }
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
                        Message = ResponseMessages.InternalError,
                        Result = false,
                        StatusCode = ResponseStatusCode.InternalServerError
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
        public async Task<JsonResult> SaveTestQuestionAnswer(UpdateTestQuestionAnswerVM userTestQuestionAnswer)
        {
            try
            {
                if (userTestQuestionAnswer == null)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = ResponseMessages.BadRequest,
                        Result = false,
                        StatusCode = ResponseStatusCode.BadRequest
                    });
                }
                Test? test = _userActiveTestHelper.GetTestOfUser(userTestQuestionAnswer.UserId);
                if (test == null)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.NotFound, ModuleNames.Test),
                        Result = false,
                        StatusCode = ResponseStatusCode.NotFound
                    });
                }
                TempUserTest? tempUserTest = _appDbContext.TempUserTests.FirstOrDefault(x => x.UserId == userTestQuestionAnswer.UserId && x.TestId == test.Id && x.IsDeleted == false);
                if (tempUserTest == null)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.NotFound, ModuleNames.UserTest),
                        Result = false,
                        StatusCode = ResponseStatusCode.NotFound
                    });
                }
                tempUserTest.TimeRemaining = userTestQuestionAnswer.TimeRemaining;
                _appDbContext.SaveChanges();

                TempUserTestResult? tempUserTestQuestion = _appDbContext.TempUserTestResult.Where(x => x.UserTestId == tempUserTest.Id && x.QuestionId == userTestQuestionAnswer.QuestionId && x.IsDeleted == false).FirstOrDefault();
                if (tempUserTestQuestion == null)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.NotFound, ModuleNames.UserTestResult),
                        Result = false,
                        StatusCode = ResponseStatusCode.NotFound
                    });
                }
                tempUserTestQuestion.UserAnswers = userTestQuestionAnswer.UserAnswers?.Length != 0 ? userTestQuestionAnswer.UserAnswers : null;
                tempUserTestQuestion.IsAttended = userTestQuestionAnswer.IsAttended;
                tempUserTestQuestion.UpdatedDate = DateTime.UtcNow;
                tempUserTestQuestion.UpdatedBy = userTestQuestionAnswer.UserId;

                int count = _appDbContext.SaveChanges();
                if (count == 1)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.UpdateSuccess, ModuleNames.TempUserTestResult),
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
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

        public async Task<JsonResult> GetCandidateTestQuestion(int questionId, int userId)
        {
            try
            {
                if (IsQuestionIdAndUserIdInvalid(questionId, userId))
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
                    Test? test = _userActiveTestHelper.GetTestOfUser(userId);
                    int? testId = test.Id;
                    if (testId == null || testId < 1)
                    {
                        return new JsonResult(new ApiResponse<string>
                        {
                            Message = ResponseMessages.BadRequest,
                            Result = false,
                            StatusCode = ResponseStatusCode.BadRequest
                        });
                    }
                    var data = await connection.Connection.QueryAsync<UserTestQuestionModelVM>("select * from getCandidateTestquestion(@question_id,@user_id,@test_id)", new { question_id = questionId, user_id = userId, test_id = testId });
                    if (data == null || !data.Any())
                    {
                        return new JsonResult(new ApiResponse<string>
                        {
                            Message = ResponseMessages.NoRecordsFound,
                            Result = false,
                            StatusCode = ResponseStatusCode.BadRequest
                        });
                    }
                    int[] questions = data.FirstOrDefault().Questions;
                    questionId = SetQuestionId(questionId, questions);

                    int nextIndex = 0;
                    if (questions.Length > 0)
                    {
                        nextIndex = Array.IndexOf(questions, questionId) + 1;
                    }
                    int nextQuestionId = 0;
                    int questionNumber = nextIndex;

                    if (nextIndex < questions.Length)
                    {
                        nextQuestionId = questions[nextIndex];
                    }

                    var question = data.FirstOrDefault();

                    if (question == null)
                    {
                        return new JsonResult(new ApiResponse<UserDetailsVM>
                        {
                            Data = null,
                            Message = string.Format(ResponseMessages.NotFound, ModuleNames.Question),
                            Result = false,
                            StatusCode = ResponseStatusCode.NotFound
                        });
                    }
                    CandidateTestQuestionVM candidateTestQuestionVM = new CandidateTestQuestionVM()
                    {
                        Id = question.QuestionId,
                        Difficulty = question.Difficulty,
                        OptionType = question.OptionType,
                        QuestionType = question.QuestionType,
                        QuestionText = question.QuestionText,
                        NextQuestionId = nextQuestionId,
                        QuestionNumber = questionNumber,
                        TotalQuestions = questions.Length
                    };

                    foreach (var item in data)
                    {

                        CandidateTestOptionsVM candidateTestOptionsVM = new CandidateTestOptionsVM()
                        {
                            OptionData = item.OptionData,
                            OptionId = item.OptionId
                        };
                        candidateTestQuestionVM.Options.Add(candidateTestOptionsVM);
                    }

                    List<CandidateTestAnswerVM> candidateTestAnswerVMList = new();

                    foreach (var item in candidateTestQuestionVM.Options)
                    {
                        bool isAnswer = false;
                        if (question.Answer != null)
                        {
                            isAnswer = IsAnswer(question, item);
                        }

                        CandidateTestAnswerVM candidateTestAnswerVM = new CandidateTestAnswerVM() { OptionId = item.OptionId, isAnswer = isAnswer };
                        candidateTestAnswerVMList.Add(candidateTestAnswerVM);
                    }
                    if (test != null && test.IsRandomAnswer == true)
                    {
                        Shuffle<CandidateTestOptionsVM>(candidateTestQuestionVM.Options);
                    }
                    candidateTestQuestionVM.Answers = candidateTestAnswerVMList;
                    return new JsonResult(new ApiResponse<CandidateTestQuestionVM>
                    {
                        Data = candidateTestQuestionVM,
                        Message = ResponseMessages.Success,
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
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



        public async Task<JsonResult> GetQuestionsStatus(int userId)
        {
            try
            {
                if (userId < 1)
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
                    Test? test = _userActiveTestHelper.GetTestOfUser(userId);
                    int? testId = test?.Id;
                    if (testId == null || testId < 1)
                    {
                        return new JsonResult(new ApiResponse<string>
                        {
                            Message = ResponseMessages.BadRequest,
                            Result = false,
                            StatusCode = ResponseStatusCode.BadRequest
                        });
                    }

                    TempUserTest? tempTest = _appDbContext.TempUserTests.Where(x => x.UserId == userId && x.TestId == testId).FirstOrDefault();
                    if (test == null || test.Id <= 0)
                    {
                        return new JsonResult(new ApiResponse<string>
                        {
                            Message = ResponseMessages.BadRequest,
                            Result = false,
                            StatusCode = ResponseStatusCode.BadRequest
                        });
                    }
                    int userTestId = 0, timeRemaining = 0;
                    if (tempTest != null)
                    {
                        userTestId = tempTest.Id;
                        timeRemaining = tempTest.TimeRemaining;
                    }
                    List<QuestionStatusVM> data = new List<QuestionStatusVM>();
                    int totalCount = 0;
                    int answered = 0;
                    int unAnswered = 0;
                    bool isQuestionsMenu = (bool)test?.IsQuestionsMenu;
                    if (isQuestionsMenu)
                    {

                        var questions = _appDbContext.TempUserTestResult.Where(t => t.UserTestId == userTestId).OrderBy(X => X.Id).Select((x) => new TempQuestionStatusVM()
                        {
                            QuestionId = x.QuestionId,
                            IsAttended = x.IsAttended,
                            UserAnswers = x.UserAnswers
                        }).ToList();

                        if (questions.Count == 0)
                        {
                            return new JsonResult(new ApiResponse<string>
                            {
                                Message = ResponseMessages.NoRecordsFound,
                                Result = false,
                                StatusCode = ResponseStatusCode.BadRequest
                            });
                        }

                        foreach (var item in questions)
                        {
                            totalCount++;
                            int status = 0;
                            unAnswered = unAnswered + IsQuestionUnanswered(item);
                            answered = answered + IsQuestionAnswered(item);
                            status = GetStatusOfQuestion(item);
                            data.Add(new QuestionStatusVM()
                            {
                                QuestionId = item.QuestionId,
                                Status = status,
                            });
                        }

                    }
                    CandidateQuestionsStatusVM candidateQuestionsStatusVM = new CandidateQuestionsStatusVM()
                    {
                        questionStatusVMs = data,
                        Answered = answered,
                        TotalQuestion = totalCount,
                        UnAnswered = unAnswered,
                        TimeLeft = timeRemaining,
                        IsQuestionsMenu = isQuestionsMenu
                    };
                    return new JsonResult(new ApiResponse<CandidateQuestionsStatusVM>
                    {
                        Data = candidateQuestionsStatusVM,
                        Message = ResponseMessages.Success,
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
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



        public async Task<JsonResult> EndTest(int userId)
        {
            try
            {
                if (userId != 0)
                {

                    using (var connection = new NpgsqlConnection(connectionString))
                    {
                        connection.Open();
                        var result = connection.Query("Select * from endtest(@user_id)", new { user_id = userId }).FirstOrDefault();
                        string jsonResult = result?.endtest;
                        connection.Close();
                        ApiResponse<string> response = JsonConvert.DeserializeObject<ApiResponse<string>>(jsonResult);
                        return new JsonResult(response);
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

        public async Task<JsonResult> GetInstructionsOfTheTestForUser(int userId, string testStatus)
        {
            try
            {
                if (userId != 0)
                {
                    Test? userTest = _userActiveTestHelper.GetTestOfUser(userId);
                    if (userTest != null)
                    {
                        if (testStatus == ModuleNames.StartTest)
                        {
                            StartTestVM startTestDetails = new StartTestVM()
                            {
                                EndTime = userTest.EndTime,
                                MessageAtStartOfTheTest = userTest.MessaageAtStartOfTheTest,
                                TestName = userTest.Name,
                                StartTime = userTest.StartTime,
                                TestDate = userTest.Date,
                                TestDurationInMinutes = userTest.TestDuration,
                                NegativeMarkingPoints = userTest.NegativeMarkingPercentage,
                                BasicPoints = userTest.BasicPoint
                            };
                            return new JsonResult(new ApiResponse<StartTestVM>
                            {
                                Data = startTestDetails,
                                Message = ResponseMessages.Success,
                                Result = true,
                                StatusCode = ResponseStatusCode.Success
                            });
                        }
                        return new JsonResult(new ApiResponse<string>
                        {
                            Data = userTest.MessaageAtEndOfTheTest,
                            Message = ResponseMessages.Success,
                            Result = true,
                            StatusCode = ResponseStatusCode.Success
                        });

                    }
                    else
                    {
                        return new JsonResult(new ApiResponse<string>
                        {
                            Message = string.Format(ResponseMessages.NotFound, ModuleNames.Test),
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
                    Message = ResponseMessages.BadRequest,
                    Result = false,
                    StatusCode = ResponseStatusCode.BadRequest
                });
            }
        }

        #endregion

        #region Helper Method
        private Test? GetTestOfUser(int userId)
        {
            int? groupId = _appDbContext.Users.Where(x => x.Id == userId).Select(x => x.GroupId).FirstOrDefault();
            if (groupId != null)
            {

                Test? test = _appDbContext.Tests.Where(x => x.GroupId == groupId && x.Status == (int)TestStatus.Active && x.IsDeleted == false).FirstOrDefault();
                if (test != null && Convert.ToDateTime(test?.EndTime) >= DateTime.Now && Convert.ToDateTime(test?.StartTime) <= DateTime.Now)
                {
                    return test;
                }
            }

            return null;
        }

        private static Dictionary<(int, QuestionType, int), int> setQuestionsConfig(List<TestWiseQuestionsCountVM> testWiseQuestionsCount)
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

        private static List<RandomQuestionsVM> SelectRandomQuestions(List<RandomQuestionsVM> allQuestions, Dictionary<(int, QuestionType, int), int> questionsPerMarkTypeTopicId)
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

        private void Shuffle<T>(List<T> list)
        {
            Random random = new Random();

            int n = list.Count;
            for (int i = n - 1; i > 0; i--)
            {
                int j = random.Next(0, i + 1);
                T temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
        }

        private static bool IsQuestionIdAndUserIdInvalid(int questionId, int userId)
        {
            if ((questionId != (int)Enums.DefaultQuestionId.QuestionId && questionId < 1) || (userId < 1))
            {
                return true;
            }
            return false;
        }

        private static int SetQuestionId(int questionId, int[]? questions)
        {
            if (questionId == (int)Enums.DefaultQuestionId.QuestionId && questions.Length > 0)
            {
                questionId = questions.FirstOrDefault();
            }
            return questionId;
        }

        private static bool IsAnswer(UserTestQuestionModelVM question, CandidateTestOptionsVM item)
        {
            if (Array.IndexOf(question.Answer, item.OptionId) > -1)
            {
                return true;
            }
            return false;
        }

        private static int GetStatusOfQuestion(TempQuestionStatusVM item)
        {
            int status = 0;
            if (item.IsAttended && item.UserAnswers == null)
            {
                status = (int)Enums.QuestionStatus.Skipped;
            }
            else if (item.IsAttended && item.UserAnswers != null)
            {
                status = (int)Enums.QuestionStatus.Answered;
            }
            return status;
        }

        private static int IsQuestionAnswered(TempQuestionStatusVM item)
        {
            if (item.IsAttended && item.UserAnswers != null)
            {
                return 1;
            }
            return 0;
        }

        private int IsQuestionUnanswered(TempQuestionStatusVM item)
        {
            if (item.IsAttended && item.UserAnswers == null)
            {
                return 1;
            }
            return 0;
        }

        #endregion
    }
}
