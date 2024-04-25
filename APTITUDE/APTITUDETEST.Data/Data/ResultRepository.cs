using AptitudeTest.Core.Entities.Candidate;
using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using AptitudeTest.Data.Common;
using APTITUDETEST.Common.Data;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Web;

namespace AptitudeTest.Data.Data
{
    public class ResultRepository : IResultRepository
    {
        #region Properties
        private readonly string? connectionString;
        private readonly ILoggerManager _logger;
        private AppDbContext _context;
        #endregion

        #region Constructor
        public ResultRepository(AppDbContext context, IConfiguration config, ILoggerManager logger)
        {
            IConfiguration _config;
            _config = config;
            connectionString = _config["ConnectionStrings:AptitudeTest"];
            _logger = logger;
            _context = context;
        }
        #endregion

        #region Methods
        public async Task<JsonResult> Get(int userId, int testId, int marks, int pageSize, int pageIndex, bool onlyCorrect)
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
                    var data = await connection.Connection.QueryAsync<UserTestResultDataVM>("select * from getUserResults_2(@userId,@testId)", new { userId = userId, testId = testId });
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

                    string userName = "";
                    List<UserResultQuestionVM> userResultQuestionVMList;

                    userResultQuestionVMList = data.GroupBy(q => q.QuestionId).Select(x =>
                    {
                        var temp = x.FirstOrDefault();
                        userName = temp.Name;
                        return new UserResultQuestionVM()
                        {
                            Difficulty = temp.Difficulty,
                            TimeSpentInSeconds = temp.TimeSpent,
                            Id = temp.QuestionId,
                            OptionType = temp.OptionType,
                            QuestionText = HttpUtility.HtmlDecode(temp.QuestionText),
                            Options = x.Select(k =>
                            {
                                return new UserResultOptionVM() { OptionId = k.OptionId, IsAnswer = k.IsAnswer, OptionValue = k.OptionData, IsUserAnswer = k.UserAnswers != null ? k.UserAnswers.Contains(k.OptionId) : false };

                            }).ToList(),
                            UserAnswers = temp.UserAnswers == null ? null : temp.UserAnswers.ToList()

                        };
                    }).ToList();

                    foreach (var item in userResultQuestionVMList)
                    {
                        int hours = Convert.ToInt32(item.TimeSpentInSeconds / 3600);
                        int minutes = Convert.ToInt32((item.TimeSpentInSeconds % 3600) / 60);
                        int seconds = Convert.ToInt32(item.TimeSpentInSeconds % 60);
                        if (hours == 0 && minutes == 0 && seconds > 0)
                        {
                            item.TimeSpent = string.Format(ResponseMessages.Seconds, seconds);
                        }
                        else if (hours == 0 && minutes > 0 && seconds == 0)
                        {
                            item.TimeSpent = string.Format(ResponseMessages.Minutes, minutes);
                        }
                        else if (hours > 0 && minutes == 0 && seconds == 0)
                        {
                            item.TimeSpent = string.Format(ResponseMessages.Hours, hours);
                        }
                        else if (hours > 0 && minutes > 0 && seconds == 0)
                        {
                            item.TimeSpent = string.Format(ResponseMessages.HoursMinutes, hours, minutes);
                        }
                        else if (hours > 0 && minutes == 0 && seconds > 0)
                        {
                            item.TimeSpent = string.Format(ResponseMessages.HoursSeconds, hours, seconds);
                        }
                        else if (hours == 0 && minutes > 0 && seconds > 0)
                        {
                            item.TimeSpent = string.Format(ResponseMessages.MinutesSeconds, minutes, seconds);
                        }
                        else if (hours > 0 && minutes > 0 && seconds > 0)
                        {
                            item.TimeSpent = string.Format(ResponseMessages.HourMinuteSecond, hours, minutes, seconds);
                        }
                    }
                    UserResultsVM tempData = getQuestionCount(userResultQuestionVMList);
                    if (marks != 0)
                    {
                        userResultQuestionVMList = userResultQuestionVMList.Where(x => x.Difficulty == marks).ToList();
                    }
                    if (onlyCorrect)
                    {
                        userResultQuestionVMList = userResultQuestionVMList.Where(x => isCorrect(x)).ToList();
                    }
                    PaginationVM<UserResultQuestionVM> paginatedData = new PaginationVM<UserResultQuestionVM>()
                    {
                        EntityList = userResultQuestionVMList.Skip(pageIndex * pageSize).Take(pageSize).ToList(),
                    };

