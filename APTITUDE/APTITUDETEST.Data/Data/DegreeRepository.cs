using AptitudeTest.Core.Entities.Master;
using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using AptitudeTest.Data.Common;
using APTITUDETEST.Common.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AptitudeTest.Data.Data
{
    public class DegreeRepository : MasterRepositoryBase<MasterDegree>, IDegreeRepository
    {
        #region Properties
        AppDbContext _context;
        #endregion

        #region Constructor
        public DegreeRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        #endregion

        #region Methods
        public async Task<JsonResult> GetDegrees(string? searchQuery, int? filter, int? currentPageIndex, int? pageSize)
        {
            try
            {
                List<MasterDegree> degreelist = await Task.FromResult(_context.MasterDegree.Where(x => x.IsDeleted == null || x.IsDeleted == false).ToList());

                if (searchQuery != null)
                {
                    string query = searchQuery.ToLower();
                    degreelist = degreelist.Where(degree => degree.Name.ToLower().Contains(query)).ToList();
                }

                if (filter != null)
                {
                    if (filter == 1)
                    {
                        degreelist = degreelist.Where(degree => degree.Status == true).ToList();
                    }
                    if (filter == 2)
                    {
                        degreelist = degreelist.Where(degree => degree.Status == false).ToList();
                    }
                }

                List<DegreeVM> degreeData = degreelist.Select(degree => new DegreeVM()
                {
                    Id = degree.Id,
                    CreatedBy = null,
                    IsEditable = degree.IsEditable,
                    Name = degree.Name,
                    Status = degree.Status,
                    Level = degree.Level,
                    UpdatedBy = null
                }).ToList();

                PaginationVM<DegreeVM> paginatedData = Pagination<DegreeVM>.Paginate(degreeData, pageSize, currentPageIndex);
                return new JsonResult(new ApiResponse<PaginationVM<DegreeVM>>
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

        public async Task<JsonResult> Create(DegreeVM degree)
        {
            try
            {
                List<MasterDegree> degrees = _context.MasterDegree.Where(d => d.Name.ToLower() == degree.Name.ToLower() && d.IsDeleted != true).ToList();
                if (degrees.Count > 0)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = ResponseMessages.DegreeAlreadyExists,
                        Result = false,
                        StatusCode = ResponseStatusCode.AlreadyExist
                    });
                }

                MasterDegree masterDegree = new MasterDegree();
                masterDegree.Status = degree.Status;
                masterDegree.Name = degree.Name;
                masterDegree.CreatedBy = degree.CreatedBy;
                masterDegree.Level = degree.Level;
                Create(masterDegree);
                _context.SaveChanges();

                return new JsonResult(new ApiResponse<string>
                {
                    Message = ResponseMessages.DegreeAddSuccess,
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

        public async Task<JsonResult> Update(DegreeVM degree)
        {
            try
            {
                List<MasterDegree> degrees = _context.MasterDegree.Where(d => d.Name.ToLower() == degree.Name.ToLower() && d.Id != degree.Id && d.IsDeleted != true).ToList();
                if (degrees.Count > 0)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = ResponseMessages.DegreeAlreadyExists,
                        Result = false,
                        StatusCode = ResponseStatusCode.AlreadyExist
                    });
                }

                MasterDegree masterDegree = await Task.FromResult(_context.MasterDegree.AsNoTracking().Where(d => d.Id == degree.Id && d.IsDeleted != true).FirstOrDefault());
                if (masterDegree != null)
                {
                    if (!(bool)masterDegree.IsEditable)
                    {
                        return new JsonResult(new ApiResponse<string>
                        {
                            Message = ResponseMessages.DegreeNotEditable,
                            Result = false,
                            StatusCode = ResponseStatusCode.BadRequest
                        });
                    }

                    masterDegree.Status = degree.Status;
                    masterDegree.Name = degree.Name;
                    masterDegree.CreatedBy = degree.CreatedBy;
                    masterDegree.Level = degree.Level;
                    masterDegree.UpdatedBy = degree.UpdatedBy;
                    masterDegree.UpdatedDate = DateTime.UtcNow;
                    Update(masterDegree);
                    _context.SaveChanges();

                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = ResponseMessages.DegreeUpdateSuccess,
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }

                return new JsonResult(new ApiResponse<string>
                {
                    Message = ResponseMessages.DegreeNotFound,
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
                int rowsEffected = CheckUncheck(degree => degree.IsDeleted == false, setters => setters.SetProperty(degree => degree.Status, check));
                return new JsonResult(new ApiResponse<int>
                {
                    Data = rowsEffected,
                    Message = ResponseMessages.DegreeUpdateSuccess,
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

                MasterDegree degree = await Task.FromResult(_context.MasterDegree.Where(d => d.Id == id && d.IsDeleted == false).FirstOrDefault());
                if (degree != null)
                {
                    degree.IsDeleted = true;
                    _context.MasterDegree.Update(degree);
                    _context.SaveChanges();
                    /*Update(degree);*/
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = ResponseMessages.DegreeDeleteSuccess,
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }
                return new JsonResult(new ApiResponse<string>
                {
                    Message = ResponseMessages.DegreeNotFound,
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
        #endregion
    }
}
