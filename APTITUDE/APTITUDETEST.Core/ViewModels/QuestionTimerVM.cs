using System.ComponentModel.DataAnnotations;

namespace AptitudeTest.Core.ViewModels
{
    public class QuestionTimerVM
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int QuestionId { get; set; }
        [Required]
        public int TimeSpent { get; set; }


    }
}
