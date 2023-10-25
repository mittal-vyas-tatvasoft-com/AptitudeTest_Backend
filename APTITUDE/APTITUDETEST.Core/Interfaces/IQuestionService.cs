using AptitudeTest.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AptitudeTest.Core.Interfaces
{
    public interface IQuestionService
    {
        public Task<JsonResult> Create(QuestionVM question);
    }
}
