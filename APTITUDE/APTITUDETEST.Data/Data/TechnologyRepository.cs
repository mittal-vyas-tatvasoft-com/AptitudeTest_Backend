using AptitudeTest.Core.Entities.Master;
using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using AptitudeTest.Data.Common;
using APTITUDETEST.Common.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AptitudeTest.Data.Data
{
    public class TechnologyRepository : MasterRepositoryBase<MasterTechnology>, ITechnologyRepository
    {
        #region Properties
        AppDbContext _context;
        #endregion

        #region Constructor
        public TechnologyRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        #endregion

        #region Methods
        public async Task<JsonResult> GetTechnologies(string? searchQuery, int? filter, int? currentPageIndex, int? pageSize)
        {
            try
            {
                List<MasterTechnology> technologylist = await Task.FromResult(_context.MasterTechnology.Where(x => x.IsDeleted == null || x.IsDeleted == false).ToList());

                if (searchQuery != null)
                {
                    string query = searchQuery.ToLower();
                    technologylist = technologylist.Where(technology => technology.Name.ToLower().Contains(query)).ToList();
                }

                if (filter != null)
                {
                    if (filter == 1)
                    {
                        technologylist = technologylist.Where(technology => technology.Status == true).ToList();
                    }
                    if (filter == 2)
                    {
                        technologylist = technologylist.Where(technology => technology.Status == false).ToList();
                    }
                }

                List<TechnologyVM> technologyData = technologylist.Select(technology => new TechnologyVM()
                {
                    Id = technology.Id,
                    CreatedBy = null,
                    Name = technology.Name,
                    Status = technology.Status,
                    UpdatedBy = null
                }).ToList();

                PaginationVM<TechnologyVM> paginatedData = Pagination<TechnologyVM>.Paginate(technologyData, pageSize, currentPageIndex);
                return new JsonResult(new ApiResponse<PaginationVM<TechnologyVM>>
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

        public async Task<JsonResult> Create(TechnologyVM technology)
        {
            try
            {
                MasterTechnology technologies = _context.MasterTechnology.Where(t => t.Name.ToLower() == technology.Name.ToLower() && t.Id != technology.Id && t.IsDeleted != true).FirstOrDefault();
                if (technologies !=null)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.AlreadyExists, "Technology"),
                        Result = false,
                        StatusCode = ResponseStatusCode.AlreadyExist
                    });
                }

                MasterTechnology MasterTechnology = new MasterTechnology();
                MasterTechnology.Status = technology.Status;
                MasterTechnology.Name = technology.Name;
                MasterTechnology.CreatedBy = technology.CreatedBy;
                Create(MasterTechnology);
                _context.SaveChanges();

                return new JsonResult(new ApiResponse<string>
                {
                    Message = string.Format(ResponseMessages.AddSuccess, "Technology"),
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

        public async Task<JsonResult> Update(TechnologyVM technology)
        {
            try
            {
                MasterTechnology technologies = _context.MasterTechnology.Where(t => t.Name.ToLower() == technology.Name.ToLower() && t.Id != technology.Id && t.IsDeleted != true).FirstOrDefault();
                if (technologies != null)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.AlreadyExists, "Technology"),
                        Result = false,
                        StatusCode = ResponseStatusCode.AlreadyExist
                    });
                }

                MasterTechnology MasterTechnology = await Task.FromResult(_context.MasterTechnology.AsNoTracking().Where(t => t.Id == technology.Id && t.IsDeleted != true).FirstOrDefault());
                if (MasterTechnology != null)
                {
                    MasterTechnology.Status = technology.Status;
                    MasterTechnology.Name = technology.Name;
                    MasterTechnology.UpdatedBy = technology.UpdatedBy;
                    MasterTechnology.UpdatedDate = DateTime.UtcNow;
                    Update(MasterTechnology);
                    _context.SaveChanges();

                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.UpdateSuccess, "Technology"),
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }

                return new JsonResult(new ApiResponse<string>
                {
                    Message = string.Format(ResponseMessages.NotFound, "Technology"),
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
                int rowsEffected = CheckUncheck(technology => technology.IsDeleted == false, setters => setters.SetProperty(technology => technology.Status, check));
                return new JsonResult(new ApiResponse<int>
                {
                    Data = rowsEffected,
                    Message = string.Format(ResponseMessages.UpdateSuccess, "Technology"),
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

                MasterTechnology technology = await Task.FromResult(_context.MasterTechnology.Where(t => t.Id == id && t.IsDeleted == false).FirstOrDefault());
                if (technology != null)
                {
                    technology.IsDeleted = true;
                    Update(technology);
                    _context.SaveChanges();
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.DeleteSuccess, "Technology"),
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }
                return new JsonResult(new ApiResponse<string>
                {
                    Message = string.Format(ResponseMessages.NotFound, "Technology"),
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
