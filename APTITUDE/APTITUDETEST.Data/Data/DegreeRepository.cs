using AptitudeTest.Core.Entities.Master;
using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using AptitudeTest.Data.Common;
using APTITUDETEST.Common.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AptitudeTest.Data.Data
{
    public class DegreeRepository : IDegreeRepository
    {
        #region Properties
        readonly AppDbContext _context;
        private readonly ILoggerManager _logger;

        #endregion

        #region Constructor
        public DegreeRepository(AppDbContext context, ILoggerManager logger)
        {
            _context = context;
            _logger = logger;
        }
        #endregion

        #region Methods
        public async Task<JsonResult> GetDegrees(string? sortField, string? sortOrder)
        {
            try
            {
                _logger.LogInfo($"DegreeRepository.GetDegrees");
                List<MasterDegree> degreelist = await Task.FromResult(_context.MasterDegree.Where(x => x.IsDeleted == null || x.IsDeleted == false).OrderByDescending(degree => degree.CreatedDate).ToList());
                List<MasterStream> masterStreams = await Task.FromResult(_context.MasterStream.ToList());
                List<DegreeVM> degreeData = degreelist.Select(degree => new DegreeVM()
                {
                    Id = degree.Id,
                    CreatedBy = null,
                    IsEditable = degree.IsEditable,
                    Name = degree.Name,
                    Status = degree.Status,
                    Level = degree.Level,
                    UpdatedBy = null,
                    Streams = masterStreams.Where(stream => stream.DegreeId == degree.Id).Select(stream => " " + stream.Name).ToList()
                }).ToList();


                switch (sortField)
                {
                    case "Name":
                        switch (sortOrder)
                        {
                            case "asc":
                                degreeData = degreeData.OrderBy(x => x.Name).ToList();
                                break;
                            case "desc":
                                degreeData = degreeData.OrderByDescending(x => x.Name).ToList();
                                break;
                        }
                        break;

                    case "Level":
                        switch (sortOrder)
                        {
                            case "asc":
                                degreeData = degreeData.OrderBy(x => x.Level).ToList();
                                break;
                            case "desc":
                                degreeData = degreeData.OrderByDescending(x => x.Level).ToList();
                                break;
                        }
                        break;
                }

                return new JsonResult(new ApiResponse<List<DegreeVM>>
                {
                    Data = degreeData,
                    Message = ResponseMessages.Success,
                    Result = true,
                    StatusCode = ResponseStatusCode.Success
                });
            }

            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in DegreeRepository.GetDegrees:{ex}");

                return new JsonResult(new ApiResponse<string>
                {
                    Message = ResponseMessages.InternalError,
                    Result = false,
                    StatusCode = ResponseStatusCode.InternalServerError
                });
            }
        }

        public async Task<JsonResult> GetActiveDegrees()
        {

            try
            {
                _logger.LogInfo($"DegreeRepository.GetActiveDegrees");
                var DegreeList = await Task.FromResult(_context.MasterDegree
                .Where(x => (x.IsDeleted == null || x.IsDeleted == false) && x.Status == true)
                .Select(x => new { Id = x.Id, Name = x.Name, Level = x.Level })
                .ToList());

                if (DegreeList != null)
                {
                    return new JsonResult(new ApiResponse<IEnumerable<object>>
                    {
                        Data = DegreeList,
                        Message = ResponseMessages.Success,
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }
                else
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Data = string.Format(ResponseMessages.NotFound, ModuleNames.Degree),
                        Message = ResponseMessages.Success,
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in DegreeRepository.GetActiveDegrees");
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
                _logger.LogInfo($"DegreeRepository.Get for Id: {id}");
                if (id == 0)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = ResponseMessages.BadRequest,
                        Result = false,
                        StatusCode = ResponseStatusCode.BadRequest
                    });
                }
                MasterDegree? masterDegree = await Task.FromResult(_context.MasterDegree.Where(x => x.IsDeleted != true && x.Id == id).FirstOrDefault());
                if (masterDegree == null)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.NotFound, ModuleNames.Degree),
                        Result = false,
                        StatusCode = ResponseStatusCode.NotFound
                    });
                }
                List<MasterStream> masterStreams = await Task.FromResult(_context.MasterStream.Where(stream => stream.DegreeId == id).ToList());

                DegreeVM degreeData = new DegreeVM()
                {
                    Id = masterDegree.Id,
                    CreatedBy = null,
                    IsEditable = masterDegree.IsEditable,
                    Name = masterDegree.Name,
                    Status = masterDegree.Status,
                    Level = masterDegree.Level,
                    UpdatedBy = null,
                    Streams = masterStreams.Select(stream => stream.Name).ToList()
                };

                return new JsonResult(new ApiResponse<DegreeVM>
                {
                    Data = degreeData,
                    Message = ResponseMessages.Success,
                    Result = true,
                    StatusCode = ResponseStatusCode.Success
                });
            }

            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in DegreeRepository.Get: {ex} for Id: {id}");
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
                _logger.LogInfo($"DegreeRepository.Create");
                MasterDegree? degrees = _context.MasterDegree.Where(d => d.Name.Trim().ToLower() == degree.Name.Trim().ToLower() && d.IsDeleted != true).FirstOrDefault();
                if (degrees != null)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.AlreadyExists, ModuleNames.Degree),
                        Result = false,
                        StatusCode = ResponseStatusCode.AlreadyExist
                    });
                }

                MasterDegree masterDegree = new MasterDegree();
                masterDegree.Status = degree.Status;
                masterDegree.Name = degree.Name.Trim();
                masterDegree.CreatedBy = degree.CreatedBy;
                masterDegree.Level = degree.Level;
                _context.Add(masterDegree);
                _context.SaveChanges();
                int id = _context.MasterDegree.OrderBy(degree => degree.CreatedDate).LastOrDefault().Id;
                IEnumerable<MasterStream> streams = degree.Streams.Select(stream => new MasterStream { Name = stream, DegreeId = id });
                _context.MasterStream.AddRange(streams);
                _context.SaveChanges();

                return new JsonResult(new ApiResponse<string>
                {
                    Message = string.Format(ResponseMessages.AddSuccess, ModuleNames.Degree),
                    Result = true,
                    StatusCode = ResponseStatusCode.Success
                });

            }

            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in DegreeRepository.Create:{ex}");
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
                _logger.LogInfo($"DegreeRepository.Update for DegreeId:{degree.Id}");
                MasterDegree? degrees = _context.MasterDegree.Where(d => d.Name.ToLower().Trim() == degree.Name.ToLower().Trim() && d.Id != degree.Id && d.IsDeleted != true).FirstOrDefault();
                if (degrees != null)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.AlreadyExists, ModuleNames.Degree),
                        Result = false,
                        StatusCode = ResponseStatusCode.AlreadyExist
                    });
                }

                MasterDegree? masterDegree = await Task.FromResult(_context.MasterDegree.AsNoTracking().Where(d => d.Id == degree.Id && d.IsDeleted != true).FirstOrDefault());
                if (masterDegree != null)
                {
                    if (!(bool)masterDegree.IsEditable)
                    {
                        return new JsonResult(new ApiResponse<string>
                        {
                            Message = string.Format(ResponseMessages.NotEditable, ModuleNames.Degree),
                            Result = false,
                            StatusCode = ResponseStatusCode.BadRequest
                        });
                    }

                    masterDegree.Status = degree.Status;
                    masterDegree.Name = degree.Name.Trim();
                    masterDegree.CreatedBy = degree.CreatedBy;
                    masterDegree.Level = degree.Level;
                    masterDegree.UpdatedBy = degree.UpdatedBy;
                    masterDegree.UpdatedDate = DateTime.UtcNow;
                    _context.Update(masterDegree);
                    _context.MasterStream.RemoveRange(_context.MasterStream.Where(stream => stream.DegreeId == degree.Id));
                    IEnumerable<MasterStream> streams = degree.Streams.Select(stream => new MasterStream { Name = stream, DegreeId = degree.Id });
                    _context.MasterStream.AddRange(streams);
                    _context.SaveChanges();

                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.UpdateSuccess, ModuleNames.Degree),
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }

                return new JsonResult(new ApiResponse<string>
                {
                    Message = string.Format(ResponseMessages.NotFound, ModuleNames.Degree),
                    Result = false,
                    StatusCode = ResponseStatusCode.NotFound
                });
            }

            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in DegreeRepository.Update:{ex} for DegreeId:{degree.Id}");
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
                _logger.LogInfo($"DegreeRepository.UpdateStatus");
                MasterDegree? degree = await Task.FromResult(_context.MasterDegree.Where(degree => degree.IsDeleted != true && degree.Id == status.Id).FirstOrDefault());
                if (degree == null)
                {
                    return new JsonResult(new ApiResponse<int>
                    {
                        Message = string.Format(ResponseMessages.NotFound, ModuleNames.Degree),
                        Result = true,
                        StatusCode = ResponseStatusCode.NotFound
                    });
                }

                degree.Status = status.Status;
                _context.Update(degree);
                _context.SaveChanges();

                return new JsonResult(new ApiResponse<int>
                {
                    Message = string.Format(ResponseMessages.UpdateSuccess, ModuleNames.Degree),
                    Result = true,
                    StatusCode = ResponseStatusCode.Success
                });
            }

            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in DegreeRepository.UpdateStatus : {ex}");
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
                _logger.LogInfo($"DegreeRepository.CheckUncheckAll");
                int rowsEffected = _context.MasterDegree.Where(degree => degree.IsDeleted == false).ExecuteUpdate(setters => setters.SetProperty(degree => degree.Status, check));
                return new JsonResult(new ApiResponse<int>
                {
                    Data = rowsEffected,
                    Message = string.Format(ResponseMessages.UpdateSuccess, ModuleNames.Degree),
                    Result = true,
                    StatusCode = ResponseStatusCode.Success
                });
            }

            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in DegreeRepository.CheckUncheckAll : {ex}");
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
                _logger.LogInfo($"DegreeRepository.Delete for Id:{id}");
                if (id == 0)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = ResponseMessages.BadRequest,
                        Result = false,
                        StatusCode = ResponseStatusCode.BadRequest
                    });
                }

                MasterDegree? degree = await Task.FromResult(_context.MasterDegree.Where(d => d.Id == id && d.IsDeleted == false).FirstOrDefault());
                if (degree != null)
                {
                    degree.IsDeleted = true;
                    _context.MasterDegree.Update(degree);
                    _context.SaveChanges();
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.DeleteSuccess, ModuleNames.Degree),
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }
                return new JsonResult(new ApiResponse<string>
                {
                    Message = string.Format(ResponseMessages.NotFound, ModuleNames.Degree),
                    Result = false,
                    StatusCode = ResponseStatusCode.NotFound
                });
            }

            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in DegreeRepository.Delete+: {ex} for Id:{id}");
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
