namespace AptitudeTest.Core.ViewModels
{
    public class StartTestVM
    {
        public string MessageAtStartOfTheTest { get; set; }
        public DateTime EndTime { get; set; }
        public string TestName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime TestDate { get; set; }
        public int TestDurationInMinutes { get; set; }
        public int BasicPoints { get; set; }
        public int NegativeMarkingPoints { get; set; }
    }
}
