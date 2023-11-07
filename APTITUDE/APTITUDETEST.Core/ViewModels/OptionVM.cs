using AptitudeTest.Core.Validations;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace AptitudeTest.Core.ViewModels
{
    public class OptionVM
    {
        public int OptionId { get; set; }
        [MaxLength(100)]
        public string? OptionValue { get; set; }
        [FileExtensionMustBeValid(".png", ".jpg", ".jpeg")]
        [MaxFileSize(100 * 1024)]
        public IFormFile? OptionImage { get; set; }
        public bool IsAnswer { get; set; }
    }
}
