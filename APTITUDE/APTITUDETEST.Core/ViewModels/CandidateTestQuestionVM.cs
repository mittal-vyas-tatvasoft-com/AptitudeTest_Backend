namespace AptitudeTest.Core.ViewModels
{
    public class CandidateTestQuestionVM
    {
        public int Id { get; set; }
        public int Difficulty { get; set; }
        public string QuestionText { get; set; }
        public int QuestionType { get; set; }
        public int OptionType { get; set; }
        public int NextQuestionId { get; set; }
        public int QuestionNumber { get; set; }
        public int TotalQuestions { get; set; }
        public List<string> Options { get; set; } = new List<string>();
        public bool[] Answers { get; set; } = new bool[] { false, false, false, false };
    }
}
