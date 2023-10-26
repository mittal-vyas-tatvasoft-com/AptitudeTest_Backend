namespace AptitudeTest.Core.ViewModels
{
    public class QuestionVM
    {
        public int Id { get; set; }
        public int TopicId { get; set; }
        public int Difficulty { get; set; }
        public bool Status { get; set; }
        public string QuestionText { get; set; }
        public int QuestionType { get; set; }
        public int OptionType { get; set; }
        public int Answer { get; set; }
        public List<int> AnswerList { get; set; }
        public List<OptionVM> Options { get; set; }
    }
}
