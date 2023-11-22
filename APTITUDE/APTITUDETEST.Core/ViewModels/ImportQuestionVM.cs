using AptitudeTest.Core.Validations;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace AptitudeTest.Core.ViewModels
{
    public class ImportQuestionVM
    {
        [Required(ErrorMessage = "Please select a file.")]
        [FileExtensionMustBeValid(".txt", ".csv")] // Add the allowed extensions you want to allow
        public IFormFile File { get; set; }

    }
}
