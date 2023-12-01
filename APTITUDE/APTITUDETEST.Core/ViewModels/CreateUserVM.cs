using System.ComponentModel.DataAnnotations;

namespace AptitudeTest.Core.ViewModels
{
    public class CreateUserVM
    {
        [Required]
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FatherName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public long? PhoneNumber { get; set; }
        [Required]
        public int GroupId { get; set; }
        [Required]
        public int CollegeId { get; set; }
        public int Gender { get; set; }
        public bool Status { get; set; }
        [Required]
        public int? CreatedBy { get; set; }

    }
}
