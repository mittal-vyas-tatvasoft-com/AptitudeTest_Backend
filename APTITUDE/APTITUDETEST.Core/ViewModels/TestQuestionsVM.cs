using System.ComponentModel.DataAnnotations;

namespace AptitudeTest.Core.ViewModels
{
    public class TestQuestionsVM
    {
        [Required]
        public int TestId { get; set; }
        [Required]
        public int TopicId { get; set; }
        public int NoOfQuestions { get; set; }
        public int? Weightage { get; set; }
        [Required]
        public List<TestQuestionsCountVM> TestQuestionsCount { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
    }
}
