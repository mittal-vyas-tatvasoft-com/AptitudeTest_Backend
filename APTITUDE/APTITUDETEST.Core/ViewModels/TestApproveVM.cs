using System.ComponentModel.DataAnnotations;

namespace AptitudeTest.Core.ViewModels
{
    public class TestApproveVM
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int TestId { get; set; }
        [Required]
        public int RemainingTimeInMinutes { get; set; }
        public int? duration { get; set; } = 0;
    }
}
