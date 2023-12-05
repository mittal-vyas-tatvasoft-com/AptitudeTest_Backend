namespace AptitudeTest.Core.ViewModels
{
    public class UserTestQuestionModelVM
    {
        public int QuestionId { get; set; }
        public int Difficulty { get; set; }
        public string? QuestionText { get; set; }
        public int QuestionType { get; set; }
        public int OptionType { get; set; }
        public int OptionId { get; set; }
        public string? OptionData { get; set; }
        public int[]? Answer { get; set; }
        public int[]? Questions { get; set; }
    }
}
