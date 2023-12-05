
using AptitudeTest.Core.Entities.Questions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AptitudeTest.Core.Entities.Test
{
    public class TestQuestions
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [ForeignKey("Tests")]
        public int TestId { get; set; }
        [ForeignKey("QuestionModules")]
        public int TopicId { get; set; }
        public int NoOfQuestions { get; set; }
        public int Weightage { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public bool? IsDeleted { get; set; } = false;
        public virtual Test? Tests { get; set; }
        public virtual QuestionModule QuestionModules { get; set; }
    }
}
