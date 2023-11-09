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
        AppDbContext _context;
        #endregion

        #region Constructor
        public DegreeRepository(AppDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Methods
        public async Task<JsonResult> GetDegrees(string? sortField, string? sortOrder)
        {
            try
            {
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
                                degreeData = degreeData.OrderBy(x => x.Name).ToList();
                                break;
                            case "desc":
                                degreeData = degreeData.OrderByDescending(x => x.Name).ToList();
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
                if (id == 0)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = ResponseMessages.BadRequest,
                        Result = false,
                        StatusCode = ResponseStatusCode.BadRequest
                    });
                }
                MasterDegree masterDegree = await Task.FromResult(_context.MasterDegree.Where(x => x.IsDeleted != true && x.Id == id).FirstOrDefault());
                if (masterDegree == null)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.NotFound, "Degree"),
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
                MasterDegree degrees = _context.MasterDegree.Where(d => d.Name.Trim().ToLower() == degree.Name.Trim().ToLower() && d.IsDeleted != true).FirstOrDefault();
                if (degrees != null)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.AlreadyExists, "Degree"),
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
                    Message = string.Format(ResponseMessages.AddSuccess, "Degree"),
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
                MasterDegree degrees = _context.MasterDegree.Where(d => d.Name.ToLower().Trim() == degree.Name.ToLower().Trim() && d.Id != degree.Id && d.IsDeleted != true).FirstOrDefault();
                if (degrees != null)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.AlreadyExists, "Degree"),
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
                            Message = string.Format(ResponseMessages.NotEditable, "Degree"),
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
                        Message = string.Format(ResponseMessages.UpdateSuccess, "Degree"),
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }

                return new JsonResult(new ApiResponse<string>
                {
                    Message = string.Format(ResponseMessages.NotFound, "Degree"),
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

        public async Task<JsonResult> UpdateStatus(StatusVM status)
        {
            try
            {
                MasterDegree degree = await Task.FromResult(_context.MasterDegree.Where(degree => degree.IsDeleted != true && degree.Id == status.Id).FirstOrDefault());
                if (degree == null)
                {
                    return new JsonResult(new ApiResponse<int>
                    {
                        Message = string.Format(ResponseMessages.NotFound, "Degree"),
                        Result = true,
                        StatusCode = ResponseStatusCode.NotFound
                    });
                }

                degree.Status = status.Status;
                _context.Update(degree);
                _context.SaveChanges();

                return new JsonResult(new ApiResponse<int>
                {
                    Message = string.Format(ResponseMessages.UpdateSuccess, "Degree"),
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

        public async Task<JsonResult> CheckUncheckAll(bool check)
        {
            try
            {
                int rowsEffected = _context.MasterDegree.Where(degree => degree.IsDeleted == false).ExecuteUpdate(setters => setters.SetProperty(degree => degree.Status, check));
                return new JsonResult(new ApiResponse<int>
                {
                    Data = rowsEffected,
                    Message = string.Format(ResponseMessages.UpdateSuccess, "Degree"),
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
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.DeleteSuccess, "Degree"),
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }
                return new JsonResult(new ApiResponse<string>
                {
                    Message = string.Format(ResponseMessages.NotFound, "Degree"),
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
