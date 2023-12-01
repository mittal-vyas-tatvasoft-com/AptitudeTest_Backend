namespace AptitudeTest.Core.ViewModels
{
    public class TempQuestionStatusVM
    {
        public int QuestionId { get; set; }
        public bool IsAttended { get; set; }
        public int[] UserAnswers { get; set; }
    }
}
