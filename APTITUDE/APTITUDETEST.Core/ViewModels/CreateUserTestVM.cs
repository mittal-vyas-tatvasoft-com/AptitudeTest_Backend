using System.ComponentModel.DataAnnotations;

namespace AptitudeTest.Core.ViewModels
{
    public class CreateUserTestVM
    {
        public int Id { get; set; }
        [Required]
        public int? UserId { get; set; }
        [Required]
        public int? TestId { get; set; }
        public bool Status { get; set; }
        public bool IsFinished { get; set; }
        public int? CreatedBy { get; set; }
    }
}
