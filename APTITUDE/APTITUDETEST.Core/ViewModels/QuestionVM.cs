using System.ComponentModel.DataAnnotations;

namespace AptitudeTest.Core.ViewModels
{
    public class QuestionVM
    {
        public string? Sequence { get; set; }
        public int Id { get; set; }
        public int DuplicateFromQuestionId { get; set; }
        [Required]
        [Range(1, 5)]
        public int TopicId { get; set; }
        [Required]
        [Range(1, 5)]
        public int Difficulty { get; set; }
        [Required]
        public bool Status { get; set; }
        [Required]
        public string QuestionText { get; set; }
        [Required]
        [Range(1, 2)]
        public int QuestionType { get; set; }
        [Required]
        [Range(1, 2)]
        public int OptionType { get; set; }
        public List<OptionVM> Options { get; set; } = new List<OptionVM>();
        public int? CreatedBy { get; set; } = 1;
        public int? UpdatedBy { get; set; }
        public int? ParentId { get; set; }
    }
}
