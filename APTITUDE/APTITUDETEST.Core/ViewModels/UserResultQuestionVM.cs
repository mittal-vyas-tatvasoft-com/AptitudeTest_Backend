namespace AptitudeTest.Core.ViewModels
{
    public class UserResultQuestionVM
    {
        public int Id { get; set; }
        public int Difficulty { get; set; }
        public int TimeSpentInSeconds { get; set; }
        public string TimeSpent { get; set; }
        public string QuestionText { get; set; } = String.Empty;
        public int OptionType { get; set; }
        public List<int> UserAnswers { get; set; } = new List<int>();
        public List<UserResultOptionVM> Options { get; set; } = new List<UserResultOptionVM>();
    }
}
