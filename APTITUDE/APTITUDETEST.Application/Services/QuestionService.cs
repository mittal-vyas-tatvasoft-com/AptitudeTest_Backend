using AptitudeTest.Core.Interfaces;
using AptitudeTest.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Application.Services
{
    public class QuestionService : IQuestionService
    {
        #region Properties
        private readonly IQuestionRepository _repository;
        #endregion

        #region Constructor
        public QuestionService(IQuestionRepository repository)
        {
            _repository = repository;
        }
        #endregion

        #region Methods
        public async Task<JsonResult> Create(QuestionVM question)
        {
            return await _repository.Create(question);
        }

        public async Task<JsonResult> Get(int id)
        {
            return await _repository.Get(id);
        }

        public async Task<JsonResult> GetQuestions(int? topic, bool? status, int pageSize, int pageIndex)
        {
            return await _repository.GetQuestions(topic, status, pageSize, pageIndex);
        }

        public async Task<JsonResult> GetQuestionCount(int? topic, bool? status)
        {
            return await _repository.GetQuestionCount(topic, status);
        }

        public async Task<JsonResult> UpdateStatus(StatusVM status)
        {
            return await _repository.UpdateStatus(status);
        }

        public async Task<JsonResult> Update(QuestionVM question)
        {
            return await _repository.Update(question);
        }

        public async Task<JsonResult> Delete(int id)
        {
            return await _repository.Delete(id);
        }
        #endregion
    }
}
