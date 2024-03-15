using System.ComponentModel.DataAnnotations;

namespace AptitudeTest.Core.ViewModels
{
    public class UpdateUserTestStatusVM
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public bool IsActive { get; set; }
    }
}
