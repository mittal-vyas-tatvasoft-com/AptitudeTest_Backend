using Microsoft.AspNetCore.Http;

namespace AptitudeTest.Core.ViewModels
{
    public class OptionVM
    {
        public int OptionId { get; set; }
        public string? OptionValue { get; set; }
        public IFormFile? OptionImage { get; set; }
        public bool IsAnswer { get; set; }
    }
}
