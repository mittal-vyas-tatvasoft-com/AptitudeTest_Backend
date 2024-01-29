using System.ComponentModel.DataAnnotations;

namespace AptitudeTest.Core.ViewModels
{
    public class UpdateTestTimeVM
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int RemainingTime { get; set; }
    }
}
