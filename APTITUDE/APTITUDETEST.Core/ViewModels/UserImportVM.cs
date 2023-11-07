namespace AptitudeTest.Core.ViewModels
{
    public class UserImportVM
    {
        public string firstname { get; set; }
        public string email { get; set; }
        public long contactnumber { get; set; }
        public int? groupid { get; set; } = 0;
        public int? collegeid { get; set; } = 0;
    }
}
