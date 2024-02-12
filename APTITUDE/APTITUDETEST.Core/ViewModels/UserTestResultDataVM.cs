namespace AptitudeTest.Core.ViewModels
{
    public class UserTestResultDataVM
    {
        public string Name { get; set; }
        public int QuestionId { get; set; }
        public int[] UserAnswers { get; set; }
        public bool IsAttended { get; set; }
        public int Difficulty { get; set; }
        public int TimeSpent { get; set; }
        public string? QuestionText { get; set; }
        public int QuestionType { get; set; }
        public int OptionType { get; set; }
        public int OptionId { get; set; }
        public string? OptionData { get; set; }
        public bool IsAnswer { get; set; }

    }
}
