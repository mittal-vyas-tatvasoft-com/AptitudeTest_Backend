namespace AptitudeTest.Core.ViewModels
{
    public class QuestionDataVM
    {
        public int QuestionId { get; set; }
        public int Topic { get; set; }
        public int Difficulty { get; set; }
        public bool Status { get; set; }
        public string QuestionText { get; set; }
        public int QuestionType { get; set; }
        public int OptionType { get; set; }
        public int OptionId { get; set; }
        public string OptionData { get; set; }
        public bool IsAnswer { get; set; }
    }
}
