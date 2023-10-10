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

        public async Task<JsonResult> Upsert(TechnologyVM technology)
        {
            try
            {
                List<MasterTechnology> technologies = _context.MasterTechnology.Where(t => t.Name == technology.Name).ToList();
                if (technologies.Count > 0)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = ResponseMessages.TechnologyAlreadyExists,
                        Result = false,
                        StatusCode = ResponseStatusCode.AlreadyExist
                    });
                }

                if (technology.Id == 0)
                {
                    MasterTechnology MasterTechnology = new MasterTechnology();
                    MasterTechnology.Status = technology.Status;
                    MasterTechnology.Name = technology.Name;
                    MasterTechnology.CreatedBy = technology.CreatedBy;
                    Create(MasterTechnology);
                    _context.SaveChanges();

                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = ResponseMessages.TechnologyAddSuccess,
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }
                else
                {
                    MasterTechnology MasterTechnology = await Task.FromResult(_context.MasterTechnology.AsNoTracking().Where(t => t.Id == technology.Id).FirstOrDefault());
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
                            Message = ResponseMessages.TechnologyUpdateSuccess,
                            Result = true,
                            StatusCode = ResponseStatusCode.Success
                        });
                    }

                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = ResponseMessages.TechnologyNotFound,
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

        public async Task<JsonResult> CheckUncheckAll(bool check)
        {
            try
            {
                int rowsEffected = CheckUncheck(setters => setters.SetProperty(technology => technology.Status, check));
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

                MasterTechnology technology = await GetById(id);
                if (technology != null)
                {
                    technology.IsDeleted = true;
                    Update(technology);
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
        #endregion
    }
}
