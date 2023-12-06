using AptitudeTest.Core.Entities.Questions;
using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using AptitudeTest.Data.Common;
using APTITUDETEST.Common.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AptitudeTest.Data.Data
{
    public class QuestionModuleRepository : IQuestionModuleRepository
    {
        #region Properties
        readonly AppDbContext _context;
        #endregion

        #region Constructor
        public QuestionModuleRepository(AppDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Methods
        public async Task<JsonResult> GetQuestionModules(string? searchQuery, int? filter, int? currentPageIndex, int? pageSize)
        {
            try
            {
                List<QuestionModule> questionModuleslist = await Task.FromResult(_context.QuestionModule.Where(s => s.IsDeleted == null || s.IsDeleted == false).ToList());

                if (searchQuery != null)
                {
                    string query = searchQuery.ToLower();
                    questionModuleslist = questionModuleslist.Where(module => module.Name.ToLower().Contains(query)).ToList();
                }

                if (filter != null)
                {
                    if (filter == 1)
                    {
                        questionModuleslist = questionModuleslist.Where(stream => stream.Status == true).ToList();
                    }
                    if (filter == 2)
                    {
                        questionModuleslist = questionModuleslist.Where(stream => stream.Status == false).ToList();
                    }
                }

                List<QuestionModuleVM> questionModuleData = questionModuleslist.Select(questionModule => new QuestionModuleVM()
                {
                    Id = questionModule.Id,
                    Name = questionModule.Name,
                    Status = questionModule.Status,

                }).ToList();

                PaginationVM<QuestionModuleVM> paginatedData = Pagination<QuestionModuleVM>.Paginate(questionModuleData, pageSize, currentPageIndex);
                return new JsonResult(new ApiResponse<PaginationVM<QuestionModuleVM>>
                {
                    Data = paginatedData,
                    Message = ResponseMessages.Success,
                    Result = true,
                    StatusCode = ResponseStatusCode.Success
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

        public async Task<JsonResult> Create(QuestionModuleVM questionModuleVM)
        {
            try
            {
                QuestionModule? questionModules = _context.QuestionModule.Where(m => m.Name.ToLower() == questionModuleVM.Name.ToLower() && m.IsDeleted != true).FirstOrDefault();
                if (questionModules != null)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.AlreadyExists, "Module"),
                        Result = false,
                        StatusCode = ResponseStatusCode.AlreadyExist
                    });
                }

                QuestionModule questionModule = new QuestionModule()
                {
                    Status = questionModuleVM.Status,
                    Name = questionModuleVM.Name,
                    CreatedBy = questionModuleVM.CreatedBy
                };

                _context.Add(questionModule);
                _context.SaveChanges();

                return new JsonResult(new ApiResponse<string>
                {
                    Message = string.Format(ResponseMessages.AddSuccess, "Module"),
                    Result = true,
                    StatusCode = ResponseStatusCode.Success
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

        public async Task<JsonResult> Update(QuestionModuleVM questionModuleVM)
        {
            try
            {
                QuestionModule? questionModule = _context.QuestionModule.Where(s => s.Name.ToLower() == questionModuleVM.Name.ToLower() && s.Id != questionModuleVM.Id && s.IsDeleted != true).FirstOrDefault();
                if (questionModule != null)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.AlreadyExists, "Module"),
                        Result = false,
                        StatusCode = ResponseStatusCode.AlreadyExist
                    });
                }

                questionModule = await Task.FromResult(_context.QuestionModule.AsNoTracking().Where(l => l.Id == questionModuleVM.Id && l.IsDeleted != true).FirstOrDefault());
                if (questionModule != null)
                {
                    questionModule.Status = questionModuleVM.Status;
                    questionModule.UpdatedBy = questionModuleVM.UpdatedBy;
                    questionModule.Name = questionModuleVM.Name;
                    questionModule.UpdatedDate = DateTime.UtcNow;
                    _context.Update(questionModule);
                    _context.SaveChanges();

                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.UpdateSuccess, "Module"),
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }

                return new JsonResult(new ApiResponse<string>
                {
                    Message = string.Format(ResponseMessages.NotFound, "Module"),
                    Result = false,
                    StatusCode = ResponseStatusCode.NotFound
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

        public async Task<JsonResult> Get(int id)
        {
            try
            {
                QuestionModule? questionModule = await Task.FromResult(_context.QuestionModule.Where(s => s.IsDeleted != true && s.Id == id).FirstOrDefault());

                if (questionModule == null)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.NotFound, "Module"),
                        Result = false,
                        StatusCode = ResponseStatusCode.NotFound
                    });
                }
                QuestionModuleVM questionModuleVM = new QuestionModuleVM() { Id = questionModule.Id, Name = questionModule.Name, Status = questionModule.Status };

                return new JsonResult(new ApiResponse<QuestionModuleVM>
                {
                    Data = questionModuleVM,
                    Message = ResponseMessages.Success,
                    Result = true,
                    StatusCode = ResponseStatusCode.Success
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

                QuestionModule? questionModule = await Task.FromResult(_context.QuestionModule.Where(s => s.Id == id && s.IsDeleted == false).FirstOrDefault());
                if (questionModule != null)
                {
                    questionModule.IsDeleted = true;
                    _context.Update(questionModule);
                    _context.SaveChanges();
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.DeleteSuccess, "Module"),
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }
                return new JsonResult(new ApiResponse<string>
                {
                    Message = string.Format(ResponseMessages.NotFound, "Module"),
                    Result = false,
                    StatusCode = ResponseStatusCode.NotFound
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
                return new JsonResult(new ApiResponse<List<QuestionMarks>>
                {
                    Data = questionMarks,
                    Message = ResponseMessages.Success,
                    Result = true,
                    StatusCode = ResponseStatusCode.Success
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

        public async Task<JsonResult> AddQuestionMarks(QuestionMarks newMark)
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
    }
}
