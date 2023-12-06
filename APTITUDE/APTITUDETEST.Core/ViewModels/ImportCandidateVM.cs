using System.ComponentModel.DataAnnotations;

namespace AptitudeTest.Core.ViewModels
{
    public class ImportCandidateVM
    {
        [Required]
        public string firstname { get; set; }
        public string? lastname { get; set; }
        public string? middlename { get; set; }
        [Required]
        [RegularExpression(@"^\w+([.-]?\w+)*@\w+([.-]?\w+)*(.\w{2,3})+$", ErrorMessage = "Invalid email format.")]
        public string email { get; set; }
        [Required]
        [Range(1000000000, 9999999999, ErrorMessage = "The PhoneNumber must be a 10-digit number.")]
        public long contactnumber { get; set; }
        public bool status { get; set; } = true;
    }
}
