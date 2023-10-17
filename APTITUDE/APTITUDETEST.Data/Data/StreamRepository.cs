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
        AppDbContext _context;
        #endregion

        #region Constructor
        public StreamRepository(AppDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Methods
        public async Task<JsonResult> Getstreams(string? searchQuery, int? filter, List<int>? degreelist, int? currentPageIndex, int? pageSize)
        {
            try
            {
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
                MasterStream streams = _context.MasterStream.Where(s => s.Name.ToLower() == stream.Name.ToLower() && s.DegreeId == stream.DegreeId && s.IsDeleted != true).FirstOrDefault();
                if (streams != null)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.AlreadyExists, "Stream"),
                        Result = false,
                        StatusCode = ResponseStatusCode.AlreadyExist
                    });
                }
                MasterDegree degree = _context.MasterDegree.Where(c => c.Id == stream.DegreeId && c.IsDeleted == false).FirstOrDefault();
                if (degree == null)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.NotFound, "Degree"),
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
                    Message = string.Format(ResponseMessages.AddSuccess, "Stream"),
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

        public async Task<JsonResult> Update(StreamVM stream)
        {
            try
            {
                MasterStream streams = _context.MasterStream.Where(s => s.Name.ToLower() == stream.Name.ToLower() && s.DegreeId == stream.DegreeId && s.Id != stream.Id && s.IsDeleted != true).FirstOrDefault();
                if (streams != null)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.AlreadyExists, "Stream"),
                        Result = false,
                        StatusCode = ResponseStatusCode.AlreadyExist
                    });
                }
                MasterDegree degree = _context.MasterDegree.Where(c => c.Id == stream.DegreeId && c.IsDeleted == false).FirstOrDefault();
                if (degree == null)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.NotFound, "Degree"),
                        Result = false,
                        StatusCode = ResponseStatusCode.BadRequest
                    });
                }

                MasterStream masterStream = await Task.FromResult(_context.MasterStream.AsNoTracking().Where(l => l.Id == stream.Id && l.IsDeleted != true).FirstOrDefault());
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
                        Message = string.Format(ResponseMessages.UpdateSuccess, "Stream"),
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }

                return new JsonResult(new ApiResponse<string>
                {
                    Message = string.Format(ResponseMessages.NotFound, "Stream"),
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
                int rowsEffected = _context.MasterStream.Where(stream => stream.IsDeleted == false).ExecuteUpdate(setters => setters.SetProperty(stream => stream.Status, check));
                return new JsonResult(new ApiResponse<int>
                {
                    Data = rowsEffected,
                    Message = string.Format(ResponseMessages.UpdateSuccess, "Stream"),
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

                MasterStream stream = await Task.FromResult(_context.MasterStream.Where(s => s.Id == id && s.IsDeleted == false).FirstOrDefault());
                if (stream != null)
                {
                    stream.IsDeleted = true;
                    _context.Update(stream);
                    _context.SaveChanges();
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.DeleteSuccess, "Stream"),
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }
                return new JsonResult(new ApiResponse<string>
                {
                    Message = string.Format(ResponseMessages.NotFound, "Stream"),
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
