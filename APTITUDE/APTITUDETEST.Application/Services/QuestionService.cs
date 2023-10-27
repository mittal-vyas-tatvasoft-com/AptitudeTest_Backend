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

        #endregion
    }
}
