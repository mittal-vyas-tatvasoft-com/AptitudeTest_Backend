using AptitudeTest.Core.Entities.Questions;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Core.Interfaces
{
    public interface IQuestionMarksRepository
    {
        public Task<JsonResult> GetAllQuestionMarks(string? searchQuery, int? currentPageIndex, int? pageSize);
        public Task<JsonResult> Create(QuestionMarks newMark);
        public Task<JsonResult> Update(QuestionMarks questionModule);
        public Task<JsonResult> Delete(int id);
    }
}
