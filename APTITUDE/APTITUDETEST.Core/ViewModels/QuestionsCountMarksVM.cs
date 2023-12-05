namespace AptitudeTest.Core.ViewModels
{
    public class QuestionsCountMarksVM
    {
        public int TopicId { get; set; }
        public int TotalQuestions { get; set; }
        public int TotalMarks { get; set; }
        public int SingleAnswerCount { get; set; }
        public TestQuestionsCountVM? SingleAnswer { get; set; }
        public int MultiAnswerCount { get; set; }
        public TestQuestionsCountVM? MultiAnswer { get; set; }
    }
}
