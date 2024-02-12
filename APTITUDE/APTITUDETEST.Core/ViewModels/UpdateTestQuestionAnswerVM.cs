using System.ComponentModel.DataAnnotations;


namespace AptitudeTest.Core.ViewModels
{
    public class UpdateTestQuestionAnswerVM
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int TestId { get; set; }
        [Required]
        public int QuestionId { get; set; }
        [Required]
        public int TimeRemaining { get; set; }
        public int TimeSpent { get; set; }
        public int[]? UserAnswers { get; set; }
        public bool IsAttended { get; set; }
    }
}
