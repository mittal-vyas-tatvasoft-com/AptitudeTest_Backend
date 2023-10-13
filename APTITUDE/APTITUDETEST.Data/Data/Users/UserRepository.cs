using AptitudeTest.Core.Entities.Master;
using AptitudeTest.Core.Entities.Users;
using AptitudeTest.Core.Interfaces.Users;
using AptitudeTest.Core.ViewModels;
using AptitudeTest.Core.ViewModels.User;
using AptitudeTest.Data.Common;
using APTITUDETEST.Common.Data;
using APTITUDETEST.Core.Entities.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AptitudeTest.Data.Data.Users
{
    public class UserRepository : RepositoryBase<User>, IUsersRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<JsonResult> Create(UserVm userVm)
        {
            try
            {
                var user = new User();
                User userExisted = _context.Users.Where(u => (u.Email.ToLower() == userVm.Email.ToLower()) && u.Id != userVm.Id && u.IsDeleted != true).FirstOrDefault();
                if (userExisted != null)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.AlreadyExists, "User"),
                        Result = false,
                        StatusCode = ResponseStatusCode.AlreadyExist
                    });
                }

                user.FirstName = userVm.FirstName;
                user.LastName = userVm.LastName;
                user.FatherName = userVm.FatherName;
                user.Email = userVm.Email;
                user.DateOfBirth = userVm.DateOfBirth;
                user.Level = userVm.Level;
                user.PhoneNumber = userVm.PhoneNumber;
                user.CreatedDate = DateTime.UtcNow;
                user.CreatedBy = userVm.CreatedBy;
                user.PermanentAddress = userVm.PermanentAddress;
                user.ACPCMeritRank = userVm.ACPCMeritRank;
                user.JEEScore = userVm.JEEScore;
                user.GUJCETScore = userVm.GUJCETScore;
                user.Group = userVm.Group;
                user.AppliedThrough = userVm.AppliedThrough;
                user.PreferedLocation = userVm.PreferedLocation;
                user.TechnologyInterestedIn = userVm.TechnologyInterestedIn;
                user.RelationshipWithExistingEmployee = userVm.RelationshipWithExistingEmployee;
                user.Password = userVm.Password;

                var academicsList = new List<UserAcademics>();
                if (userVm.UserAcademics.Count > 0)
                {
                    foreach (var academics in userVm.UserAcademics)
                    {
                        academicsList.Add(new UserAcademics()
                        {
                            CreatedDate = DateTime.UtcNow,
                            CreatedBy = userVm.CreatedBy,
                            DegreeId = academics.DegreeSpecializationId,
                            University = academics.SchoolOrCollegeOrUniversity,
                            StreamId = academics.StreamId,
                            Grade = academics.Grade,
                            Maths = academics.Maths,
                            Physics = academics.Physics,
                            DurationFromMonth = academics.DurationFromMonth,
                            DurationFromYear = academics.DurationFromYear,
                            DurationToMonth = academics.DurationToMonth,
                            DurationToYear = academics.DurationToYear,
                        }); ;
                    }

                }
                var familyList = new List<UserFamily>();
                if (userVm.UserFamily.Count > 0)
                {
                    foreach (var family in userVm.UserFamily)
                    {
                        familyList.Add(new UserFamily()
                        {
                            CreatedDate = DateTime.UtcNow,
                            CreatedBy = userVm.CreatedBy,
                            FamilyPerson = family.RelationShipId,
                            Qualification = family.Qualification,
                            Occupation = family.Occupation,
                        });
                    }
                }

                user.UserAcademics = academicsList;
                user.UserFamily = familyList;

                Create(user);
                var userId = _context.SaveChanges();

                if (userId > 0)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.AddSuccess, "User"),
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }
                else
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.Addfailed, "User"),
                        Result = true,
                        StatusCode = ResponseStatusCode.RequestFailed
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
        public async Task<JsonResult> Update(UserVm userVm)
        {
            try
            {
                var user = new User();
                User userExisted = _context.Users.Where(u => (u.Email.ToLower() == userVm.Email.ToLower()) && u.Id != userVm.Id && u.IsDeleted != true).FirstOrDefault();
                if (userExisted != null)
                {
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.AlreadyExists, "User"),
                        Result = false,
                        StatusCode = ResponseStatusCode.AlreadyExist
                    });
                }
                User userEdit = await Task.FromResult(_context.Users.Include(x => x.UserAcademics).Include(x => x.UserFamily).Where(u => u.Id == userVm.Id && u.IsDeleted != true).FirstOrDefault());
                if (userEdit != null)
                {
                    userEdit.UpdatedDate = DateTime.UtcNow;
                    userEdit.UpdatedBy = userVm.UpdatedBy;
                    userEdit.FirstName = userVm.FirstName;
                    userEdit.LastName = userVm.LastName;
                    userEdit.FatherName = userVm.FatherName;
                    userEdit.Email = userVm.Email;
                    userEdit.DateOfBirth = userVm.DateOfBirth;
                    userEdit.Level = userVm.Level;
                    userEdit.PhoneNumber = userVm.PhoneNumber;
                    userEdit.PermanentAddress = userVm.PermanentAddress;
                    userEdit.ACPCMeritRank = userVm.ACPCMeritRank;
                    userEdit.JEEScore = userVm.JEEScore;
                    userEdit.GUJCETScore = userVm.GUJCETScore;
                    userEdit.Group = userVm.Group;
                    userEdit.AppliedThrough = userVm.AppliedThrough;
                    userEdit.PreferedLocation = userVm.PreferedLocation;
                    userEdit.TechnologyInterestedIn = userVm.TechnologyInterestedIn;
                    userEdit.RelationshipWithExistingEmployee = userVm.RelationshipWithExistingEmployee;
                    userEdit.Password = userVm.Password;

                    var academicsList = new List<UserAcademics>();
                    if (userVm.UserAcademics.Count > 0)
                    {
                        foreach (var academics in userVm.UserAcademics)
                        {
                            academicsList.Add(new UserAcademics()
                            {
                                CreatedDate = DateTime.UtcNow,
                                CreatedBy = userVm.CreatedBy,
                                DegreeId = academics.DegreeSpecializationId,
                                University = academics.SchoolOrCollegeOrUniversity,
                                StreamId = academics.StreamId,
                                Grade = academics.Grade,
                                Maths = academics.Maths,
                                Physics = academics.Physics,
                                DurationFromMonth = academics.DurationFromMonth,
                                DurationFromYear = academics.DurationFromYear,
                                DurationToMonth = academics.DurationToMonth,
                                DurationToYear = academics.DurationToYear,
                            }); ;
                        }

                    }
                    var familyList = new List<UserFamily>();
                    if (userVm.UserFamily.Count > 0)
                    {
                        foreach (var family in userVm.UserFamily)
                        {
                            familyList.Add(new UserFamily()
                            {
                                CreatedDate = DateTime.UtcNow,
                                CreatedBy = userVm.CreatedBy,
                                FamilyPerson = family.RelationShipId,
                                Qualification = family.Qualification,
                                Occupation = family.Occupation,
                            });
                        }

                    }

                    userEdit.UserAcademics = academicsList;
                    userEdit.UserFamily = familyList;


                    Update(userEdit);
                    _context.SaveChanges();

                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.UpdateSuccess, "User"),
                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }

                return new JsonResult(new ApiResponse<string>
                {
                    Message = string.Format(ResponseMessages.NotFound, "User"),
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

                User user = await Task.FromResult(_context.Users.Include(x => x.UserAcademics).Include(x => x.UserFamily).Where(u => u.Id == id && u.IsDeleted == false).FirstOrDefault());
                if (user != null)
                {
                    user.IsDeleted = true;
                    user.UserAcademics.ForEach(x => x.IsDeleted = true);
                    user.UserFamily.ForEach(x => x.IsDeleted = true);

                    Update(user);
                    _context.SaveChanges();
                    return new JsonResult(new ApiResponse<string>
                    {
                        Message = string.Format(ResponseMessages.DeleteSuccess, "User"),

                        Result = true,
                        StatusCode = ResponseStatusCode.Success
                    });
                }
                return new JsonResult(new ApiResponse<string>
                {
                    Message = string.Format(ResponseMessages.NotFound, "User"),
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
    }
}
