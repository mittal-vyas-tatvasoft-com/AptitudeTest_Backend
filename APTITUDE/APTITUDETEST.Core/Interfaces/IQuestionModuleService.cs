using AptitudeTest.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
