namespace AptitudeTest.Core.ViewModels
{
    public class CandidateQuestionsStatusVM
    {
        public List<QuestionStatusVM> questionStatusVMs { get; set; } = new List<QuestionStatusVM>();
        public int TotalQuestion { get; set; }
        public int Answered { get; set; }
        public int UnAnswered { get; set; }
        public int TimeLeft { get; set; }
    }
}
