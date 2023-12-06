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
                MasterGroup? existingGroup = _context.MasterGroup.Where(g => g.Name.Trim().ToLower().Equals(groupToBeAdded.Name.ToLower()) && g.IsDeleted != true).FirstOrDefault();
                if (existingGroup != null)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.AlreadyExists, ModuleNames.Group),
                        Result = false,
                        StatusCode = ResponseStatusCode.AlreadyExist
                    });
                }
                if (groupToBeAdded.IsDefault)
                {
                    foreach (var masterGroup in _context.MasterGroup)
                    {
                        masterGroup.IsDefault = false;
                    }
                }
                _context.SaveChanges();
                group.Name = groupToBeAdded.Name.Trim();
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
            catch
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
                MasterGroup? groupToBeDeleted = _context.MasterGroup.Where(group => group.Id == id && group.IsDeleted != true).FirstOrDefault();
                if (groupToBeDeleted != null && groupToBeDeleted.IsDefault == false)
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
                if (groupToBeDeleted != null && groupToBeDeleted.IsDefault == true)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.NotEditable, ModuleNames.Group),
                        Result = true,
                        StatusCode = ResponseStatusCode.NotAcceptable
                    });
                }
                return new JsonResult(new ApiResponse<string>
                {
                    Message = string.Format(ResponseMessages.NotFound, ModuleNames.Group),
                    Result = false,
                    StatusCode = ResponseStatusCode.NotFound
                });
            }
            catch
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
            catch
            {
                return new JsonResult(new ApiResponse<string>
                {
                    Message = ResponseMessages.InternalError,
                    Result = false,
                    StatusCode = ResponseStatusCode.InternalServerError
                });
            }
        }

        public async Task<JsonResult> GetGroups(string? searchGroup, int? collegeId)
        {
            try
            {
                List<MasterGroup> existingGroups = await Task.FromResult(_context.MasterGroup.Where(group => (group.IsDeleted == null || group.IsDeleted == false) && group.Status == true).OrderByDescending(group => group.Id).ToList());

                if (!searchGroup.IsNullOrEmpty())
                {
                    existingGroups = existingGroups.Where(group => group.Name.ToLower().Contains(searchGroup.ToLower())).ToList();
                }
                if (collegeId != null)
                {
                    var users = _context.Users.Where(user => user.CollegeId == collegeId).ToList();
                    if (users.Count == 0)
                    {
                        return new JsonResult(new ApiResponse<string>
                        {
                            Message = string.Format(ResponseMessages.NotFound, ModuleNames.Group),
                            Result = false,
                            StatusCode = ResponseStatusCode.NotFound
                        });
                    }
                    List<MasterGroup> filteredGroups = new List<MasterGroup>();
                    foreach (var user in users)
                    {
                        var userGroup = existingGroups.FirstOrDefault(group => group.Id == user.GroupId);
                        if (userGroup != null)
                        {
                            var groupExists = filteredGroups.Exists(group => group.Id == userGroup.Id);
                            if (!groupExists)
                            {
                                filteredGroups.Add(userGroup);
                            }
                        }

                    }
                    existingGroups = filteredGroups.OrderBy(group => group.Name).ToList();
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
                    var users = _context.Users.Where(user => user.GroupId == group.Id && user.IsDeleted != true).ToList();
                    foreach (var user in users)
                    {
                        var college = _context.MasterCollege.FirstOrDefault(college => college.Id == user.CollegeId && college.IsDeleted != true);
                        if (college != null)
                        {
                            bool collegeExists = groupItem.CollegesUnderGroup.Exists(existedCollege => existedCollege.Name.Equals(college.Name));
                            if (!collegeExists)
                            {
                                int students = _context.Users.Where(user => user.CollegeId == college.Id && user.IsDeleted != true).Count();
                                groupItem.CollegesUnderGroup.Add(new GroupedCollegeVM()
                                {
                                    Name = college.Name,
                                    NumberOfStudentsInCollege = students,
                                });
                            }
                        }
                    }

                    groupItem.NumberOfStudentsInGroup = groupItem.CollegesUnderGroup.Sum(x => x.NumberOfStudentsInCollege);
                    var sortedColleges = groupItem.CollegesUnderGroup.OrderBy(college => college.Name).ToList();
                    groupItem.CollegesUnderGroup = sortedColleges;
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
            catch
            {
                return new JsonResult(new ApiResponse<string>
                {
                    Message = ResponseMessages.InternalError,
                    Result = false,
                    StatusCode = ResponseStatusCode.InternalServerError
                });
            }
        }

        public async Task<JsonResult> Update(GroupsQueryVM updatedGroup)
        {
            try
            {
                MasterGroup? existingGroup = _context.MasterGroup.Where(group => group.Name.Equals(updatedGroup.Name) && group.Id != updatedGroup.Id && group.IsDeleted != true).FirstOrDefault();
                if (existingGroup != null)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.AlreadyExists, ModuleNames.Group),
                        Result = false,
                        StatusCode = ResponseStatusCode.AlreadyExist
                    });
                }
                MasterGroup? groupToBeUpdated = await Task.FromResult(_context.MasterGroup.AsNoTracking().Where(group => group.Id == updatedGroup.Id && group.IsDeleted != true).FirstOrDefault());
                if (groupToBeUpdated != null)
                {
                    groupToBeUpdated.Name = updatedGroup.Name.Trim();
                    groupToBeUpdated.IsDefault = updatedGroup.IsDefault;
                    _context.Update(groupToBeUpdated);

                    if ((bool)groupToBeUpdated.IsDefault)
                    {
                        foreach (var group in _context.MasterGroup)
                        {
                            if (group.Id != groupToBeUpdated.Id)
                            {
                                group.IsDefault = false;
                                _context.Update(group);
                            }
                        }
                    }
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
            catch
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
