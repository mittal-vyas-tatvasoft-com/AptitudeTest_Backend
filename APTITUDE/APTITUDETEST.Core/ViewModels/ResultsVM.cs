namespace AptitudeTest.Core.ViewModels
{
    public class ResultsVM
    {
        public int UserId { get; set; }
        public int Points { get; set; }
        public int CorrectMarks { get; set; }
        public int CorrectCount { get; set; }
        public int WrongMarks { get; set; }
        public int WrongCount { get; set; }
        public int UnAnsweredCount { get; set; }
        public int UnDisplayedCount { get; set; }
        public int? TotalRecords { get; set; }
        public int? TotalPages { get; set; }
        public int? NextPage { get; set; }
    }
}