                    UserResultsVM userResultsVM = new UserResultsVM()
                    {
                        AllCorrectQuestionCount = tempData.AllCorrectQuestionCount,
                        AllQuestionCount = tempData.AllQuestionCount,
                        Marks5CorrectQuestionCount = tempData.Marks5CorrectQuestionCount,
                        Marks1CorrectQuestionCount = tempData.Marks1CorrectQuestionCount,
                        Marks1QuestionCount = tempData.Marks1QuestionCount,
                        Marks2CorrectQuestionCount = tempData.Marks2CorrectQuestionCount,
                        Marks2QuestionCount = tempData.Marks2QuestionCount,
                        Marks3CorrectQuestionCount = tempData.Marks3CorrectQuestionCount,
                        Marks3QuestionCount = tempData.Marks3QuestionCount,
                        Marks4CorrectQuestionCount = tempData.Marks4CorrectQuestionCount,
                        Marks4QuestionCount = tempData.Marks4QuestionCount,
                        Marks5QuestionCount = tempData.Marks5QuestionCount,
                        Name = userName,
                        UserId = userId,
                        PaginatedData = paginatedData
                    };

                    return new JsonResult(new ApiResponse<UserResultsVM>
                    {
                        Data = userResultsVM,
                        Message = ResponseMessages.Success,
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }
            }

            catch (Exception ex)
            {
                _logger.LogError($"Error occurred for userId : {userId} in ResultRepository.Get \n MESSAGE : {ex.Message} \n INNER EXCEPTION : {ex.InnerException} \n");
                return new JsonResult(new ApiResponse<string>
                {
                    Message = ResponseMessages.InternalError,
                    Result = false,
                    StatusCode = ResponseStatusCode.InternalServerError
                });
            }
        }

