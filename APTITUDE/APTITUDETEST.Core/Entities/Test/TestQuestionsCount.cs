
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AptitudeTest.Core.Entities.Test
{
    public class TestQuestionsCount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [ForeignKey("TestQuestions")]
        public int TestQuestionId { get; set; }
        [Range(1, 2)]
        public int QuestionType { get; set; }
        public int OneMarks { get; set; }
        public int TwoMarks { get; set; }
        public int ThreeMarks { get; set; }
        public int FourMarks { get; set; }
        public int FiveMarks { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public bool? IsDeleted { get; set; } = false;
        public virtual TestQuestions? TestQuestions { get; set; }
    }
}
