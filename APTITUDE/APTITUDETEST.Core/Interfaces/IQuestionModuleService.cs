﻿using AptitudeTest.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Core.Interfaces
{
    public interface IQuestionModuleService
    {
        public Task<JsonResult> GetQuestionModules(string? searchQuery, int? filter, int? currentPageIndex, int? pageSize);
        public Task<JsonResult> Create(QuestionModuleVM questionModule);
        public Task<JsonResult> Update(QuestionModuleVM questionModule);
        public Task<JsonResult> Get(int id);
        public Task<JsonResult> Delete(int id);
    }
}
