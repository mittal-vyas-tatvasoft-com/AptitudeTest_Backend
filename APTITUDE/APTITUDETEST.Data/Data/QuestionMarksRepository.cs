using AptitudeTest.Core.Entities.Questions;
using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using AptitudeTest.Data.Common;
using APTITUDETEST.Common.Data;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Data.Data
{
    public class QuestionMarksRepository : IQuestionMarksRepository
    {
        #region Properties
        readonly AppDbContext _context;
        private readonly ILoggerManager _logger;
        #endregion

        #region Constructor
        public QuestionMarksRepository(AppDbContext context, ILoggerManager logger)
        {
            _context = context;
            _logger = logger;
        }
        #endregion

        #region Methods
        public async Task<JsonResult> GetAllQuestionMarks(string? searchQuery, int? currentPageIndex, int? pageSize)
        {
            try
            {
                List<QuestionMarks> questionMarks;
                if (!string.IsNullOrEmpty(searchQuery))
                {
                    questionMarks = _context.QuestionMarks.Where(QM => QM.Marks.Equals(searchQuery)).ToList();
                }
                else
                {
                    questionMarks = _context.QuestionMarks.ToList();
                }
                PaginationVM<QuestionMarks> paginatedData = Pagination<QuestionMarks>.Paginate(questionMarks, pageSize, currentPageIndex);
                return new JsonResult(new ApiResponse<PaginationVM<QuestionMarks>>
                {
                    Data = paginatedData,
                    Message = ResponseMessages.Success,
                    Result = true,
                    StatusCode = ResponseStatusCode.Success
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in QuestionMarksRepository.GetAllQuestionMarks : {ex}");
                return new JsonResult(new ApiResponse<string>
                {
                    Message = ResponseMessages.InternalError,
                    Result = false,
                    StatusCode = ResponseStatusCode.InternalServerError
                });
            }
        }

        public async Task<JsonResult> Create(QuestionMarks newMark)
        {

            try
            {
                if (newMark != null)
                {
                    QuestionMarks? markAlreadyExist = await Task.FromResult(_context.QuestionMarks.Where(qm => qm.Marks == newMark.Marks).FirstOrDefault());
                    if (markAlreadyExist == null)
                    {
                        _context.Add(newMark);
                        _context.SaveChanges();

                        return new JsonResult(new ApiResponse<string>
                        {
                            Message = string.Format(ResponseMessages.AddSuccess, "Mark"),
                            Result = true,
                            StatusCode = ResponseStatusCode.Success
                        });
                    }
                    else
                    {
                        return new JsonResult(new ApiResponse<string>
                        {
                            Message = string.Format(ResponseMessages.AlreadyExists, "Marks"),
                            Result = false,
                            StatusCode = ResponseStatusCode.AlreadyExist
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
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in QuestionMarksRepository.Create : {ex}");
                return new JsonResult(new ApiResponse<string>
                {
                    Message = ResponseMessages.InternalError,
                    Result = false,
                    StatusCode = ResponseStatusCode.InternalServerError
                });
            }
        }

        public async Task<JsonResult> Update(QuestionMarks updatedMark)
        {
            try
            {
                QuestionMarks? questionMark = _context.QuestionMarks.Where(qm => qm.Marks == updatedMark.Marks && qm.Id != updatedMark.Id && qm.IsDeleted != true).FirstOrDefault();
                if (questionMark != null)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.AlreadyExists, "Mark"),
                        Result = false,
                        StatusCode = ResponseStatusCode.AlreadyExist
                    });
                }

                QuestionMarks? questionMarkToBeUpdated = await Task.FromResult(_context.QuestionMarks.Where(qm => qm.Id == updatedMark.Id && qm.IsDeleted != true).FirstOrDefault());
                if (questionMarkToBeUpdated != null)
                {
                    questionMarkToBeUpdated.Status = updatedMark.Status;
                    questionMarkToBeUpdated.UpdatedBy = updatedMark.UpdatedBy;
                    questionMarkToBeUpdated.Marks = updatedMark.Marks;
                    questionMarkToBeUpdated.UpdatedDate = DateTime.UtcNow;
                    _context.Update(questionMarkToBeUpdated);
                    _context.SaveChanges();

                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.UpdateSuccess, "Mark"),
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }

                return new JsonResult(new ApiResponse<string>
                {
                    Message = string.Format(ResponseMessages.NotFound, "Mark"),
                    Result = false,
                    StatusCode = ResponseStatusCode.NotFound
                });
            }

            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in QuestionMarksRepository.Update : {ex}");
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
                if (id == 0)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = ResponseMessages.BadRequest,
                        Result = false,
                        StatusCode = ResponseStatusCode.BadRequest
                    });
                }

                QuestionMarks? questionMark = await Task.FromResult(_context.QuestionMarks.Where(s => s.Id == id && s.IsDeleted == false).FirstOrDefault());
                if (questionMark != null)
                {
                    _context.Remove(questionMark);
                    _context.SaveChanges();
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.DeleteSuccess, "Mark"),
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }
                return new JsonResult(new ApiResponse<string>
                {
                    Message = string.Format(ResponseMessages.NotFound, "Mark"),
                    Result = false,
                    StatusCode = ResponseStatusCode.NotFound
                });
            }

            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in QuestionMarksRepository.Delete : {ex}");
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
