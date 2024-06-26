﻿using AptitudeTest.Core.Entities.Master;
using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using AptitudeTest.Data.Common;
using APTITUDETEST.Common.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AptitudeTest.Data.Data
{
    public class CollegeRepository : ICollegeRepository
    {

        private readonly AppDbContext _context;
        private readonly ILoggerManager _logger;


        public CollegeRepository(AppDbContext context, ILoggerManager logger)
        {
            _context = context;
            _logger = logger;
        }

        #region Methods
        public async Task<JsonResult> GetColleges(int? currentPageIndex, int? pageSize, string? sortField, string? sortOrder)
        {

            try
            {
                List<MasterCollege> collegeList = await Task.FromResult(_context.MasterCollege.Where(x => x.IsDeleted == null || x.IsDeleted == false).OrderByDescending(x => x.CreatedDate).ToList());

                switch (sortField)
                {
                    case "Name":
                        switch (sortOrder)
                        {
                            case "asc":
                                collegeList = collegeList.OrderBy(x => x.Name).ToList();
                                break;
                            case "desc":
                                collegeList = collegeList.OrderByDescending(x => x.Name).ToList();
                                break;
                        }
                        break;

                    case "Abbreviation":
                        switch (sortOrder)
                        {
                            case "asc":
                                collegeList = collegeList.OrderBy(x => x.Abbreviation).ToList();
                                break;
                            case "desc":
                                collegeList = collegeList.OrderByDescending(x => x.Abbreviation).ToList();
                                break;
                        }
                        break;
                }
                PaginationVM<MasterCollege> paginatedData = Pagination<MasterCollege>.Paginate(collegeList, pageSize, currentPageIndex);



                return new JsonResult(new ApiResponse<PaginationVM<MasterCollege>>
                {
                    Data = paginatedData,
                    Message = ResponseMessages.Success,
                    Result = true,
                    StatusCode = ResponseStatusCode.Success
                });
            }

            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in CollegeRepository.GetColleges \n MESSAGE : {ex.Message} \n INNER EXCEPTION : {ex.InnerException} \n");
                return new JsonResult(new ApiResponse<string>
                {
                    Message = ResponseMessages.InternalError,
                    Result = false,
                    StatusCode = ResponseStatusCode.InternalServerError
                });
            }

        }

        public async Task<JsonResult> GetActiveColleges()
        {

            try
            {
                var collegeList = await Task.FromResult(_context.MasterCollege
                .Where(x => (x.IsDeleted == null || x.IsDeleted == false) && x.Status == true)
                .Select(x => new { Id = x.Id, Name = x.Name })
                .ToList().OrderByDescending(x => x.Id));

                if (collegeList != null)
                {
                    return new JsonResult(new ApiResponse<IEnumerable<object>>
                    {
                        Data = collegeList,
                        Message = ResponseMessages.Success,
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }
                else
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Data = string.Format(ResponseMessages.NotFound, "College"),
                        Message = ResponseMessages.Success,
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in CollegeRepository.GetActiveColleges \n MESSAGE : {ex.Message} \n INNER EXCEPTION : {ex.InnerException} \n");
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
                if (id == 0)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = ResponseMessages.BadRequest,
                        Result = false,
                        StatusCode = ResponseStatusCode.BadRequest
                    });
                }
                MasterCollege? masterCollege = await Task.FromResult(_context.MasterCollege.Where(x => x.IsDeleted != true && x.Id == id).FirstOrDefault());
                if (masterCollege == null)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.NotFound, ModuleNames.College),
                        Result = false,
                        StatusCode = ResponseStatusCode.NotFound
                    });
                }

                CollegeVM collegeData = new CollegeVM()
                {
                    Id = masterCollege.Id,
                    Name = masterCollege.Name,
                    Abbreviation = masterCollege.Abbreviation,
                    Status = masterCollege.Status,
                };

                return new JsonResult(new ApiResponse<CollegeVM>
                {
                    Data = collegeData,
                    Message = ResponseMessages.Success,
                    Result = true,
                    StatusCode = ResponseStatusCode.Success
                });
            }

            catch (Exception ex)
            {
                _logger.LogError($"Error occurred for Id : {id} in CollegeRepository.Get \n MESSAGE : {ex.Message} \n INNER EXCEPTION : {ex.InnerException}\n");
                return new JsonResult(new ApiResponse<string>
                {
                    Message = ResponseMessages.InternalError,
                    Result = false,
                    StatusCode = ResponseStatusCode.InternalServerError
                });
            }
        }


        public async Task<JsonResult> Create(CollegeVM collegeToUpsert)
        {

            try
            {
                MasterCollege college = new MasterCollege();
                MasterCollege? masterCollege = _context.MasterCollege.Where(c => c.Name.Trim().ToLower() == collegeToUpsert.Name.Trim().ToLower() && c.IsDeleted != true).FirstOrDefault();
                if (masterCollege != null)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.AlreadyExists, ModuleNames.College),
                        Result = false,
                        StatusCode = ResponseStatusCode.AlreadyExist
                    });
                }
                MasterCollege? masterCollegeWithSameAbbreviation = _context.MasterCollege.Where(c => c.Abbreviation.Trim().ToLower() == collegeToUpsert.Abbreviation.Trim().ToLower() && c.IsDeleted != true).FirstOrDefault();

                if (masterCollegeWithSameAbbreviation != null)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.AlreadyExists, ModuleNames.Abbreviation),
                        Result = false,
                        StatusCode = ResponseStatusCode.AlreadyExist
                    });
                }

                college.Status = collegeToUpsert.Status;
                college.Name = collegeToUpsert.Name.Trim();
                college.Abbreviation = collegeToUpsert.Abbreviation.Trim();
                college.CreatedBy = collegeToUpsert.CreatedBy;
                _context.Add(college);
                _context.SaveChanges();
                return new JsonResult(new ApiResponse<string>
                {
                    Message = string.Format(ResponseMessages.AddSuccess, ModuleNames.College),
                    Result = true,
                    StatusCode = ResponseStatusCode.Success
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in CollegeRepository.Create \n MESSAGE : {ex.Message} \n INNER EXCEPTION : {ex.InnerException} \n");
                return new JsonResult(new ApiResponse<string>
                {
                    Message = ResponseMessages.InternalError,
                    Result = false,
                    StatusCode = ResponseStatusCode.InternalServerError
                });
            }

        }

        public async Task<JsonResult> Update(CollegeVM collegeToUpsert)
        {

            try
            {
                MasterCollege? masterCollege = _context.MasterCollege.Where(c => c.Name.Trim().ToLower() == collegeToUpsert.Name.Trim().ToLower() && c.Id != collegeToUpsert.Id && c.IsDeleted != true).FirstOrDefault();
                if (masterCollege != null)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.AlreadyExists, ModuleNames.College),
                        Result = false,
                        StatusCode = ResponseStatusCode.AlreadyExist
                    });
                }

                MasterCollege? masterCollegeWithSameAbbreviation = _context.MasterCollege.Where(c => c.Abbreviation.Trim().ToLower() == collegeToUpsert.Abbreviation.Trim().ToLower() && c.Id != collegeToUpsert.Id && c.IsDeleted != true).FirstOrDefault();

                if (masterCollegeWithSameAbbreviation != null)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.AlreadyExists, ModuleNames.Abbreviation),
                        Result = false,
                        StatusCode = ResponseStatusCode.AlreadyExist
                    });
                }

                MasterCollege? masterCollegeToBeUpdate = await Task.FromResult(_context.MasterCollege.AsNoTracking().Where(college => college.Id == collegeToUpsert.Id && college.IsDeleted != true).FirstOrDefault());
                if (masterCollegeToBeUpdate != null)
                {
                    masterCollegeToBeUpdate.Status = collegeToUpsert.Status;
                    masterCollegeToBeUpdate.Name = collegeToUpsert.Name.Trim();
                    masterCollegeToBeUpdate.Abbreviation = collegeToUpsert.Abbreviation.Trim();
                    masterCollegeToBeUpdate.UpdatedBy = collegeToUpsert.UpdatedBy;
                    masterCollegeToBeUpdate.UpdatedDate = DateTime.UtcNow;
                    _context.Update(masterCollegeToBeUpdate);
                    _context.SaveChanges();

                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.UpdateSuccess, ModuleNames.College),
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }


                return new JsonResult(new ApiResponse<string>
                {
                    Message = string.Format(ResponseMessages.NotFound, ModuleNames.College),
                    Result = false,
                    StatusCode = ResponseStatusCode.NotFound
                });
            }

            catch (Exception ex)
            {
                _logger.LogError($"Error occurred for ID : {collegeToUpsert.Id} in CollegeRepository.Update \n MESSAGE : {ex.Message} \n INNER EXCEPTION : {ex.InnerException} \n");
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
                MasterCollege? college = await Task.FromResult(_context.MasterCollege.Where(college => college.IsDeleted != true && college.Id == status.Id).FirstOrDefault());
                if (college == null)
                {
                    return new JsonResult(new ApiResponse<int>
                    {
                        Message = string.Format(ResponseMessages.NotFound, ModuleNames.College),
                        Result = true,
                        StatusCode = ResponseStatusCode.NotFound
                    });
                }
                college.Status = status.Status;
                _context.Update(college);
                _context.SaveChanges();

                return new JsonResult(new ApiResponse<int>
                {
                    Message = string.Format(ResponseMessages.UpdateSuccess, ModuleNames.College),
                    Result = true,
                    StatusCode = ResponseStatusCode.Success
                });
            }

            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in CollegeRepository.UpdateStatus \n MESSAGE : {ex.Message} \n INNER EXCEPTION : {ex.InnerException}\n");
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

                MasterCollege? college = await Task.FromResult(_context.MasterCollege.Where(c => c.Id == id && c.IsDeleted == false).FirstOrDefault());
                if (college != null)
                {
                    college.IsDeleted = true;
                    _context.Update(college);
                    _context.SaveChanges();
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.DeleteSuccess, ModuleNames.College),

                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }
                return new JsonResult(new ApiResponse<string>
                {
                    Message = string.Format(ResponseMessages.NotFound, ModuleNames.College),
                    Result = false,
                    StatusCode = ResponseStatusCode.NotFound
                });
            }

            catch (Exception ex)
            {
                _logger.LogError($"Error occurred for Id: {id} in CollegeRepository.Delete \n MESSAGE : {ex.Message} \n INNER EXCEPTION : {ex.InnerException} \n");
                return new JsonResult(new ApiResponse<string>
                {
                    Message = ResponseMessages.InternalError,
                    Result = false,
                    StatusCode = ResponseStatusCode.InternalServerError
                });
            }

        }

        public async Task<JsonResult> CheckUncheckAll(bool check)
        {
            try
            {
                int rowsEffected = _context.MasterStream.Where(college => college.IsDeleted == false).ExecuteUpdate(setters => setters.SetProperty(college => college.Status, check));
                return new JsonResult(new ApiResponse<int>
                {
                    Data = rowsEffected,
                    Message = string.Format(ResponseMessages.UpdateSuccess, ModuleNames.College),
                    Result = true,
                    StatusCode = ResponseStatusCode.Success
                });
            }

            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in CollegeRepository.CheckUncheckAll \n MESSAGE : {ex.Message} \n INNER EXCEPTION : {ex.InnerException} \n");
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
