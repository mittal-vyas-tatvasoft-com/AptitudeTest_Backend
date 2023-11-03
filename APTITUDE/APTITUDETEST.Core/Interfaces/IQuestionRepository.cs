using AptitudeTest.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Core.Interfaces
{
    public interface IQuestionRepository
    {
        public Task<JsonResult> Create(QuestionVM question);
        public Task<JsonResult> Get(int id);
        public Task<JsonResult> GetQuestions(int? topic, bool? status, int pageSize, int pageIndex);
        public Task<JsonResult> GetQuestionCount(int? topic, bool? status);
        public Task<JsonResult> UpdateStatus(StatusVM status);
        public Task<JsonResult> Update(QuestionVM question);
        public Task<JsonResult> Delete(int id);
    }
}
