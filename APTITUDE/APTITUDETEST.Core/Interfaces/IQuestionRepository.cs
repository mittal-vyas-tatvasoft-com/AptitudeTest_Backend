using AptitudeTest.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Core.Interfaces
{
    public interface IQuestionRepository
    {
        public Task<JsonResult> Create(QuestionVM question);
        public Task<JsonResult> Get(int id);
    }
}
