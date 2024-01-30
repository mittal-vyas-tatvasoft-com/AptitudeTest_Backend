namespace AptitudeTest.Core.ViewModels
{
    public class ResultsVM
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CollegeName { get; set; }
        public DateTime StartTime { get; set; }
        public decimal Points { get; set; }
        public int CorrectMarks { get; set; }
        public int CorrectCount { get; set; }
        public decimal WrongMarks { get; set; }
        public int WrongCount { get; set; }
        public int UnAnsweredCount { get; set; }
        public int UnDisplayedCount { get; set; }
        public int UserTestId { get; set; }
        public string Status { get; set; }
        public int? TotalRecords { get; set; }
        public int? TotalPages { get; set; }
        public int? NextPage { get; set; }
    }
}
