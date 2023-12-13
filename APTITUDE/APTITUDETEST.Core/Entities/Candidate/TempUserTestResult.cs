using AptitudeTest.Core.Entities.Questions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AptitudeTest.Core.Entities.CandidateSide
{
    public class TempUserTestResult
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [ForeignKey("UserTests")]
        public int UserTestId { get; set; }
        [Required]
        [ForeignKey("Questions")]
        public int QuestionId { get; set; }
        public int[]? UserAnswers { get; set; }
        public bool IsAttended { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public bool? IsDeleted { get; set; } = false;
        public virtual TempUserTest UserTests { get; set; }
        public virtual Question Questions { get; set; }
    }
}