        public async Task<JsonResult> GetResults(string? searchQuery, int? TestId, int? GroupId, int? CollegeId, int? Year, int? currentPageIndex, int? pageSize, string? sortField, string? sortOrder)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    List<ResultsVM> data = connection.Query<ResultsVM>("Select * from getallresults(@SearchQuery,@GroupId,@CollegeId,@TestId,@YearAttended,@PageNumber,@PageSize,@SortField,@SortOrder)", new { SearchQuery = searchQuery, GroupId = (object)GroupId!, CollegeId = (object)CollegeId!, TestId = (object)TestId!, YearAttended = Year, PageNumber = currentPageIndex, PageSize = pageSize, SortField = sortField, SortOrder = sortOrder }).ToList();
                    var userTests = _context.TempUserTests.Select(x => new { x.UserId, x.TestStartTime, x.IsStartTimeUpdated }).ToList();
                    int i = (int)currentPageIndex * (int)pageSize;
                    foreach (var item in data)
                    {
                        var userTestData = userTests.FirstOrDefault(x => x.UserId == item.UserId);
                        if (userTestData.IsStartTimeUpdated)
                        {
                            item.StartTime = userTestData.TestStartTime;
                        }
                        item.Index = i;
                        i++;
                    }
                    return new JsonResult(new ApiResponse<List<ResultsVM>>
                    {
                        Data = data,
                        Message = ResponseMessages.Success,
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }

            }

            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in ResultRepository.GetResults \n MESSAGE : {ex.Message} \n INNER EXCEPTION : {ex.InnerException} \n");
                return new JsonResult(new ApiResponse<string>
                {
                    Message = ResponseMessages.InternalError,
                    Result = false,
                    StatusCode = ResponseStatusCode.InternalServerError
                });
            }

        }

        public async Task<JsonResult> GetResultStatistics(string? searchQuery, int? TestId, int? GroupId, int? CollegeId, int? Year, int? currentPageIndex, string? sortField, string? sortOrder)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    List<ResultStatisticsVM> data = connection.Query<ResultStatisticsVM>("Select * from getstatisticsresult(@SearchQuery,@GroupId,@CollegeId,@TestId,@YearAttended,@PageNumber,@SortField,@SortOrder)", new { SearchQuery = searchQuery, GroupId = (object)GroupId!, CollegeId = (object)CollegeId!, TestId = (object)TestId!, YearAttended = Year, PageNumber = currentPageIndex, SortField = sortField, SortOrder = sortOrder }).ToList();
                    return new JsonResult(new ApiResponse<List<ResultStatisticsVM>>
                    {
                        Data = data,
                        Message = ResponseMessages.Success,
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"\nError occurred in ResultRepository.GetResultStatistics \n MESSAGE : {ex.Message} \n INNER EXCEPTION : {ex.InnerException} \n");
                return new JsonResult(new ApiResponse<string>
                {
                    Message = ResponseMessages.InternalError,
                    Result = false,
                    StatusCode = ResponseStatusCode.InternalServerError
                });
            }

        }

        public async Task<JsonResult> GetResultExportData(string? searchQuery, int? TestId, int? GroupId, int? CollegeId, int? Year, int? currentPageIndex, int? pageSize, string? sortField, string? sortOrder)
        {
            try
            {
                searchQuery = string.IsNullOrEmpty(searchQuery) ? null : searchQuery;
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    List<ResultExportDataVM> data = connection.Query<ResultExportDataVM>("Select * from getallresultsexport(@SearchQuery,@GroupId,@CollegeId,@TestId,@YearAttended,@PageNumber,@PageSize,@SortField,@SortOrder)", new { SearchQuery = searchQuery, GroupId = (object)GroupId!, CollegeId = (object)CollegeId!, TestId = (object)TestId!, YearAttended = Year, PageSize = pageSize, PageNumber = currentPageIndex, SortField = sortField, SortOrder = sortOrder }).ToList();
                    if (!data.Any())
                    {
                        return new JsonResult(new ApiResponse<List<ResultExportDataVM>>
                        {
                            Data = data,
                            Message = ResponseMessages.NoRecordsFound,
                            Result = true,
                            StatusCode = ResponseStatusCode.NotFound
                        });
                    }

                    return new JsonResult(new ApiResponse<List<ResultExportDataVM>>
                    {
                        Data = data,
                        Message = ResponseMessages.Success,
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in ResultRepository.GetResultExportData \n MESSAGE : {ex.Message} \n INNER EXCEPTION : {ex.InnerException} \n");
                return new JsonResult(new ApiResponse<string>
                {
                    Message = ResponseMessages.InternalError,
                    Result = false,
                    StatusCode = ResponseStatusCode.InternalServerError
                });
            }
        }

        public async Task<JsonResult> ApproveResumeTest(TestApproveVM testApproveVM)
        {
            try
            {
                if (testApproveVM.UserId < 1 || testApproveVM.TestId < 1)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = ResponseMessages.BadRequest,
                        Result = false,
                        StatusCode = ResponseStatusCode.BadRequest
                    });
                }

                TempUserTest tempUserTest = _context.TempUserTests.Where(t => t.UserId == testApproveVM.UserId && t.TestId == testApproveVM.TestId).FirstOrDefault();
                if (tempUserTest == null)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.NotFound, ModuleNames.UserTest),
                        Result = false,
                        StatusCode = ResponseStatusCode.NotFound
                    });
                }
                if (tempUserTest.IsFinished)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = ResponseMessages.TestSubmitted,
                        Result = false,
                        StatusCode = ResponseStatusCode.RequestFailed
                    });
                }
                //int duration = _context.Tests.Where(t => t.Id == testApproveVM.TestId).Select(t => t.TestDuration).FirstOrDefault();
                var test = _context.Tests.Where(t => t.Id == testApproveVM.TestId).FirstOrDefault();
                if (test.TestDuration != 0)
                {
                    if (testApproveVM.RemainingTimeInMinutes <= test.TestDuration)
                    {
                        tempUserTest.IsAdminApproved = true;
                        var remainingTimeOfTest = Convert.ToInt32((test.EndTime - DateTime.Now).TotalMinutes);
                        if (testApproveVM.RemainingTimeInMinutes > remainingTimeOfTest)
                        {
                            tempUserTest.TimeRemaining = remainingTimeOfTest * 60;
                        }
                        else if (testApproveVM.RemainingTimeInMinutes <= remainingTimeOfTest)
                        {
                            tempUserTest.TimeRemaining = testApproveVM.RemainingTimeInMinutes * 60;
                        }
                        _context.Update(tempUserTest);
                        _context.SaveChanges();
                        return new JsonResult(new ApiResponse<string>
                        {
                            Message = string.Format(ResponseMessages.StatusUpdateSuccess, ModuleNames.Approval),
                            Result = true,
                            StatusCode = ResponseStatusCode.Success
                        });
                    }
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = ResponseMessages.DurationExceeds,
                        Result = false,
                        StatusCode = ResponseStatusCode.NotAcceptable
                    });

                }

                return new JsonResult(new ApiResponse<string>
                {
                    Message = string.Format(ResponseMessages.NotFound, ModuleNames.Test),
                    Result = true,
                    StatusCode = ResponseStatusCode.NotFound
                });
            }

            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in ResultRepository.ApproveResumeTest \n MESSAGE : {ex.Message} \n INNER EXCEPTION : {ex.InnerException} \n");
                return new JsonResult(new ApiResponse<string>
                {
                    Message = ResponseMessages.InternalError,
                    Result = false,
                    StatusCode = ResponseStatusCode.InternalServerError
                });
            }
        }

        public async Task<JsonResult> GetApproveTestData(int userId, int testId)
        {
            try
            {
                if (userId < 1 || testId < 1)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = ResponseMessages.BadRequest,
                        Result = false,
                        StatusCode = ResponseStatusCode.BadRequest
                    });
                }

                TempUserTest tempUserTest = _context.TempUserTests.Where(t => t.UserId == userId && t.TestId == testId).FirstOrDefault();
                if (tempUserTest == null)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.NotFound, ModuleNames.UserTest),
                        Result = false,
                        StatusCode = ResponseStatusCode.NotFound
                    });
                }
                if (tempUserTest.IsFinished)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = ResponseMessages.TestSubmitted,
                        Result = false,
                        StatusCode = ResponseStatusCode.RequestFailed
                    });
                }
                int duration = _context.Tests.Where(t => t.Id == testId).Select(t => t.TestDuration).FirstOrDefault();
                if (duration != 0)
                {
                    return new JsonResult(new ApiResponse<TestApproveVM>
                    {
                        Data = new TestApproveVM() { duration = duration, RemainingTimeInMinutes = tempUserTest.TimeRemaining, UserId = userId, TestId = testId },
                        Message = string.Format(ResponseMessages.StatusUpdateSuccess, ModuleNames.Approval),
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }

                return new JsonResult(new ApiResponse<string>
                {
                    Message = string.Format(ResponseMessages.NotFound, ModuleNames.Test),
                    Result = true,
                    StatusCode = ResponseStatusCode.NotFound
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in ResultRepository.GetApproveTestData \n MESSAGE : {ex.Message} \n INNER EXCEPTION : {ex.InnerException} \n");
                return new JsonResult(new ApiResponse<string>
                {
                    Message = ResponseMessages.InternalError,
                    Result = false,
                    StatusCode = ResponseStatusCode.InternalServerError
                });
            }
        }

        public async Task<JsonResult> UpdateTestRemainingTime(List<UserTestVM> userTests, int timeToBeAdded)
        {
            try
            {
                if (userTests != null && userTests.Count > 0 && timeToBeAdded >= 0)
                {
                    int testTimeUpdated = 0;
                    foreach (var userTest in userTests)
                    {
                        var test = _context.Tests.FirstOrDefault(test => test.Id == userTest.TestId && !(bool)test.IsDeleted);
                        TempUserTest? testOfUser = _context.TempUserTests.FirstOrDefault(test => test.TestId == userTest.TestId && test.UserId == userTest.UserId && !(bool)test.IsDeleted);
                        var remainingTimeToEnd = Convert.ToInt32((test.EndTime - DateTime.Now).TotalMinutes);
                        int timeAfterAddition = (testOfUser.TimeRemaining + timeToBeAdded) / 60;
                        if (testOfUser != null && !testOfUser.IsFinished && remainingTimeToEnd > 0)
                        {

                            if (timeAfterAddition >= remainingTimeToEnd)
                            {
                                testOfUser.TimeRemaining = remainingTimeToEnd * 60;
                            }
                            else
                            {
                                testOfUser.TimeRemaining = timeAfterAddition * 60;
                            }
                            testOfUser.IsAdminApproved = true;
                            await _context.SaveChangesAsync();
                            testTimeUpdated++;
                        }
                    }
                    if (testTimeUpdated > 0)
                    {
                        return new JsonResult(new ApiResponse<string>
                        {
                            Message = string.Format(ResponseMessages.TestTimeUpdated, testTimeUpdated),
                            Result = true,
                            StatusCode = ResponseStatusCode.Success
                        });
                    }
                    else
                    {
                        return new JsonResult(new ApiResponse<string>
                        {
                            Message = ResponseMessages.TestTimeNotUpdated,
                            Result = false,
                            StatusCode = ResponseStatusCode.RequestFailed
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
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in CandidateRepository.UpdateTestRemainingTime \n MESSAGE : {ex.Message} \n INNER EXCEPTION : {ex.InnerException} \n");
                return new JsonResult(new ApiResponse<string>
                {
                    Message = ResponseMessages.InternalError,
                    Result = false,
                    StatusCode = ResponseStatusCode.InternalServerError
                });
            }
        }

        public async Task<JsonResult> GetGroupOfTest(int testId)
        {
            try
            {
                if (testId <= 0)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.NotFound, ModuleNames.Test),
                        Result = true,
                        StatusCode = ResponseStatusCode.NotFound
                    });
                }
                int? groupIdOfTest = _context.Tests.Where(x => x.Id == testId && !(bool)x.IsDeleted).Select(x => x.GroupId).FirstOrDefault();

                if (groupIdOfTest != null)
                {
                    return new JsonResult(new ApiResponse<int?>
                    {
                        Data = groupIdOfTest,
                        Message = ResponseMessages.Success,
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }
                else
                {
                    return new JsonResult(new ApiResponse<int?>
                    {
                        Data = 0,
                        Message = ResponseMessages.Success,
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in ResultRepository.GetApproveTestData \n MESSAGE : {ex.Message} \n INNER EXCEPTION : {ex.InnerException} \n");
                return new JsonResult(new ApiResponse<string>
                {
                    Message = ResponseMessages.InternalError,
                    Result = false,
                    StatusCode = ResponseStatusCode.InternalServerError
                });
            }
        }

        public async Task<JsonResult> ReverseLockedTests(ReverseLockedTestVM reverseLockedTestVM)
        {
            try
            {
                int count = 0;
                using (DbConnection connection = new DbConnection())
                {
                    count = await connection.Connection.ExecuteScalarAsync<int>("select * from reverselockedtest(@user_ids,@test_id)", new { user_ids = reverseLockedTestVM.UserIds, test_id = reverseLockedTestVM.TestId });
                }
                return new JsonResult(new ApiResponse<int>
                {
                    Data = count,
                    Message = string.Format(ResponseMessages.StatusUpdateSuccess, ModuleNames.Approval),
                    Result = false,
                    StatusCode = ResponseStatusCode.Success
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in ResultRepository.ReverseLockedTests \n MESSAGE : {ex.Message} \n INNER EXCEPTION : {ex.InnerException} \n");
                return new JsonResult(new ApiResponse<string>
                {
                    Message = ResponseMessages.InternalError,
                    Result = false,
                    StatusCode = ResponseStatusCode.InternalServerError
                });
            }
        }
        #endregion

        #region Helper Methods

        private UserResultsVM getQuestionCount(List<UserResultQuestionVM> data)
        {
            int totalCount = 0;
            int totalCorrectCount = 0;
            int total1MarkCorrectCount = 0;
            int total2MarkCorrectCount = 0;
            int total3MarkCorrectCount = 0;
            int total4MarkCorrectCount = 0;
            int total5MarkCorrectCount = 0;
            foreach (var item in data)
            {
                totalCount++;
                List<int> correctAnswers = item.Options.Where(o => o.IsAnswer).Select(o => o.OptionId).ToList();
                if (CompareList(item.UserAnswers, correctAnswers))
                {
                    totalCorrectCount++;
                    switch (item.Difficulty)
                    {
                        case 1:
                            total1MarkCorrectCount++;
                            break;
                        case 2:
                            total2MarkCorrectCount++;
                            break;
                        case 3:
                            total3MarkCorrectCount++;
                            break;
                        case 4:
                            total4MarkCorrectCount++;
                            break;
                        case 5:
                            total5MarkCorrectCount++;
                            break;
                    }
                }
            }

            int total1MarkCount = data.Count(x => x.Difficulty == 1);
            int total2MarkCount = data.Count(x => x.Difficulty == 2);
            int total3MarkCount = data.Count(x => x.Difficulty == 3);
            int total4MarkCount = data.Count(x => x.Difficulty == 4);
            int total5MarkCount = data.Count(x => x.Difficulty == 5);

            return new UserResultsVM()
            {
                AllCorrectQuestionCount = totalCorrectCount,
                AllQuestionCount = totalCount,

                Marks1QuestionCount = total1MarkCount,
                Marks2QuestionCount = total2MarkCount,
                Marks3QuestionCount = total3MarkCount,
                Marks4QuestionCount = total4MarkCount,
                Marks5QuestionCount = total5MarkCount,
                Marks1CorrectQuestionCount = total1MarkCorrectCount,
                Marks2CorrectQuestionCount = total2MarkCorrectCount,
                Marks3CorrectQuestionCount = total3MarkCorrectCount,
                Marks4CorrectQuestionCount = total4MarkCorrectCount,
                Marks5CorrectQuestionCount = total5MarkCorrectCount
            };



        }

        private bool CompareList(List<int> list1, List<int> list2)
        {
            if ((list1 == null && list2 != null) || (list1 != null && list2 == null))
            {
                return false;
            }

            if (list1 == null && list2 == null)
            {
                return true;
            }

            bool result = list1.Count == list2.Count;
            if (result)
            {
                foreach (var item in list1)
                {
                    if (!list2.Contains(item))
                    {
                        return false;
                    }
                }

                foreach (var item in list2)
                {
                    if (!list1.Contains(item))
                    {
                        return false;
                    }
                }
            }
            return result;
        }

        private bool isCorrect(UserResultQuestionVM question)
        {
            List<int> correctAnswers = question.Options.Where(o => o.IsAnswer).Select(o => o.OptionId).ToList();
            return CompareList(question.UserAnswers, correctAnswers);
        }

        #endregion
    }
}
