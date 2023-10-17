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


        public CollegeRepository(AppDbContext context)
        {
            _context = context;
        }

        #region Methods
        public async Task<JsonResult> GetColleges(CollegeQueryVM collegeQuery)
        {

            try
            {
                List<MasterCollege> collegeList = await Task.FromResult(_context.MasterCollege.Where(x => x.IsDeleted == null || x.IsDeleted == false).ToList());

                if (collegeQuery.SearchQuery != null)
                {
                    string query = collegeQuery.SearchQuery.ToLower();
                    collegeList = collegeList.Where(college => college.Name.ToLower().Contains(query) || college.Abbreviation.ToLower().Contains(query)).ToList();
                }

                if (collegeQuery.Filter != null)
                {
                    if (collegeQuery.Filter == 1)
                    {
                        collegeList = collegeList.Where(college => college.Status == true).ToList();
                    }
                    if (collegeQuery.Filter == 2)
                    {
                        collegeList = collegeList.Where(college => college.Status == false).ToList();
                    }
                }

                PaginationVM<MasterCollege> paginatedData = Pagination<MasterCollege>.Paginate(collegeList, collegeQuery.PageSize, collegeQuery.CurrentPageIndex);

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
                MasterCollege masterCollege = _context.MasterCollege.Where(c => (c.Name.ToLower() == collegeToUpsert.Name.ToLower() || c.Abbreviation.ToLower() == collegeToUpsert.Abbreviation.ToLower()) && c.IsDeleted != true).FirstOrDefault();
                if (masterCollege != null)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.AlreadyExists, "College"),
                        Result = false,
                        StatusCode = ResponseStatusCode.AlreadyExist
                    });
                }

                college.Status = collegeToUpsert.Status;
                college.Name = collegeToUpsert.Name;
                college.Abbreviation = collegeToUpsert.Abbreviation;
                college.CreatedBy = collegeToUpsert.CreatedBy;
                _context.Add(college);
                _context.SaveChanges();
                return new JsonResult(new ApiResponse<string>
                {
                    Message = string.Format(ResponseMessages.AddSuccess, "College"),
                    Result = true,
                    StatusCode = ResponseStatusCode.Success
                }); ;
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

        public async Task<JsonResult> Update(CollegeVM collegeToUpsert)
        {

            try
            {
                MasterCollege college = new MasterCollege();
                MasterCollege cplleges = _context.MasterCollege.Where(c => (c.Name.ToLower() == collegeToUpsert.Name.ToLower() || c.Abbreviation.ToLower() == collegeToUpsert.Abbreviation.ToLower()) && c.Id != collegeToUpsert.Id && c.IsDeleted != true).FirstOrDefault();
                if (cplleges != null)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.AlreadyExists, "College"),
                        Result = false,
                        StatusCode = ResponseStatusCode.AlreadyExist
                    });
                }

                MasterCollege masterCollege = await Task.FromResult(_context.MasterCollege.AsNoTracking().Where(college => college.Id == collegeToUpsert.Id && college.IsDeleted != true).FirstOrDefault());
                if (masterCollege != null)
                {
                    masterCollege.Status = collegeToUpsert.Status;
                    masterCollege.Name = collegeToUpsert.Name;
                    masterCollege.Abbreviation = collegeToUpsert.Abbreviation;
                    masterCollege.UpdatedBy = collegeToUpsert.UpdatedBy;
                    masterCollege.UpdatedDate = DateTime.UtcNow;
                    _context.Update(masterCollege);
                    _context.SaveChanges();

                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.UpdateSuccess, "College"),
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }


                return new JsonResult(new ApiResponse<string>
                {
                    Message = string.Format(ResponseMessages.NotFound, "College"),
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

                MasterCollege college = await Task.FromResult(_context.MasterCollege.Where(c => c.Id == id && c.IsDeleted == false).FirstOrDefault());
                if (college != null)
                {
                    college.IsDeleted = true;
                    _context.Update(college);
                    _context.SaveChanges();
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.DeleteSuccess, "College"),

                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }
                return new JsonResult(new ApiResponse<string>
                {
                    Message = string.Format(ResponseMessages.NotFound, "College"),
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

        public async Task<JsonResult> CheckUncheckAll(bool check)
        {
            try
            {
                int rowsEffected = _context.MasterStream.Where(college => college.IsDeleted == false).ExecuteUpdate(setters => setters.SetProperty(college => college.Status, check));
                return new JsonResult(new ApiResponse<int>
                {
                    Data = rowsEffected,
                    Message = string.Format(ResponseMessages.UpdateSuccess, "College"),
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

        #endregion
    }
}
