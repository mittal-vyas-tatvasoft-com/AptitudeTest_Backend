namespace AptitudeTest.Core.ViewModels
{
    public class TestsViewModel
    {
        public int Id { get; set; }
        public string? TestName { get; set; }
        public string? GroupName { get; set; }
        public int TestTime { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int NoOfCandidates { get; set; }
        public int Status { get; set; }
        public int? TotalRecords { get; set; }
        public int? TotalPages { get; set; }
        public int? NextPage { get; set; }
    }
}
