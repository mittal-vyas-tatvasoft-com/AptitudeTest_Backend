
using System.ComponentModel.DataAnnotations;

namespace AptitudeTest.Core.ViewModels
{
    public class CreateUserTestResultVM
    {
        public int Id { get; set; }
        [Required]
        public int UserTestId { get; set; }
        [Required]
        public int QuestionId { get; set; }
        public int[]? UserAnswers { get; set; }
        public bool IsAttended { get; set; }
        public int? CreatedBy { get; set; }
    }
}
