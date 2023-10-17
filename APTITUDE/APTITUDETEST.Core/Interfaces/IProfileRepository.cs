﻿using AptitudeTest.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Core.Interfaces
{
    public interface IProfileRepository
    {
        public Task<JsonResult> GetTechnologies(string? searchQuery, int? filter, int? currentPageIndex, int? pageSize);

        public Task<JsonResult> Create(TechnologyVM technology);
        public Task<JsonResult> Update(TechnologyVM technology);
        public Task<JsonResult> CheckUncheckAll(bool check);

        public Task<JsonResult> Delete(int id);
    }
}