namespace AptitudeTest.Core.ViewModels.Master
{
    public class CollegeVM
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public bool? Status { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
    }
}
