using AptitudeTest.Core.Entities.Master;
using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using AptitudeTest.Data.Common;
using APTITUDETEST.Common.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AptitudeTest.Data.Data
{
    public class ProfileRepository : IProfileRepository
    {
        #region Properties
        AppDbContext _context;
        #endregion

        #region Constructor
        public ProfileRepository(AppDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Methods
        public async Task<JsonResult> GetProfiles(string? searchQuery, int? filter, int? currentPageIndex, int? pageSize)
        {
            try
            {
                List<MasterTechnology> profilelist = await Task.FromResult(_context.MasterTechnology.Where(x => x.IsDeleted == null || x.IsDeleted == false).ToList());

                if (searchQuery != null)
                {
                    string query = searchQuery.ToLower();
                    profilelist = profilelist.Where(profile => profile.Name.ToLower().Contains(query)).ToList();
                }

                if (filter != null)
                {
                    if (filter == 1)
                    {
                        profilelist = profilelist.Where(profile => profile.Status == true).ToList();
                    }
                    if (filter == 2)
                    {
                        profilelist = profilelist.Where(profile => profile.Status == false).ToList();
                    }
                }

                List<ProfileVM> profileData = profilelist.Select(profile => new ProfileVM()
                {
                    Id = profile.Id,
                    CreatedBy = null,
                    Name = profile.Name,
                    Status = profile.Status,
                    UpdatedBy = null
                }).ToList();

                PaginationVM<ProfileVM> paginatedData = Pagination<ProfileVM>.Paginate(profileData, pageSize, currentPageIndex);
                return new JsonResult(new ApiResponse<PaginationVM<ProfileVM>>
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

        public async Task<JsonResult> Create(ProfileVM profile)
        {
            try
            {
                MasterTechnology profiles = _context.MasterTechnology.Where(t => t.Name.ToLower() == profile.Name.ToLower() && t.Id != profile.Id && t.IsDeleted != true).FirstOrDefault();
                if (profiles != null)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.AlreadyExists, "Profile"),
                        Result = false,
                        StatusCode = ResponseStatusCode.AlreadyExist
                    });
                }

                MasterTechnology masterProfile = new MasterTechnology();
                masterProfile.Status = profile.Status;
                masterProfile.Name = profile.Name;
                masterProfile.CreatedBy = profile.CreatedBy;
                _context.Add(masterProfile);
                _context.SaveChanges();

                return new JsonResult(new ApiResponse<string>
                {
                    Message = string.Format(ResponseMessages.AddSuccess, "Profile"),
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

        public async Task<JsonResult> Update(ProfileVM profile)
        {
            try
            {
                MasterTechnology profiles = _context.MasterTechnology.Where(t => t.Name.ToLower() == profile.Name.ToLower() && t.Id != profile.Id && t.IsDeleted != true).FirstOrDefault();
                if (profiles != null)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.AlreadyExists, "Profile"),
                        Result = false,
                        StatusCode = ResponseStatusCode.AlreadyExist
                    });
                }

                MasterTechnology MasterProfile = await Task.FromResult(_context.MasterTechnology.AsNoTracking().Where(t => t.Id == profile.Id && t.IsDeleted != true).FirstOrDefault());
                if (MasterProfile != null)
                {
                    MasterProfile.Status = profile.Status;
                    MasterProfile.Name = profile.Name;
                    MasterProfile.UpdatedBy = profile.UpdatedBy;
                    MasterProfile.UpdatedDate = DateTime.UtcNow;
                    _context.Update(MasterProfile);
                    _context.SaveChanges();

                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.UpdateSuccess, "Profile"),
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }

                return new JsonResult(new ApiResponse<string>
                {
                    Message = string.Format(ResponseMessages.NotFound, "Profile"),
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
                int rowsEffected = _context.MasterStream.Where(profile => profile.IsDeleted == false).ExecuteUpdate(setters => setters.SetProperty(profile => profile.Status, check));
                return new JsonResult(new ApiResponse<int>
                {
                    Data = rowsEffected,
                    Message = string.Format(ResponseMessages.UpdateSuccess, "Profile"),
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

                MasterTechnology profile = await Task.FromResult(_context.MasterTechnology.Where(t => t.Id == id && t.IsDeleted == false).FirstOrDefault());
                if (profile != null)
                {
                    profile.IsDeleted = true;
                    _context.Update(profile);
                    _context.SaveChanges();
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.DeleteSuccess, "Profile"),
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }
                return new JsonResult(new ApiResponse<string>
                {
                    Message = string.Format(ResponseMessages.NotFound, "Profile"),
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
