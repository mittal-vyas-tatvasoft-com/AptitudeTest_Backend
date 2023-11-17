namespace AptitudeTest.Core.ViewModels
{
    public class TestQuestionsCountMarksVM
    {
        public int TestId { get; set; }
        public int TotalQuestions { get; set; }
        public int TotalMarks { get; set; }
        public List<QuestionsCountMarksVM> QuestionsCount { get; set; }
    }
}
