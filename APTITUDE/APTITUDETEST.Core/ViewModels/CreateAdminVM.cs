using System.ComponentModel.DataAnnotations;

namespace AptitudeTest.Core.ViewModels
{
    public class CreateAdminVM
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        [Required]
        [RegularExpression(@"^\w+([.-]?\w+)*@\w+([.-]?\w+)*(.\w{2,3})+$", ErrorMessage = "Invalid email format.")]

        public string Email { get; set; }
        [Required]
        [Range(1000000000, 9999999999, ErrorMessage = "The PhoneNumber must be a 10-digit number.")]
        public long PhoneNumber { get; set; }
        [Required]
        public bool Status { get; set; }
        [Required]
        public int CreatedBy { get; set; } = 1;
    }
}
