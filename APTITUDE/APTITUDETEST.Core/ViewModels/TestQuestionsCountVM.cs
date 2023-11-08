
using System.ComponentModel.DataAnnotations;

namespace AptitudeTest.Core.ViewModels
{
    public class TestQuestionsCountVM
    {
        [Required]
        [Range(1, 2)]
        public int QuestionType { get; set; }
        [Required]
        public int OneMarkQuestion { get; set; }
        [Required]
        public int TwoMarkQuestion { get; set; }
        [Required]
        public int ThreeMarkQuestion { get; set; }
        [Required]
        public int FourMarkQuestion { get; set; }
        [Required]
        public int FiveMarkQuestion { get; set; }

    }
}
