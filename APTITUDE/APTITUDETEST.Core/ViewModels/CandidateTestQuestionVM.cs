namespace AptitudeTest.Core.ViewModels
{
    public class CandidateTestQuestionVM
    {
        public int Id { get; set; }
        public int Difficulty { get; set; }
        public string? QuestionText { get; set; }
        public int QuestionType { get; set; }
        public int Topic { get; set; }
        public int OptionType { get; set; }
        public int NextQuestionId { get; set; }
        public int QuestionNumber { get; set; }
        public int TotalQuestions { get; set; }
        public List<CandidateTestOptionsVM> Options { get; set; } = new List<CandidateTestOptionsVM>();
        public List<CandidateTestAnswerVM> Answers { get; set; } = new List<CandidateTestAnswerVM>();
    }
}
