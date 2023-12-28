using AptitudeTest.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Core.Interfaces
{
    public interface IQuestionService
    {
        public Task<JsonResult> Create(QuestionVM questionVM);
        public Task<JsonResult> Get(int id);
        public Task<JsonResult> GetQuestions(int? topic, bool? status, int pageSize, int pageIndex);
        public Task<JsonResult> GetQuestionCount(int? topic, bool? status);
        public Task<JsonResult> UpdateStatus(StatusVM status);
        public Task<JsonResult> Update(QuestionVM questionVM);
        public Task<JsonResult> Delete(int id);
        public Task<JsonResult> ImportQuestions(ImportQuestionVM importQuestionVM);
        public Task<JsonResult> UpdateBulkStatus(BulkStatusUpdateVM bulkStatusUpdateVM);
        public Task<JsonResult> DeleteMultipleQuestions(int[] questionIdList);
    }
}
