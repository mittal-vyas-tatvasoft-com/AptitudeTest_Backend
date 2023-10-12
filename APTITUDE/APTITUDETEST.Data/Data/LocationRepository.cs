using AptitudeTest.Core.Entities.Master;
using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using AptitudeTest.Data.Common;
using APTITUDETEST.Common.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AptitudeTest.Data.Data
{
    public class LocationRepository : MasterRepositoryBase<MasterLocation>, ILocationRepository
    {
        #region Properties
        AppDbContext _context;
        #endregion

        #region Constructor
        public LocationRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        #endregion

        #region Methods
        public async Task<JsonResult> GetLocations(string? searchQuery, int? filter, List<int>? collegelist, int? currentPageIndex, int? pageSize)
        {
            try
            {
                List<MasterLocation> locationlist = await Task.FromResult(_context.MasterLocation.Where(x => x.IsDeleted == null || x.IsDeleted == false).ToList());

                if (searchQuery != null)
                {
                    string query = searchQuery.ToLower();
                    locationlist = locationlist.Where(location => location.Location.ToLower().Contains(query)).ToList();
                }

                if (filter != null)
                {
                    if (filter == 1)
                    {
                        locationlist = locationlist.Where(location => location.Status == true).ToList();
                    }
                    if (filter == 2)
                    {
                        locationlist = locationlist.Where(location => location.Status == false).ToList();
                    }
                }

                if (collegelist != null && collegelist.Count != 0)
                {
                    locationlist = locationlist.Where(location => collegelist.Contains(location.CollegeId)).ToList();
                }

                List<LocationVM> locationData = locationlist.Select(location => new LocationVM()
                {
                    CollegeId = location.CollegeId,
                    CreatedBy = null,
                    Id = location.Id,
                    Location = location.Location,
                    Status = location.Status,
                    UpdatedBy = null
                }).ToList();


                PaginationVM<LocationVM> paginatedData = Pagination<LocationVM>.Paginate(locationData, pageSize, currentPageIndex);
                return new JsonResult(new ApiResponse<PaginationVM<LocationVM>>
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

        public async Task<JsonResult> Upsert(LocationVM location)
        {
            try
            {
                List<MasterLocation> locations = _context.MasterLocation.Where(l => l.Location.ToLower() == location.Location.ToLower() && l.CollegeId == location.CollegeId && l.Id != location.Id && l.IsDeleted != true).ToList();
                if (locations.Count > 0)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = ResponseMessages.LocationAlreadyExists,
                        Result = false,
                        StatusCode = ResponseStatusCode.AlreadyExist
                    });
                }
                MasterCollege college = _context.MasterCollege.Where(c => c.Id == location.CollegeId && c.IsDeleted != true).FirstOrDefault();
                if (college == null)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = ResponseMessages.CollegeNotFound,
                        Result = false,
                        StatusCode = ResponseStatusCode.BadRequest
                    });
                }

                if (location.Id == 0)
                {
                    MasterLocation masterLocation = new MasterLocation();
                    masterLocation.Status = location.Status;
                    masterLocation.Location = location.Location;
                    masterLocation.CollegeId = location.CollegeId;
                    masterLocation.CreatedBy = location.CreatedBy;
                    Create(masterLocation);
                    _context.SaveChanges();

                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = ResponseMessages.LocationAddSuccess,
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }
                else
                {
                    MasterLocation masterLocation = await Task.FromResult(_context.MasterLocation.AsNoTracking().Where(l => l.Id == location.Id).FirstOrDefault());
                    if (masterLocation != null)
                    {
                        masterLocation.Status = location.Status;
                        masterLocation.CollegeId = location.CollegeId;
                        masterLocation.Location = location.Location;
                        masterLocation.UpdatedBy = location.UpdatedBy;
                        masterLocation.UpdatedDate = DateTime.UtcNow;
                        Update(masterLocation);
                        _context.SaveChanges();

                        return new JsonResult(new ApiResponse<string>
                        {
                            Message = ResponseMessages.LocationUpdateSuccess,
                            Result = true,
                            StatusCode = ResponseStatusCode.Success
                        });
                    }

                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = ResponseMessages.LocationNotFound,
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
                int rowsEffected = CheckUncheck(location => location.IsDeleted == false, setters => setters.SetProperty(location => location.Status, check));
                return new JsonResult(new ApiResponse<int>
                {
                    Data = rowsEffected,
                    Message = ResponseMessages.LocationUpdateSuccess,
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

                MasterLocation location = await Task.FromResult(_context.MasterLocation.Where(l => l.Id == id && l.IsDeleted == false).FirstOrDefault());
                if (location != null)
                {
                    location.IsDeleted = true;
                    Update(location);
                    _context.SaveChanges();
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = ResponseMessages.LocationDeleteSuccess,
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }
                return new JsonResult(new ApiResponse<string>
                {
                    Message = ResponseMessages.LocationNotFound,
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
