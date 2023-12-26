using AptitudeTest.Core.Entities.Master;
using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using AptitudeTest.Data.Common;
using APTITUDETEST.Common.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AptitudeTest.Data.Data
{
    public class StreamRepository : IStreamRepository
    {
        #region Properties
        readonly AppDbContext _context;
        private readonly ILoggerManager _logger;
        #endregion

        #region Constructor
        public StreamRepository(AppDbContext context, ILoggerManager logger)
        {
            _context = context;
            _logger = logger;
        }
        #endregion

        #region Methods

        #region GetAllActiveStreams
        public async Task<JsonResult> GetAllActiveStreams()
        {
            try
            {
                _logger.LogInfo($"StreamRepository.GetAllActiveStreams");
                var distinctStreams = await Task.FromResult(_context.MasterStream
                .Join(_context.MasterDegree, ms => ms.DegreeId, md => md.Id, (ms, md) => new { ms, md })
                .Where(x => (x.ms.IsDeleted == null || x.ms.IsDeleted == false) && x.ms.Status == true
                        && (x.md.IsDeleted == null || x.md.IsDeleted == false) && x.md.Status == true)
                .Select(x => new
                {
                    level = x.md.Level,
                    id = x.ms.Id,
                    name = x.ms.Name
                })
                .ToList());

                var distinctFilteredStreams = distinctStreams
                    .GroupBy(x => new { x.level, x.name })
                    .Select(x => x.First())
                    .ToList();

                if (distinctFilteredStreams.Count > 0)
                {
                    return new JsonResult(new ApiResponse<IEnumerable<object>>
                    {
                        Data = distinctFilteredStreams,
                        Message = ResponseMessages.Success,
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }
                else
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Data = string.Format(ResponseMessages.NotFound, ModuleNames.Stream),
                        Message = ResponseMessages.Success,
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in StreamRepository.GetAllActive Streams:{ex}");
                return new JsonResult(new ApiResponse<string>
                {
                    Message = ResponseMessages.InternalError,
                    Result = false,
                    StatusCode = ResponseStatusCode.InternalServerError
                });
            }
        }
        #endregion

        public async Task<JsonResult> Getstreams(string? searchQuery, int? filter, List<int>? degreelist, int? currentPageIndex, int? pageSize)
        {
            try
            {
                _logger.LogInfo($"StreamRepository.GetStreams");
                List<MasterStream> streamlist = await Task.FromResult(_context.MasterStream.Where(s => s.IsDeleted == null || s.IsDeleted == false).Include(s => s.MasterDegrees).ToList());

                if (searchQuery != null)
                {
                    string query = searchQuery.ToLower();
                    streamlist = streamlist.Where(stream => stream.Name.ToLower().Contains(query)).ToList();
                }

                if (filter != null)
                {
                    if (filter == 1)
                    {
                        streamlist = streamlist.Where(stream => stream.Status == true).ToList();
                    }
                    if (filter == 2)
                    {
                        streamlist = streamlist.Where(stream => stream.Status == false).ToList();
                    }
                }

                if (degreelist != null && degreelist.Count != 0)
                {
                    streamlist = streamlist.Where(stream => degreelist.Contains(stream.DegreeId)).ToList();
                }

                List<StreamVM> streamData = streamlist.Select(stream => new StreamVM()
                {
                    DegreeId = stream.DegreeId,
                    CreatedBy = null,
                    Id = stream.Id,
                    Name = stream.Name,
                    DegreeName = stream.MasterDegrees.Name,
                    Status = stream.Status,
                    UpdatedBy = null
                }).ToList();

                PaginationVM<StreamVM> paginatedData = Pagination<StreamVM>.Paginate(streamData, pageSize, currentPageIndex);
                return new JsonResult(new ApiResponse<PaginationVM<StreamVM>>
                {
                    Data = paginatedData,
                    Message = ResponseMessages.Success,
                    Result = true,
                    StatusCode = ResponseStatusCode.Success
                });
            }

            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in StreamRepository.GetStreams:{ex}");
                return new JsonResult(new ApiResponse<string>
                {
                    Message = ResponseMessages.InternalError,
                    Result = false,
                    StatusCode = ResponseStatusCode.InternalServerError
                });
            }
        }

        public async Task<JsonResult> Create(StreamVM stream)
        {
            try
            {
                _logger.LogInfo($"StreamRepository.Create");
                MasterStream? streams = _context.MasterStream.Where(s => s.Name.ToLower() == stream.Name.ToLower() && s.DegreeId == stream.DegreeId && s.IsDeleted != true).FirstOrDefault();
                if (streams != null)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.AlreadyExists, ModuleNames.Stream),
                        Result = false,
                        StatusCode = ResponseStatusCode.AlreadyExist
                    });
                }
                MasterDegree? degree = _context.MasterDegree.Where(c => c.Id == stream.DegreeId && c.IsDeleted == false).FirstOrDefault();
                if (degree == null)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.NotFound, ModuleNames.Degree),
                        Result = false,
                        StatusCode = ResponseStatusCode.BadRequest
                    });
                }
                MasterStream masterStream = new MasterStream();
                masterStream.Status = stream.Status;
                masterStream.Name = stream.Name;
                masterStream.DegreeId = stream.DegreeId;
                masterStream.CreatedBy = stream.CreatedBy;
                _context.Add(masterStream);
                _context.SaveChanges();

                return new JsonResult(new ApiResponse<string>
                {
                    Message = string.Format(ResponseMessages.AddSuccess, ModuleNames.Stream),
                    Result = true,
                    StatusCode = ResponseStatusCode.Success
                });


            }

            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in StreamRepository.Create:{ex}");
                return new JsonResult(new ApiResponse<string>
                {
                    Message = ResponseMessages.InternalError,
                    Result = false,
                    StatusCode = ResponseStatusCode.InternalServerError
                });
            }
        }

        public async Task<JsonResult> Update(StreamVM stream)
        {
            try
            {
                _logger.LogInfo($"StreamRepository.Update");
                MasterStream? streams = _context.MasterStream.Where(s => s.Name.ToLower() == stream.Name.ToLower() && s.DegreeId == stream.DegreeId && s.Id != stream.Id && s.IsDeleted != true).FirstOrDefault();
                if (streams != null)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.AlreadyExists, ModuleNames.Stream),
                        Result = false,
                        StatusCode = ResponseStatusCode.AlreadyExist
                    });
                }
                MasterDegree? degree = _context.MasterDegree.Where(c => c.Id == stream.DegreeId && c.IsDeleted == false).FirstOrDefault();
                if (degree == null)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.NotFound, ModuleNames.Stream),
                        Result = false,
                        StatusCode = ResponseStatusCode.BadRequest
                    });
                }

                MasterStream? masterStream = await Task.FromResult(_context.MasterStream.AsNoTracking().Where(l => l.Id == stream.Id && l.IsDeleted != true).FirstOrDefault());
                if (masterStream != null)
                {
                    masterStream.Status = stream.Status;
                    masterStream.Name = stream.Name;
                    masterStream.DegreeId = stream.DegreeId;
                    masterStream.UpdatedBy = stream.UpdatedBy;
                    masterStream.UpdatedDate = DateTime.UtcNow;
                    _context.Update(masterStream);
                    _context.SaveChanges();

                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.UpdateSuccess, ModuleNames.Stream),
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }

                return new JsonResult(new ApiResponse<string>
                {
                    Message = string.Format(ResponseMessages.NotFound, ModuleNames.Stream),
                    Result = false,
                    StatusCode = ResponseStatusCode.NotFound
                });
            }

            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in StreamRepository.Update:{ex}");
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
                _logger.LogInfo($"StreamRepository.CheckUncheckAll");
                int rowsEffected = _context.MasterStream.Where(stream => stream.IsDeleted == false).ExecuteUpdate(setters => setters.SetProperty(stream => stream.Status, check));
                return new JsonResult(new ApiResponse<int>
                {
                    Data = rowsEffected,
                    Message = string.Format(ResponseMessages.UpdateSuccess, ModuleNames.Stream),
                    Result = true,
                    StatusCode = ResponseStatusCode.Success
                });
            }

            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in StreamRepository.CheckUncheckAll:{ex}");
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
                _logger.LogInfo($"StreamRepository.Delete for Id: {id}");
                MasterStream? stream = await Task.FromResult(_context.MasterStream.Where(s => s.Id == id && s.IsDeleted == false).FirstOrDefault());
                if (stream != null)
                {
                    stream.IsDeleted = true;
                    _context.Update(stream);
                    _context.SaveChanges();
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.DeleteSuccess, ModuleNames.Stream),
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }
                return new JsonResult(new ApiResponse<string>
                {
                    Message = string.Format(ResponseMessages.NotFound, ModuleNames.Stream),
                    Result = false,
                    StatusCode = ResponseStatusCode.NotFound
                });
            }

            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in StreamRepository.Delete:{ex} for Id:{id}");
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
