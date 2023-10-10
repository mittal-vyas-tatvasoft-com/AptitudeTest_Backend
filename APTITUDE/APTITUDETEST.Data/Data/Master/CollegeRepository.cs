using AptitudeTest.Core.Entities.Master;
using AptitudeTest.Core.Interfaces.Master;
using AptitudeTest.Core.ViewModels.Common;
using AptitudeTest.Core.ViewModels.Master;
using AptitudeTest.Data.Common;
using APTITUDETEST.Common.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AptitudeTest.Data.Data.Master
{
    public class CollegeRepository : MasterRepositoryBase<MasterCollege>, ICollegeRepository
    {

        private readonly AppDbContext _context;


        public CollegeRepository(AppDbContext context) : base(context)
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

        public async Task<JsonResult> Upsert(CollegeVM collegeToUpsert)
        {

            try
            {
                MasterCollege college = new MasterCollege();
                List<MasterCollege> colleges = _context.MasterCollege.Where(c => c.Name == collegeToUpsert.Name || c.Abbreviation == collegeToUpsert.Abbreviation).ToList();
                if (colleges.Count > 0)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = ResponseMessages.CollegeAlreadyExists,
                        Result = false,
                        StatusCode = ResponseStatusCode.AlreadyExist
                    });
                }

                if (collegeToUpsert.Id == 0)
                {
                    college.Status = collegeToUpsert.Status;
                    college.Name = collegeToUpsert.Name;
                    college.Abbreviation = collegeToUpsert.Abbreviation;
                    college.CreatedBy = collegeToUpsert.CreatedBy;
                    Create(college);
                    _context.SaveChanges();
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = ResponseMessages.CollegeAddSuccess,
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }
                else
                {
                    MasterCollege masterCollege = await Task.FromResult(_context.MasterCollege.AsNoTracking().Where(college => college.Id == collegeToUpsert.Id).FirstOrDefault());
                    if (masterCollege != null)
                    {
                        masterCollege.Status = collegeToUpsert.Status;
                        masterCollege.Name = collegeToUpsert.Name;
                        masterCollege.Abbreviation = collegeToUpsert.Abbreviation;
                        masterCollege.UpdatedBy = collegeToUpsert.UpdatedBy;
                        masterCollege.UpdatedDate = DateTime.UtcNow;
                        Update(masterCollege);
                        _context.SaveChanges();

                        return new JsonResult(new ApiResponse<string>
                        {
                            Message = ResponseMessages.CollegeUpdateSuccess,
                            Result = true,
                            StatusCode = ResponseStatusCode.Success
                        });
                    }


                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = ResponseMessages.CollegeNotFound,
                        Result = false,
                        StatusCode = ResponseStatusCode.NotFound
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

                MasterCollege college = await GetById(id);
                if (college != null)
                {
                    college.IsDeleted = true;
                    Update(college);
                    _context.SaveChanges();
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = ResponseMessages.CollegeDeleteSuccess,
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }
                return new JsonResult(new ApiResponse<string>
                {
                    Message = ResponseMessages.CollegeNotFound,
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
                int rowsEffected = CheckUncheck(setters => setters.SetProperty(college => college.Status, check));
                return new JsonResult(new ApiResponse<int>
                {
                    Data = rowsEffected,
                    Message = ResponseMessages.CollegeUpdateSuccess,
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
