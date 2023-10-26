﻿using AptitudeTest.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Core.Interfaces
{

    public interface IUsersRepository
    {
        public Task<JsonResult> GetAllUsers(string? searchQuery, int? currentPageIndex, int? pageSize);
        public Task<JsonResult> GetUserById(int id);
        public Task<JsonResult> Create(UserVM user);
        public Task<JsonResult> Update(UserVM user);
        public Task<JsonResult> InActiveUsers(List<int> userIds);
        public Task<JsonResult> DeleteUsers(List<int> userIds);
    }
}
