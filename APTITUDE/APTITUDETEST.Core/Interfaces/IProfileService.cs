﻿using AptitudeTest.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Core.Interfaces
{
    public interface IProfileService
    {
        public Task<JsonResult> GetProfiles(string? searchQuery, int? filter, int? currentPageIndex, int? pageSize);

        public Task<JsonResult> Create(ProfileVM profile);
        public Task<JsonResult> Update(ProfileVM profile);

        public Task<JsonResult> CheckUncheckAll(bool check);

        public Task<JsonResult> Delete(int id);
    }
}
