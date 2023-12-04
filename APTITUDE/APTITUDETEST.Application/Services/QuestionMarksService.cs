using AptitudeTest.Core.Entities.Questions;
using AptitudeTest.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Application.Services
{
    public class QuestionMarksService : IQuestionMarksService
    {

        #region Properties
        readonly IQuestionMarksRepository _repository;
        #endregion

        #region Constructor
        public QuestionMarksService(IQuestionMarksRepository repository)
        {
            _repository = repository;
        }
        #endregion

        #region Methods
        public async Task<JsonResult> GetAllQuestionMarks(string? searchQuery, int? currentPageIndex, int? pageSize)
        {
            return await _repository.GetAllQuestionMarks(searchQuery, currentPageIndex, pageSize);
        }

        public async Task<JsonResult> Create(QuestionMarks newMark)
        {
            return await _repository.Create(newMark);
        }
        public async Task<JsonResult> Update(QuestionMarks updatedMark)
        {
            return await _repository.Update(updatedMark);
        }

        public async Task<JsonResult> Delete(int id)
        {
            return await _repository.Delete(id);
        }
        #endregion
    }
}
