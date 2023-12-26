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
        readonly AppDbContext _context;
        private readonly ILoggerManager _logger;
        #endregion

        #region Constructor
        public ProfileRepository(AppDbContext context, ILoggerManager logger)
        {
            _context = context;
            _logger = logger;
        }
        #endregion

        #region Methods
        public async Task<JsonResult> GetProfiles(string? sortField, string? sortOrder)
        {
            try
            {
                _logger.LogInfo($"ProfileRepository.GetProfiles");
                List<MasterTechnology> profilelist = await Task.FromResult(_context.MasterTechnology.Where(x => x.IsDeleted == null || x.IsDeleted == false).OrderByDescending(x => x.CreatedDate).ToList());

                List<ProfileVM> profileData = profilelist.Select(profile => new ProfileVM()
                {
                    Id = profile.Id,
                    CreatedBy = null,
                    Name = profile.Name,
                    Status = profile.Status,
                    UpdatedBy = null
                }).ToList();

                if (sortField == "Name" && sortOrder == "asc")
                {
                    profileData = profileData.OrderBy(x => x.Name).ToList();
                }
                else if (sortField == "Name" && sortOrder == "desc")
                {
                    profileData = profileData.OrderByDescending(x => x.Name).ToList();
                }
                return new JsonResult(new ApiResponse<List<ProfileVM>>
                {
                    Data = profileData,
                    Message = ResponseMessages.Success,
                    Result = true,
                    StatusCode = ResponseStatusCode.Success
                });
            }

            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in ProfileRepository.GetProfiles:{ex}");
                return new JsonResult(new ApiResponse<string>
                {
                    Message = ResponseMessages.InternalError,
                    Result = false,
                    StatusCode = ResponseStatusCode.InternalServerError
                });
            }
        }

        public async Task<JsonResult> GetActiveProfiles()
        {

            try
            {
                _logger.LogInfo($"ProfileRepository.GetActiveProfiles");
                var ProfileList = await Task.FromResult(_context.MasterTechnology
                .Where(x => (x.IsDeleted == null || x.IsDeleted == false) && x.Status == true)
                .Select(x => new { Id = x.Id, Name = x.Name })
                .ToList());

                if (ProfileList != null)
                {
                    return new JsonResult(new ApiResponse<IEnumerable<object>>
                    {
                        Data = ProfileList,
                        Message = ResponseMessages.Success,
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }
                else
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Data = string.Format(ResponseMessages.NotFound, ModuleNames.Profile),
                        Message = ResponseMessages.Success,
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in ProfileRepository.GetActiveProfiles:{ex}");
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
                _logger.LogInfo($"ProfileRepository.Create");
                MasterTechnology? profiles = _context.MasterTechnology.Where(t => t.Name.Trim().ToLower() == profile.Name.Trim().ToLower() && t.Id != profile.Id && t.IsDeleted != true).FirstOrDefault();
                if (profiles != null)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.AlreadyExists, ModuleNames.Profile),
                        Result = false,
                        StatusCode = ResponseStatusCode.AlreadyExist
                    });
                }

                MasterTechnology masterProfile = new MasterTechnology();
                masterProfile.Status = profile.Status;
                masterProfile.Name = profile.Name.Trim();
                masterProfile.CreatedBy = profile.CreatedBy;
                _context.Add(masterProfile);
                _context.SaveChanges();

                return new JsonResult(new ApiResponse<string>
                {
                    Message = string.Format(ResponseMessages.AddSuccess, ModuleNames.Profile),
                    Result = true,
                    StatusCode = ResponseStatusCode.Success
                });
            }

            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in ProfileRepository.Create:{ex}");
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
                _logger.LogInfo($"ProfileRepository.Update for Id:{profile.Id}");
                MasterTechnology? profiles = _context.MasterTechnology.Where(t => t.Name.Trim().ToLower() == profile.Name.Trim().ToLower() && t.Id != profile.Id && t.IsDeleted != true).FirstOrDefault();
                if (profiles != null)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.AlreadyExists, ModuleNames.Profile),
                        Result = false,
                        StatusCode = ResponseStatusCode.AlreadyExist
                    });
                }

                MasterTechnology? MasterProfile = await Task.FromResult(_context.MasterTechnology.AsNoTracking().Where(t => t.Id == profile.Id && t.IsDeleted != true).FirstOrDefault());
                if (MasterProfile != null)
                {
                    MasterProfile.Status = profile.Status;
                    MasterProfile.Name = profile.Name.Trim();
                    MasterProfile.UpdatedBy = profile.UpdatedBy;
                    MasterProfile.UpdatedDate = DateTime.UtcNow;
                    _context.Update(MasterProfile);
                    _context.SaveChanges();

                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.UpdateSuccess, ModuleNames.Profile),
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }

                return new JsonResult(new ApiResponse<string>
                {
                    Message = string.Format(ResponseMessages.NotFound, ModuleNames.Profile),
                    Result = false,
                    StatusCode = ResponseStatusCode.NotFound
                });

            }

            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in ProfileRepository.Update:{ex} for Id:{profile.Id}");
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
                _logger.LogInfo($"ProfileRepository.CheckUncheckAll");
                int rowsEffected = _context.MasterStream.Where(profile => profile.IsDeleted == false).ExecuteUpdate(setters => setters.SetProperty(profile => profile.Status, check));
                return new JsonResult(new ApiResponse<int>
                {
                    Data = rowsEffected,
                    Message = string.Format(ResponseMessages.UpdateSuccess, ModuleNames.Profile),
                    Result = true,
                    StatusCode = ResponseStatusCode.Success
                });
            }

            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in ProfileRepository.CheckUncheckAll: {ex}");
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
                _logger.LogInfo($"ProfileRepository.Delete for id: {id}");
                if (id == 0)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = ResponseMessages.BadRequest,
                        Result = false,
                        StatusCode = ResponseStatusCode.BadRequest
                    });
                }

                MasterTechnology? profile = await Task.FromResult(_context.MasterTechnology.Where(t => t.Id == id && t.IsDeleted == false).FirstOrDefault());
                if (profile != null)
                {
                    profile.IsDeleted = true;
                    _context.Update(profile);
                    _context.SaveChanges();
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.DeleteSuccess, ModuleNames.Profile),
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }
                return new JsonResult(new ApiResponse<string>
                {
                    Message = string.Format(ResponseMessages.NotFound, ModuleNames.Profile),
                    Result = false,
                    StatusCode = ResponseStatusCode.NotFound
                });
            }

            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in ProfileRepository.Delete: {ex} for id: {id}");
                return new JsonResult(new ApiResponse<string>
                {
                    Message = ResponseMessages.InternalError,
                    Result = false,
                    StatusCode = ResponseStatusCode.InternalServerError
                });
            }
        }

        public async Task<JsonResult> GetProfileById(int? id)
        {
            try
            {
                _logger.LogInfo($"ProfileRepository.GetProfileById");
                MasterTechnology? Technology = _context.MasterTechnology.Where(mt => mt.Id == id).FirstOrDefault();

                if (Technology != null)
                {
                    return new JsonResult(new ApiResponse<MasterTechnology> { Data = Technology, Message = ResponseMessages.Success, StatusCode = ResponseStatusCode.Success, Result = true });
                }
                else
                {
                    return new JsonResult(new ApiResponse<MasterTechnology> { Message = string.Format(ResponseMessages.NotFound, "Technology"), StatusCode = ResponseStatusCode.NotFound, Result = false });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in ProfileRepository.GetProfileById: {ex}");
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
                _logger.LogInfo($"ProfileRepository.UpdateStatus");
                MasterTechnology? profile = await Task.FromResult(_context.MasterTechnology.Where(mt => mt.IsDeleted == false && mt.Id == status.Id).FirstOrDefault());
                if (profile == null)
                {
                    return new JsonResult(new ApiResponse<int>
                    {
                        Message = string.Format(ResponseMessages.NotFound, ModuleNames.Profile),
                        Result = true,
                        StatusCode = ResponseStatusCode.NotFound
                    });
                }

                profile.Status = status.Status;
                _context.Update(profile);
                _context.SaveChanges();

                return new JsonResult(new ApiResponse<int>
                {
                    Message = string.Format(ResponseMessages.UpdateSuccess, ModuleNames.Profile),
                    Result = true,
                    StatusCode = ResponseStatusCode.Success
                });
            }

            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in ProfileRepository.UpdateStatus:{ex}");
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
