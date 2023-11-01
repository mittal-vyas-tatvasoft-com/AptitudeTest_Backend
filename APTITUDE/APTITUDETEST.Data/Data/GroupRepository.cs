using AptitudeTest.Core.Entities.Master;
using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using AptitudeTest.Data.Common;
using APTITUDETEST.Common.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace AptitudeTest.Data.Data
{
    public class GroupRepository : IGroupRepository
    {
        private readonly AppDbContext _context;


        public GroupRepository(AppDbContext context)
        {
            _context = context;
        }

        #region Methods

        public async Task<JsonResult> Create(GroupsQueryVM groupToBeAdded)
        {
            try
            {
                MasterGroup group = new MasterGroup();
                MasterGroup existingGroup = _context.MasterGroup.Where(g => g.Name.Trim().ToLower().Equals(groupToBeAdded.Name.ToLower()) && g.IsDeleted != true).FirstOrDefault();
                if (existingGroup != null)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.AlreadyExists, ModuleNames.Group),
                        Result = false,
                        StatusCode = ResponseStatusCode.AlreadyExist
                    });
                }
                group.Name = groupToBeAdded.Name;
                group.IsDefault = groupToBeAdded.IsDefault;
                _context.MasterGroup.Add(group);
                _context.SaveChanges();

                return new JsonResult(new ApiResponse<string>
                {
                    Message = string.Format(ResponseMessages.AddSuccess, ModuleNames.Group),
                    Result = true,
                    StatusCode = ResponseStatusCode.Success
                });
            }
            catch (Exception)
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
                MasterGroup groupToBeDeleted = _context.MasterGroup.Where(group => group.Id == id && group.IsDeleted != true).FirstOrDefault();
                if (groupToBeDeleted != null)
                {
                    groupToBeDeleted.IsDeleted = true;
                    _context.Update(groupToBeDeleted);
                    _context.SaveChanges();

                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.DeleteSuccess, ModuleNames.Group),

                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }
                return new JsonResult(new ApiResponse<string>
                {
                    Message = string.Format(ResponseMessages.NotFound, ModuleNames.Group),
                    Result = false,
                    StatusCode = ResponseStatusCode.NotFound
                });
            }
            catch (Exception)
            {
                return new JsonResult(new ApiResponse<string>
                {
                    Message = ResponseMessages.InternalError,
                    Result = false,
                    StatusCode = ResponseStatusCode.InternalServerError
                });
            }
        }

        public async Task<JsonResult> GetActiveGroups()
        {
            try
            {
                var collegeList = await Task.FromResult(_context.MasterGroup
                                            .Where(x => (x.IsDeleted == null || x.IsDeleted == false) && x.Status == true)
                                            .Select(x => new { Id = x.Id, Name = x.Name })
                                            .ToList());

                if (collegeList != null)
                {
                    return new JsonResult(new ApiResponse<IEnumerable<object>>
                    {
                        Data = collegeList,
                        Message = ResponseMessages.Success,
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }
                else
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Data = string.Format(ResponseMessages.NotFound, ModuleNames.Group),
                        Message = ResponseMessages.Success,
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }
            }
            catch (Exception)
            {
                return new JsonResult(new ApiResponse<string>
                {
                    Message = ResponseMessages.InternalError,
                    Result = false,
                    StatusCode = ResponseStatusCode.InternalServerError
                });
            }
        }

        public async Task<JsonResult> GetGroups(string? searchGroup)
        {
            try
            {
                List<MasterGroup> existingGroups = await Task.FromResult(_context.MasterGroup.Where(group => group.IsDeleted == null || group.IsDeleted == false).OrderByDescending(group => group.CreatedDate).ToList());
                if (!searchGroup.IsNullOrEmpty())
                {
                    existingGroups = existingGroups.Where(group => group.Name.ToLower().Contains(searchGroup) && (group.IsDeleted == null || group.IsDeleted == false)).OrderByDescending(group => group.CreatedDate).ToList();
                }
                List<GroupsResponseVM> groups = new List<GroupsResponseVM>();

                foreach (var group in existingGroups)
                {
                    GroupsResponseVM groupItem = new GroupsResponseVM()
                    {
                        Id = group.Id,
                        Name = group.Name,
                        IsDefault = (bool)group.IsDefault,
                        CollegesUnderGroup = new List<GroupedCollegeVM>()
                    };
                    var colleges = _context.MasterCollege.Where(college => college.GroupId == group.Id).ToList();
                    foreach (var college in colleges)
                    {
                        int students = _context.Users.Where(user => user.CollegeId == college.Id).Count();
                        groupItem.CollegesUnderGroup.Add(new GroupedCollegeVM()
                        {
                            Name = college.Name,
                            NumberOfStudentsInCollege = students,
                        });
                    }
                    groupItem.NumberOfStudentsInGroup = groupItem.CollegesUnderGroup.Sum(x => x.NumberOfStudentsInCollege);
                    groups.Add(groupItem);
                }
                return new JsonResult(new ApiResponse<List<GroupsResponseVM>>
                {
                    Data = groups,
                    Message = ResponseMessages.Success,
                    Result = true,
                    StatusCode = ResponseStatusCode.Success
                });
            }
            catch (Exception)
            {
                return new JsonResult(new ApiResponse<string>
                {
                    Message = ResponseMessages.InternalError,
                    Result = false,
                    StatusCode = ResponseStatusCode.InternalServerError
                });
            }
        }

        public async Task<JsonResult> Rename(GroupsQueryVM updatedGroup)
        {
            try
            {
                MasterGroup existingGroup = _context.MasterGroup.Where(group => group.Name.Equals(updatedGroup.Name) && group.Id != updatedGroup.Id && group.IsDeleted != true).FirstOrDefault();
                if (existingGroup != null)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.AlreadyExists, ModuleNames.Group),
                        Result = false,
                        StatusCode = ResponseStatusCode.AlreadyExist
                    });
                }
                MasterGroup groupToBeRenamed = await Task.FromResult(_context.MasterGroup.AsNoTracking().Where(group => group.Id == updatedGroup.Id && group.IsDeleted != true).FirstOrDefault());
                if (groupToBeRenamed != null)
                {
                    groupToBeRenamed.Name = updatedGroup.Name;
                    _context.Update(groupToBeRenamed);
                    _context.SaveChanges();

                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.UpdateSuccess, ModuleNames.Group),
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }

                return new JsonResult(new ApiResponse<string>
                {
                    Message = string.Format(ResponseMessages.NotFound, ModuleNames.Group),
                    Result = false,
                    StatusCode = ResponseStatusCode.NotFound
                });
            }
            catch (Exception)
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
