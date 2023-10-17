using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AptitudeTest.Application.Services
{
    public class QuestionModuleService :IQuestionModuleService
    {
        #region Properties
        IQuestionModuleRepository _repository;
        #endregion

        #region Constructor
        public QuestionModuleService(IQuestionModuleRepository repository)
        {
            _repository = repository;
        }
        #endregion

        #region Methods
        public async Task<JsonResult> GetQuestionModules(string? searchQuery, int? filter, int? currentPageIndex, int? pageSize)
        {
            return await _repository.GetQuestionModules(searchQuery, filter, currentPageIndex, pageSize);
        }

        public async Task<JsonResult> Create(QuestionModuleVM questionModule)
        {
            return await _repository.Create(questionModule);
        }
        public async Task<JsonResult> Update(QuestionModuleVM questionModule)
        {
            return await _repository.Update(questionModule);
        }

        public async Task<JsonResult> Get(int id)
        {
            return await _repository.Get(id);
        }

        public async Task<JsonResult> Delete(int id)
        {
            return await _repository.Delete(id);
        }

        #endregion
    }
}
