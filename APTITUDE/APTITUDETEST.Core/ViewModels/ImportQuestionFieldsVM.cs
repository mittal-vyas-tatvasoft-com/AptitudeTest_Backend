using System.ComponentModel.DataAnnotations;

namespace AptitudeTest.Core.ViewModels
{
    public class ImportQuestionFieldsVM
    {
        public string topic { get; set; }
        public int? topicid { get; set; }
        [Required]
        [Range(1, 5)]
        public int difficulty { get; set; }
        [Required]
        public bool status { get; set; }
        [Required]
        public string questiontext { get; set; }
        [Required]
        [Range(1, 2)]
        public int questiontype { get; set; }
        [Required]
        [Range(1, 1)]
        public int optiontype { get; set; }
        [Required]
        public string optiondata1 { get; set; }
        [Required]
        public bool isanswer1 { get; set; }
        [Required]
        public string optiondata2 { get; set; }
        [Required]
        public bool isanswer2 { get; set; }
        [Required]
        public string optiondata3 { get; set; }
        [Required]
        public bool isanswer3 { get; set; }
        [Required]
        public string optiondata4 { get; set; }
        [Required]
        public bool isanswer4 { get; set; }

    }
}
