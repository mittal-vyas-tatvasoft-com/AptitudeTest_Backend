using System.ComponentModel.DataAnnotations;

namespace AptitudeTest.Core.ViewModels
{
    public class CreateTempUserTestVM
    {
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int TestId { get; set; }
        public bool Status { get; set; }
        public bool IsFinished { get; set; }
        public int TimeRemaining { get; set; }
        public bool IsAdminApproved { get; set; }
        public int? CreatedBy { get; set; }
    }
}
